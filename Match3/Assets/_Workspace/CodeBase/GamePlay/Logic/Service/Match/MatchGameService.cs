﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _Workspace.CodeBase.Extensions;
using _Workspace.CodeBase.GamePlay.Configs;
using _Workspace.CodeBase.GamePlay.Handlers;
using _Workspace.CodeBase.GamePlay.Logic.Service.Color;
using _Workspace.CodeBase.GamePlay.Progress;
using _Workspace.CodeBase.Infrastructure.Service.SaveLoad;
using _Workspace.CodeBase.Service.EventBus;
using _Workspace.CodeBase.Service.StaticData;
using Cysharp.Threading.Tasks;
using ModestTree;
using Unity.VisualScripting;
using UnityEngine;

namespace _Workspace.CodeBase.GamePlay.Logic.Service.Match
{
    public class MatchGameService : IMatchGameService, IMatchModel, ISavedProgressReader<MatchGameProgress>,
        ISavedProgressWriter<MatchGameProgress>
    {
        public event Func<Dictionary<UnityEngine.Color, int>, int, UniTask> OnUpdated;

        private const int MinCountToDissolve = 2;

        private readonly IColorProviderService _colorService;
        private readonly IEventBusService _eventBus;
        private readonly IStaticDataService _staticData;
        private IMatchConfig _matchConfig;

        private Dictionary<UnityEngine.Color, int> _collectedColorsMap;

        private MatchBoard _board;
        private int _steps;
        private bool _isMatching;


        public MatchGameService(IColorProviderService colorService
            , IEventBusService eventBus
            , IStaticDataService staticData)
        {
            _colorService = colorService;
            _eventBus = eventBus;
            _staticData = staticData;

            _collectedColorsMap = new Dictionary<UnityEngine.Color, int>();
        }

        public async UniTask InitializeAsync(MatchBoard board)
        {
            _board = board;
            _matchConfig = _staticData.GetMatchConfig();
            await _board.InitializeAsync();
        }

        public async void HandleTileItemClicked(TileItem clickedItem)
        {
            if (_isMatching)
                return;

            HashSet<TileItem> items = new HashSet<TileItem>();
            UnityEngine.Color color = clickedItem.GetColor();

            items = CollectSameColorNeighbourItems(clickedItem, color);

            if (!IsEnoughToDissolve(items.ToList()))
            {
                ResetItems(items.ToList());
                return;
            }

            _steps++;

            if (IsEnoughToGenerateNewColor())
                GenerateNewColor();

            CollectColor(color, items.Count);

            await MatchLogic(color, items);
        }

        private async Task MatchLogic(UnityEngine.Color color, HashSet<TileItem> items)
        {
            _isMatching = true;

            await DissolveItemsInformed(items);

            if (IsEnoughToDeleteColor(color))
            {
                _collectedColorsMap.Remove(color);
                _colorService.RemoveColor(color);

                List<TileItem> allColorItems = _board.GetAllItemsByColor(color);
                TileItem[] unSelectedColorItems = allColorItems.Where(item => !item.Selected()).ToArray();
                
                foreach (TileItem item in unSelectedColorItems) 
                    item.Select();

                items.AddRange(unSelectedColorItems);

                await DissolveItemsInformed(items);
            }

            if (!_colorService.HasColors())
            {
                RaiseGameOver();
                return;
            }

            await UpdateItemsSaved(items);

            if (_board.CanContinueMatching())
                _isMatching = false;
            else
                RaiseGameOver();
        }

        private async UniTask UpdateItemsSaved(HashSet<TileItem> items)
        {
            _board.UpdateItemsColor(items.ToList());
            Dictionary<TileItem, float> crumbledItemsPositionMap = CrumbleItems();
            await MoveToFromPositionAsync(crumbledItemsPositionMap);
            ResetItems(items.ToList());
            await UpdateItems(items.ToList());
            RaiseSaveProgress();
        }

        private async UniTask MoveToFromPositionAsync(Dictionary<TileItem, float> itemsFromPosition) 
            => await _board.MoveItemsFromPositionAsync(itemsFromPosition);

        private HashSet<TileItem> CollectSameColorNeighbourItems(TileItem clickedItem, UnityEngine.Color itemColor)
        {
            HashSet<TileItem> items = new HashSet<TileItem>();
            _board.CollectSameColorNeighbourItems(clickedItem.GetX(), clickedItem.GetY(), itemColor, ref items);
            return items;
        }

        private void GenerateNewColor()
            => _colorService.GenerateNewColor();

        private void RaiseGameOver() =>
            _eventBus.RaiseEvent
                <IGameOverHandler>((handler => handler.HandleGameOver(_steps)));

        private bool IsEnoughToGenerateNewColor()
            => _steps % _matchConfig.StepsPerColor == 0;

        private bool IsEnoughToDeleteColor(UnityEngine.Color color)
            => _collectedColorsMap[color] >= _matchConfig.MatchesToDeleteColor;

        private void CollectColor(UnityEngine.Color color, int count)
        {
            if (_collectedColorsMap.ContainsKey(color))
            {
                _collectedColorsMap[color] += count;
                return;
            }

            _collectedColorsMap.Add(color, count);
        }

        private void ResetItems(List<TileItem> items)
        {
            foreach (TileItem item in items)
                item.Reset();
        }

        private async UniTask UpdateItems(List<TileItem> items) 
            => await _board.UpdateBoard(items);

        private Dictionary<TileItem, float> CrumbleItems()
            => _board.CrumbleItems();

        private async UniTask DissolveItemsInformed(HashSet<TileItem> items)
        {
            await InformBoardUpdated();
            await DissolveItems(items);
        }

        private async UniTask DissolveItems(HashSet<TileItem> items)
        {
            List<UniTask> tasks = Enumerable.Select(items, item => item.Dissolve()).ToList();
            await UniTask.WhenAll(tasks);
        }

        private bool IsEnoughToDissolve(List<TileItem> items)
            => items.Count > MinCountToDissolve;

        private void RaiseSaveProgress() =>
            _eventBus.RaiseEvent<IProgressSaveHandler>
                (handler => handler.SaveProgress());

        private async UniTask InformBoardUpdated()
            => OnUpdated?.Invoke(_collectedColorsMap, _steps);

        public void LoadProgress(MatchGameProgress progress)
        {
            if (progress.BoardData == null) return;

            if (!progress.CollectedColorsData.IsEmpty())
                _collectedColorsMap = progress.CollectedColorsData.ToMap();


            _board.LoadProgress(progress);
            _steps = progress.Steps;

            InformBoardUpdated().Forget();
        }

        public void UpdateProgress(MatchGameProgress progress)
        {
            progress.Steps = _steps;
            progress.CollectedColorsData = _collectedColorsMap.ToDataList();
            _board.UpdateProgress(progress);
        }
    }
}