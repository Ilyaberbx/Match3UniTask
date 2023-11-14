using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _Workspace.CodeBase.Extensions;
using _Workspace.CodeBase.GamePlay.Factory;
using _Workspace.CodeBase.GamePlay.Logic.Service.Color;
using _Workspace.CodeBase.GamePlay.Progress;
using _Workspace.CodeBase.Infrastructure.Service.SaveLoad;
using Cysharp.Threading.Tasks;
using ModestTree;
using UnityEngine;
using Zenject;

namespace _Workspace.CodeBase.GamePlay.Logic
{
    public class MatchBoard : MonoBehaviour, ISavedProgressReader<MatchGameProgress>,
        ISavedProgressWriter<MatchGameProgress>
    {
        [Header("Board settings")] [SerializeField]
        private int _width;

        [SerializeField] private int _height;

        private Tile[,] _tiles;
        private IItemsFactory _itemsFactory;
        private ITilesFactory _tilesFactory;
        private IColorProviderService _colorProvider;
        private TileItemData itemData;
        private TileItem item;

        [Inject]
        public void Construct(IItemsFactory itemsFactory, ITilesFactory tilesFactory,
            IColorProviderService colorProvider)
        {
            _itemsFactory = itemsFactory;
            _tilesFactory = tilesFactory;
            _colorProvider = colorProvider;
        }

        public async UniTask InitializeAsync()
        {
            _tilesFactory.Initialize(_width, _height);
            _tiles = await InitializeTiles();
            await InitializeItems();
        }

        private async UniTask<Tile[,]> InitializeTiles()
        {
            Tile[,] tiles = new Tile[_width, _height];
            Transform container = transform;

            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    Tile tile = await _tilesFactory.CreateTile(j, i, container);
                    tiles[j, i] = tile;
                }
            }

