using _Workspace.CodeBase.GamePlay.Logic.Service.Match;
using _Workspace.CodeBase.UI.Match;

namespace _Workspace.CodeBase.GamePlay.Logic.Controllers.Service.Factory
{
    public interface IMatchControllerFactory
    {
        MatchController CreateMatchController(IMatchModel model, IMatchWindow view);
    }
}