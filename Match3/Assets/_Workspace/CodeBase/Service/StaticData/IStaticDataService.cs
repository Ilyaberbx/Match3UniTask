using _Workspace.CodeBase.GamePlay.Configs;
using Cysharp.Threading.Tasks;

namespace _Workspace.CodeBase.Service.StaticData
{
    public interface IStaticDataService
    {
        UniTask InitializeAsync();
        IColorGenerationConfig GetColorConfig();
        IMatchConfig GetMatchConfig();
    }
}