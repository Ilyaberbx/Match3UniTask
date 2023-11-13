using _Workspace.CodeBase.GamePlay.Configs;
using _Workspace.CodeBase.Infrastructure.Service.Assets;
using Cysharp.Threading.Tasks;

namespace _Workspace.CodeBase.Service.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string GameplayConfigKey = "GamePlayConfig";
        private readonly IAssetsProvider _assets;
        private GamePlayGenerationConfig _gamePlayGenerationConfig;

        public StaticDataService(IAssetsProvider assets)
            => _assets = assets;

        public async UniTask InitializeAsync()
            => _gamePlayGenerationConfig = await LoadGameplayConfig();

        public IColorGenerationConfig GetColorConfig()
            => _gamePlayGenerationConfig;

        public IMatchConfig GetMatchConfig()
            => _gamePlayGenerationConfig;

        private async UniTask<GamePlayGenerationConfig> LoadGameplayConfig()
            => await _assets.Load<GamePlayGenerationConfig>(GameplayConfigKey);
    }
}