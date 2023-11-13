using System.Threading.Tasks;
using _Workspace.CodeBase.Infrastructure.Service.Assets;
using _Workspace.CodeBase.Infrastructure.Service.Assets.Data;
using _Workspace.CodeBase.Infrastructure.Service.SceneManagement;
using _Workspace.CodeBase.Infrastructure.Service.StateMachine.State;
using _Workspace.CodeBase.UI.LoadingCurtain;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace _Workspace.CodeBase.Infrastructure.Service.StateMachine.GameStateMachine.State
{
    public class GameLoadingState : IState
    {
        private readonly ILoadingCurtain _curtain;
        private readonly ISceneLoader _sceneLoader;
        private readonly IAssetsProvider _assets;

        public GameLoadingState(ILoadingCurtain curtain,ISceneLoader sceneLoader,IAssetsProvider assets)
        {
            _curtain = curtain;
            _sceneLoader = sceneLoader;
            _assets = assets;
        }

        public async UniTask Enter()
        {
            await _curtain.Show();

            await _assets.WarmUpAssetsByLabel(AssetsLabel.GameLoadingState);
            await _sceneLoader.LoadAsync(InfrastructureAssetsAddress.GameLoadingScene, LoadSceneMode.Single);
            
            await _curtain.Hide();
        }

        public async UniTask Exit() 
            => await _assets.ReleaseAssetsByLabel(AssetsLabel.GameLoadingState);
    }
}