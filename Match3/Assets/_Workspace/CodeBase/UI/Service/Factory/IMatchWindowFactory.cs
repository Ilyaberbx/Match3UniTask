using _Workspace.CodeBase.UI.Match;
using Cysharp.Threading.Tasks;

namespace _Workspace.CodeBase.UI.Service.Factory
{
    public interface IMatchWindowFactory
    {
        UniTask<MatchWindow> CreateMatchWindow();
    }
}