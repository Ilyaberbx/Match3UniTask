using System.Threading.Tasks;
using _Workspace.CodeBase.GamePlay.Handlers;
using _Workspace.CodeBase.GamePlay.Progress;
using _Workspace.CodeBase.Infrastructure.Service.StateMachine.GameStateMachine;
using _Workspace.CodeBase.Infrastructure.Service.StateMachine.GameStateMachine.State;
using _Workspace.CodeBase.Infrastructure.Service.StateMachine.State;
using _Workspace.CodeBase.Service.EventBus;
using _Workspace.CodeBase.Service.Progress;
using _Workspace.CodeBase.UI.GameOver;
using _Workspace.CodeBase.UI.Service.Factory;
using Cysharp.Threading.Tasks;

namespace _Workspace.CodeBase.GamePlay.StateMachine.State
{
    public class GameOverState : IPayloadedState<int>, IGameRestartHandler
    {
        private const string MatchProgressKey = "MatchGameProgress";
        
        private readonly IGameOverWindowFactory _gameOverWindowFactory;
        private readonly IEventBusService _eventBus;
        private readonly IProgressService _progressService;
        private readonly GameStateMachine _gameStateMachine;


        public GameOverState(IGameOverWindowFactory gameOverWindowFactory
            , IEventBusService eventBus
            ,IProgressService progressService
            ,GameStateMachine gameStateMachine)
        {
            _gameOverWindowFactory = gameOverWindowFactory;
            _eventBus = eventBus;
            _progressService = progressService;
            _gameStateMachine = gameStateMachine;
        }

        public async UniTask Enter(int totalSteps)
        {
            _eventBus.Subscribe(this);
            GameOverWindow view = await _gameOverWindowFactory.CreateGameOverWindow();
            view.UpdateContent(totalSteps);
        }

        public async UniTask Exit()
            => _eventBus.CleanUp();

        public void HandleGameRestart()
        {
            _progressService.MatchProgress = new MatchGameProgress();
            _gameStateMachine.Enter<GamePlayState>().Forget();
        }
    }
}