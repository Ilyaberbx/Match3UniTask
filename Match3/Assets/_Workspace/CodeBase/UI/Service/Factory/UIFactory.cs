using _Workspace.CodeBase.GamePlay.Assets;
using _Workspace.CodeBase.Service.Factory;
using _Workspace.CodeBase.UI.GameOver;
using _Workspace.CodeBase.UI.Match;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace _Workspace.CodeBase.UI.Service.Factory
{
    public class UIFactory : IMatchWindowFactory, IMatchViewFactory, IUIRootFactory,IGameOverWindowFactory
    {
        private readonly IPrefabFactoryAsync _factory;
        private UIRoot _uiRoot;

        public UIFactory(IPrefabFactoryAsync factory)
            => _factory = factory;

        public async UniTask<UIRoot> CreateUIRoot()
            => _uiRoot = await _factory.Create<UIRoot>(GamePlayAssetsAddress.UIRoot);

        public async UniTask<MatchView> CreateMatchView(VerticalLayoutGroup container)
        {
            MatchView view = await _factory.Create<MatchView>(GamePlayAssetsAddress.MatchView);
            view.transform.SetParent(container.transform);
            return view;
        }

        public async UniTask<MatchWindow> CreateMatchWindow() 
            => await _factory.Create<MatchWindow>(GamePlayAssetsAddress.MatchWindow, Vector3.zero, _uiRoot.transform);

        public async UniTask<GameOverWindow> CreateGameOverWindow() 
            => await _factory.Create<GameOverWindow>(GamePlayAssetsAddress.GameOverWindow, Vector3.zero, _uiRoot.transform);
    }
}