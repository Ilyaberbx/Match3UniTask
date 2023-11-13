using _Workspace.CodeBase.GamePlay.Progress;
using _Workspace.CodeBase.Infrastructure.Service.SaveLoad;
using _Workspace.CodeBase.Infrastructure.Service.StateMachine.State;
using _Workspace.CodeBase.Service.Progress;
using Cysharp.Threading.Tasks;

namespace _Workspace.CodeBase.GameLoading.StateMachine.State
{
    public class LoadingProgressState : IState
    {
        private const string MatchGameProgressKey = "MatchGameProgress";
        private readonly LoadingStateMachine _loadingStateMachine;
        private readonly IProgressService _progressService;
        private readonly ISaveLoadService _saveLoad;

        public LoadingProgressState(LoadingStateMachine loadingStateMachine
            , IProgressService progressService
            , ISaveLoadService saveLoad)
        {
            _loadingStateMachine = loadingStateMachine;
            _progressService = progressService;
            _saveLoad = saveLoad;
        }

        public async UniTask Enter()
        {
            _progressService.MatchProgress = LoadMatchProgressOrInitNew();
            _loadingStateMachine.Enter<FinishLoadingState>().Forget();
        }

        private MatchGameProgress LoadMatchProgressOrInitNew()
            => _saveLoad.LoadProgress<MatchGameProgress>(MatchGameProgressKey) ?? new MatchGameProgress();

        public UniTask Exit()
            => default;
    }
}