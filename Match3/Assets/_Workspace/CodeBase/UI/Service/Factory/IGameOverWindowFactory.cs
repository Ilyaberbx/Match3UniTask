using _Workspace.CodeBase.UI.GameOver;
using Cysharp.Threading.Tasks;

namespace _Workspace.CodeBase.UI.Service.Factory
{
    public interface IGameOverWindowFactory
    {
        public UniTask<GameOverWindow> CreateGameOverWindow();
    }
}