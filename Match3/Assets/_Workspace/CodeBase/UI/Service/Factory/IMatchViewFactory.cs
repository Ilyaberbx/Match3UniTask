using _Workspace.CodeBase.UI.Match;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

namespace _Workspace.CodeBase.UI.Service.Factory
{
    public interface IMatchViewFactory
    {
        UniTask<MatchView> CreateMatchView(VerticalLayoutGroup container);
    }
}