using _Workspace.CodeBase.GamePlay.Handlers;
using Cysharp.Threading.Tasks;

namespace _Workspace.CodeBase.GamePlay.Logic.Service.Match
{
    public interface IMatchGameService : IItemClickedHandler
    {
        UniTask InitializeAsync(MatchBoard board);
    }
}