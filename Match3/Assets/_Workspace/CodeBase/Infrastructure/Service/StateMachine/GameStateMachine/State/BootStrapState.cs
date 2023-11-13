﻿using System.Threading.Tasks;
using _Workspace.CodeBase.Infrastructure.Service.Assets;
using _Workspace.CodeBase.Infrastructure.Service.StateMachine.State;
using _Workspace.CodeBase.Service.StaticData;
using _Workspace.CodeBase.UI.LoadingCurtain;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Workspace.CodeBase.Infrastructure.Service.StateMachine.GameStateMachine.State
{
    public class BootStrapState : IState
    {
        private readonly LoadingCurtainProxy _curtainProxy;
        private readonly GameStateMachine _gameStateMachine;
        private readonly IAssetsProvider _assets;
        private readonly IStaticDataService _staticData;

        public BootStrapState(LoadingCurtainProxy curtainProxy, IAssetsProvider assets, IStaticDataService staticData,
            GameStateMachine gameStateMachine)
        {
            _curtainProxy = curtainProxy;
            _assets = assets;
            _staticData = staticData;
            _gameStateMachine = gameStateMachine;
        }

        public async UniTask Enter()
        {
            UnlockFrameRate();
            await InitializeServices();
            _gameStateMachine.Enter<GameLoadingState>().Forget();
        }

        private async UniTask InitializeServices()
        {
            await _assets.InitializeAsync();
            await _staticData.InitializeAsync();
            await _curtainProxy.InitializeAsync();
        }

        private void UnlockFrameRate()
            => Application.targetFrameRate = 120;

        public UniTask Exit()
            => default;
    }
}