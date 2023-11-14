using System;
using _Workspace.CodeBase.Extensions;
using _Workspace.CodeBase.GamePlay.Assets;
using _Workspace.CodeBase.GamePlay.Logic;
using _Workspace.CodeBase.GamePlay.Logic.Extensions;
using _Workspace.CodeBase.GamePlay.Progress;
using _Workspace.CodeBase.Infrastructure.Service.SaveLoad;
using _Workspace.CodeBase.Service.Factory;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Workspace.CodeBase.GamePlay.Factory
{
    public class MatchFactory : IBoardFactory, IItemsFactory, ITilesFactory
    {
        public const float ReferencedResolutionRationConst = 1.77f;

        private Camera _camera;

        private readonly IPrefabFactoryAsync _prefabFactory;
        private MatchBoard _createMatchBoard;

        private Vector3 _startTilesPosition;
        private float _tileCoefficient;
        private float _tileScaleCoefficient;

        public MatchFactory(IPrefabFactoryAsync prefabFactory)
            => _prefabFactory = prefabFactory;

        public void Initialize(float width, float height)
        {
            _camera = Camera.main;
            _tileCoefficient = CalculateTileCoefficient(width);
            _tileScaleCoefficient = CalculateTileScaleCoefficient();

            Debug.Log(_tileScaleCoefficient);
            _startTilesPosition = MatchStartPosition(height);
        }

        private float CalculateTileCoefficient(float width)
            => (_camera.GetRightUpperCornerWorldPosition().x - _camera.GetLeftUpperCornerWorldPosition().x) /
               (width + 1);

        private float CalculateScreenHeight()
            => _camera.GetLeftUpperCornerWorldPosition().y - _camera.GetLeftLowerCornerWorldPosition().y;

        private Vector3 MatchStartPosition(float height)
            => _camera.GetLeftUpperCornerWorldPosition().AddX(_tileCoefficient)
                .AddY(-_tileCoefficient * (CalculateScreenHeight() - height * _tileCoefficient) / 1.5f)
                .AddZ(10);

        public async UniTask<MatchBoard> CreateMatchBoard()
        {
            MatchBoard boardObject = await InstantiateRegistered<MatchBoard>(GamePlayAssetsAddress.MatchBoard);
            _createMatchBoard = boardObject.GetComponent<MatchBoard>();
            return _createMatchBoard;
        }

        public async UniTask<TileItem> CreateItem()
        {
            TileItem item = await InstantiateRegistered<TileItem>(GamePlayAssetsAddress.TileItem);
            return item;
        }

        public async UniTask<Tile> CreateTile(int x, int y, Transform container)
        {
            Tile tile = await InstantiateRegistered<Tile>(GamePlayAssetsAddress.Tile);
            Transform transform = tile.transform;
            transform.localScale *= _tileScaleCoefficient;
            transform.localPosition = _startTilesPosition.AddX(x * _tileCoefficient).AddY(y * -_tileCoefficient);
            tile.transform.SetParent(container, false);

            tile.SetPosition(x, y);
            return tile;
        }

        private float CalculateTileScaleCoefficient()
        {
            float screenAspectRatio = (float)Screen.height / Screen.width;
            return ReferencedResolutionRationConst / (float)Math.Round(screenAspectRatio, 2);
        }

        private async UniTask<T> InstantiateRegistered<T>(string address) where T : Component
        {
            T instance = await _prefabFactory.Create<T>(address);
            return instance;
        }
    }
}