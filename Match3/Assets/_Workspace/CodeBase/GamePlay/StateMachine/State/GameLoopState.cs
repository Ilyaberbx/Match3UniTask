using System.Collections.Generic;
using _Workspace.CodeBase.GamePlay.Handlers;
using _Workspace.CodeBase.GamePlay.Progress;
using _Workspace.CodeBase.Infrastructure.Service.SaveLoad;
using _Workspace.CodeBase.Infrastructure.Service.StateMachine.State;
using _Workspace.CodeBase.Service.EventBus;
using _Workspace.CodeBase.Service.Progress;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Workspace.CodeBase.GamePlay.StateMachine.State
{
    public class GameLoopState : IState, IGameOverHandler, IProgressSaveHandler
    {
        private const string MatchGameProgressKey = "MatchGameProgress";
        private readonly IEventBusService _eventBus;
        private readonly GameplayStateMachine _gameplayStateMachine;
        private readonly ISaveLoadService _saveLoad;
        private readonly IEnumerable<ISavedProgressWriter<MatchGameProgress>> _progressWritersService;
        private readonly IProgressService _progressService;


        public GameLoopState(IEventBusService eventBus
            , GameplayStateMachine gameplayStateMachine
            , ISaveLoadService saveLoad
            , IEnumerable<ISavedProgressWriter<MatchGameProgress>> progressWritersService
            , IProgressService progressService)
        {
            _eventBus = eventBus;
            _gameplayStateMachine = gameplayStateMachine;
            _saveLoad = saveLoad;
            _progressWritersService = progressWritersService;
            _progressService = progressService;
        }

        public async UniTask Enter()
            => _eventBus.Subscribe(this);

        public async UniTask Exit()
            => _eventBus.CleanUp();

        public void HandleGameOver(int totalSteps)
        {
            _saveLoad.ClearUp(MatchGameProgressKey);
            
            _gameplayStateMachine.Enter<GameOverState, int>(totalSteps).Forget();
        }

        public void SaveProgress()
        {
            foreach (ISavedProgressWriter<MatchGameProgress> writer in _progressWritersService)
                writer.UpdateProgress(_progressService.MatchProgress);

            Debug.Log("SaveProgress");
            _saveLoad.SaveProgress(MatchGameProgressKey, _progressService.MatchProgress);
        }
    }
}