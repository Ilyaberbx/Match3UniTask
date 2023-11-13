using _Workspace.CodeBase.GamePlay.Factory;
using _Workspace.CodeBase.GamePlay.Logic.Controllers;
using _Workspace.CodeBase.GamePlay.Logic.Controllers.Service.Factory;
using _Workspace.CodeBase.GamePlay.Logic.Service;
using _Workspace.CodeBase.GamePlay.Logic.Service.Color;
using _Workspace.CodeBase.GamePlay.Logic.Service.Match;
using _Workspace.CodeBase.GamePlay.StateMachine;
using _Workspace.CodeBase.Service.Factory;
using _Workspace.CodeBase.UI.Service.Factory;
using UnityEngine;
using Zenject;

namespace _Workspace.CodeBase.GamePlay
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private GameplayBootstrapper _bootstrapper;
        public override void InstallBindings()
        {
            BindPrefabFactory();
            BindBoardFactory();
            BindColorProvider();
            BindMatchGameService();
            BindUIFactory();
            BindControllerFactory();
            BindStateMachine();
            BindBootstrapper();
        }

        private void BindControllerFactory() 
            => Container.BindInterfacesTo<MatchControllerFactory>().AsSingle();

        private void BindUIFactory()
            => Container.BindInterfacesTo<UIFactory>().AsSingle();
        private void BindColorProvider()
            => Container.BindInterfacesTo<ColorProviderService>().AsSingle();

        private void BindMatchGameService() 
            => Container.BindInterfacesTo<MatchGameService>().AsSingle();

        private void BindBoardFactory()
            => Container.BindInterfacesTo<MatchFactory>().AsSingle();
        
        private void BindStateMachine() 
            => Container.Bind<GameplayStateMachine>().AsSingle();

        private void BindPrefabFactory() 
            => Container.BindInterfacesTo<PrefabFactoryAsync>().AsSingle();

        private void BindBootstrapper()
            => Container.BindInterfacesTo<GameplayBootstrapper>()
                .FromInstance(_bootstrapper)
                .AsSingle();
    }
}