using _Workspace.CodeBase.GamePlay.Logic;
using _Workspace.CodeBase.Infrastructure.Service.EventBus.Handlers;

namespace _Workspace.CodeBase.GamePlay.Handlers
{
    public interface IItemClickedHandler : IGlobalSubscriber
    {
        void HandleTileItemClicked(TileItem clickedItem);
    }
}