using _Workspace.CodeBase.GamePlay.Logic;
using Cysharp.Threading.Tasks;

namespace _Workspace.CodeBase.GamePlay.Factory
{
    public interface IBoardFactory
    {
        UniTask<MatchBoard>   CreateMatchBoard();
    }
}