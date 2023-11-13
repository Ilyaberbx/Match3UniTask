using _Workspace.CodeBase.Infrastructure.Service.EventBus.Handlers;

namespace _Workspace.CodeBase.GamePlay.Handlers
{
    public interface IGameOverHandler : IGlobalSubscriber
    {
        void HandleGameOver(int totalSteps);
    }
}