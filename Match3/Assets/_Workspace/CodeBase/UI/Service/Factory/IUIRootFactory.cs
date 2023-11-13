using Cysharp.Threading.Tasks;

namespace _Workspace.CodeBase.UI.Service.Factory
{
    public interface IUIRootFactory
    {
        UniTask<UIRoot> CreateUIRoot();
    }
}