            return tiles;
        }

        private List<TileItem> GetAllItems()
        {
            List<TileItem> items = new List<TileItem>();

            foreach (Tile tile in _tiles)
                items.Add(tile.Item);

            return items;
        }

        private async UniTask InitializeItems()
        {
            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++) _tiles[j, i].SetItem(await _itemsFactory.CreateItem());
            }
        }

        public void UpdateItemsColor(List<TileItem> items)
        {
            foreach (TileItem item in items)
                item.UpdateContent(_colorProvider.GetRandomColor());
        }

        public async UniTask UpdateBoard(List<TileItem> items)
        {
            List<UniTask> tasks = new List<UniTask>();

            foreach (TileItem item in items)
                tasks.Add(item.Appear());

            await UniTask.WhenAll(tasks);
        }

        public void CollectSameColorNeighbourItems(int x, int y, Color color, ref HashSet<TileItem> items)
        {
            CollectIfValid(x - 1, y, color, ref items);
            CollectIfValid(x + 1, y, color, ref items);
            CollectIfValid(x, y - 1, color, ref items);
            CollectIfValid(x, y + 1, color, ref items);
        }

        private void CollectIfValid(int x, int y, Color color, ref HashSet<TileItem> items)
        {
            if (IsValidPosition(x, y))
                CollectSameColorItems(x, y, color, ref items);
        }

        private bool IsValidPosition(int x, int y)
            => IsValidWidth(x) && IsValidHeight(y);

        private bool IsValidHeight(int y)
            => y >= 0 && IsLessThenHeight(y);

        private bool IsLessThenHeight(int y)
            => y < _height;

        private bool IsValidWidth(int x)
            => x >= 0 && x < _width;

        private void CollectSameColorItems(int x, int y, Color color, ref HashSet<TileItem> items)
        {
            if (!TryCollectItem(x, y, color, out TileItem item)) return;

            items.Add(item);

            CollectSameColorNeighbourItems(x, y, color, ref items);
        }

        public List<TileItem> GetAllItemsByColor(Color color)
            => GetAllItems().Where(item => item.GetColor() == color).ToList();

        private bool TryCollectItem(int x, int y, Color color, out TileItem item)
        {
            item = _tiles[x, y].Item;

            if (item == null)
                return false;

            if (item.GetColor() != color || item.Selected())
                return false;

            item.Select();
            return true;
        }

        public bool CanContinueMatching()
        {
            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    TileItem item = _tiles[j, i].Item;
                    HashSet<TileItem> items = new HashSet<TileItem>();

                    CollectSameColorNeighbourItems(j, i, item.GetColor(), ref items);

                    if (items.IsEmpty()) continue;

                    foreach (TileItem selectedItem in items)
                        selectedItem.Deselect();

                    if (items.Count >= 3)
                        return true;
                }
            }

            return false;
        }


        public Dictionary<TileItem, float> CrumbleItems()
        {
            Dictionary<TileItem, float> crumbledItemsMap = new Dictionary<TileItem, float>();

            for (int i = 0; i < _width; i++)
            {
                for (int j = 1; j < _height + 1; j++)
                {
                    Debug.Log("X: " + i + " Y: " + j);
                    TileItem item = _tiles[i, _height - j].Item;
                    float itemPositionY = item.transform.position.y;

                    if (item.Selected())
                        continue;

                    if (!IsLessThenHeight(item.GetY() + 1))
                        continue;

                    if (!TryFindBottomItem(true, i, item.GetY(), out TileItem bottomItem)) continue;

                    float bottomItemPositionY = item.transform.position.y;

                    if (crumbledItemsMap.ContainsKey(bottomItem))
                        crumbledItemsMap[bottomItem] = itemPositionY;
                    else
                        crumbledItemsMap.Add(bottomItem, itemPositionY);


                    if (crumbledItemsMap.ContainsKey(item))
                        crumbledItemsMap[item] = bottomItemPositionY;
                    else
                        crumbledItemsMap.Add(item, bottomItemPositionY);

                    _tiles[bottomItem.GetX(), bottomItem.GetY()]
                        .SwapItems(_tiles[i, _height - j]);
                }
            }

            return crumbledItemsMap;
        }

        private bool TryFindBottomItem(bool isSelected, int x, int rootY, out TileItem item)
        {
            item = null;

            int bottomTilesLenght = _height - rootY;

            for (int i = 1; i < bottomTilesLenght; i++)
            {
                TileItem tempItem = _tiles[x, _height - i].Item;

                if (tempItem.Selected() != isSelected) continue;

                item = tempItem;
                return true;
            }

            return false;
        }

        public void LoadProgress(MatchGameProgress progress)
        {
            TileData[][] tilesData = progress.BoardData?.TilesData;

            void UpdateContentAction(int x, int y, Color color)
                => _tiles[x, y].Item.UpdateContent(color);

            if (tilesData != null)
            {
                for (int i = 0; i < _height; i++)
                {
                    for (int j = 0; j < _width; j++)
                        UpdateContentAction(j, i, tilesData[j][i].ItemData.ColorData.ToUnityColor());
                }
            }
            else
            {
                for (int i = 0; i < _height; i++)
                {
                    for (int j = 0; j < _width; j++)
                        UpdateContentAction(j, i, _colorProvider.GetRandomColor());
                }
            }

            UpdateBoard(GetAllItems()).Forget();
        }


        public void UpdateProgress(MatchGameProgress progress)
        {
            TileData[][] tilesData = new TileData[_width][];

            for (int i = 0; i < _width; i++)
            {
                tilesData[i] = new TileData[_height];

                for (int j = 0; j < _height; j++)
                    tilesData[i][j] = new TileData(itemData: _tiles[i, j].Item.ToItemData());
            }

            progress.BoardData.TilesData = tilesData;
        }

        public async UniTask MoveItemsFromPositionAsync(Dictionary<TileItem, float> itemsPositionMap)
        {
            List<UniTask> tasks = new List<UniTask>();

            foreach (TileItem item in itemsPositionMap.Keys)
                tasks.Add(item.MoveFromPositionAsync(itemsPositionMap[item]));

            await UniTask.WhenAll(tasks);
        }
    }
}