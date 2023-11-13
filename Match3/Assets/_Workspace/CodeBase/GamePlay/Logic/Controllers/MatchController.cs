using System;
using _Workspace.CodeBase.GamePlay.Logic.Service.Match;
using _Workspace.CodeBase.UI.Match;

namespace _Workspace.CodeBase.GamePlay.Logic.Controllers
{
    public class MatchController : IDisposable
    {
        private readonly IMatchModel _model;
        private readonly IMatchWindow _window;

        public MatchController(IMatchModel model, IMatchWindow window)
        {
            _model = model;
            _window = window;
            
            model.OnUpdated += window.UpdateMatchStats;
        }

        public void Dispose() 
            =>  _model.OnUpdated -= _window.UpdateMatchStats;
    }
}