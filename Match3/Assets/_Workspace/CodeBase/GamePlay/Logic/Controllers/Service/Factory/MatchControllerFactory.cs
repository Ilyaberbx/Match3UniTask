using System;
using System.Collections.Generic;
using _Workspace.CodeBase.GamePlay.Logic.Service.Match;
using _Workspace.CodeBase.UI.Match;
using Zenject;

namespace _Workspace.CodeBase.GamePlay.Logic.Controllers.Service.Factory
{
    public class MatchControllerFactory : IMatchControllerFactory, IDisposable
    {
        private readonly IInstantiator _instantiator;
        private readonly List<IDisposable> _disposables;

        public MatchControllerFactory(IInstantiator instantiator)
        {
            _instantiator = instantiator;
            _disposables = new List<IDisposable>();
        }

        public MatchController CreateMatchController(IMatchModel model, IMatchWindow view)
        {
            MatchController controller = _instantiator.Instantiate<MatchController>(new object[] { model, view });
            _disposables.Add(controller);
            return controller;
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables) 
                disposable.Dispose();
        }
    }
}