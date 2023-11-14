using System.Collections.Generic;
using _Workspace.CodeBase.GamePlay.Factory;
using _Workspace.CodeBase.GamePlay.Logic;
using _Workspace.CodeBase.GamePlay.Logic.Controllers.Service.Factory;
using _Workspace.CodeBase.GamePlay.Logic.Service.Match;
using _Workspace.CodeBase.GamePlay.Progress;
using _Workspace.CodeBase.Infrastructure.Service.SaveLoad;
using _Workspace.CodeBase.Infrastructure.Service.StateMachine.State;
using _Workspace.CodeBase.Service.EventBus;
using _Workspace.CodeBase.Service.Progress;
using _Workspace.CodeBase.UI.LoadingCurtain;
using _Workspace.CodeBase.UI.Match;
using _Workspace.CodeBase.UI.Service.Factory;
using Cysharp.Threading.Tasks;

namespace _Workspace.CodeBase.GamePlay.StateMachine.State
{
    public class GameplayBootstrapState : IState
    {
        private readonly IBoardFactory _boardFactory;
        private readonly IEventBusService _eventBus;
        private readonly IMatchGameService _matchGame;
        private readonly IMatchControllerFactory _matchControllerFactory;
        private readonly IMatchWindowFactory _matchWindowFactory;
        private readonly IUIRootFactory _rootFactory;
        private readonly IEnumerable<ISavedProgressReader<MatchGameProgress>> _progressReadersService;
        private readonly IProgressService _progressService;
        private readonly ILoadingCurtain _curtain;
        private readonly GameplayStateMachine _gameplayStateMachine;

        public GameplayBootstrapState(IBoardFactory boardFactory
            , IEventBusService eventBus
            , IMatchGameService matchGame
            , IMatchControllerFactory matchControllerFactory
            , IMatchWindowFactory matchWindowFactory
            , IUIRootFactory rootFactory
            , IEnumerable<ISavedProgressReader<MatchGameProgress>> progressReadersService
            , IProgressService progressService
            , ILoadingCurtain curtain
            , GameplayStateMachine gameplayStateMachine)
        {
            _boardFactory = boardFactory;
            _eventBus = eventBus;
            _matchGame = matchGame;
            _matchControllerFactory = matchControllerFactory;
            _matchWindowFactory = matchWindowFactory;
            _rootFactory = rootFactory;
            _progressReadersService = progressReadersService;
            _progressService = progressService;
            _curtain = curtain;
            _gameplayStateMachine = gameplayStateMachine;
        }

        public async UniTask Enter()
        {
            _eventBus.Subscribe(_matchGame);
            
            await _rootFactory.CreateUIRoot();

            InitializeMatchSystem().Forget();

            InformProgressReaders(_progressService.MatchProgress);
            
            _curtain.Hide().Forget();
            
            _gameplayStateMachine.Enter<GameLoopState>().Forget();
        }

        private void InformProgressReaders(MatchGameProgress progress)
        {
            foreach (ISavedProgressReader<MatchGameProgress> reader in _progressReadersService)
                reader.LoadProgress(progress);
        }

        private async UniTask InitializeMatchSystem()
        {
            MatchBoard board = await _boardFactory.CreateMatchBoard();
            MatchWindow view = await _matchWindowFactory.CreateMatchWindow();
            await _matchGame.InitializeAsync(board);
            _matchControllerFactory.CreateMatchController(_matchGame as IMatchModel, view);
        }

        public UniTask Exit()
            => default;
    }
}