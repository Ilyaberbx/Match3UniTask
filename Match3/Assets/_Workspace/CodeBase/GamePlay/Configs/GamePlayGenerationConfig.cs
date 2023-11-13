using UnityEngine;

namespace _Workspace.CodeBase.GamePlay.Configs
{
    [CreateAssetMenu(menuName = "Create GamePlayConfig", fileName = "GamePlayConfig", order = 0)]
    public class GamePlayGenerationConfig : ScriptableObject, IColorGenerationConfig, IMatchConfig
    {
        [field: Header("Game settings")] 
        [field: SerializeField] public int MaxColorsCount { get; private set; }
        [field: SerializeField] public int MinRandomWeight { get; private set; }
        [field: SerializeField] public int MaxRandomWeight { get; private set; }
        [field: SerializeField] public int StartColorsCount { get; private set; }
        [field: SerializeField] public int StepsPerColor { get; private set; }
        [field: SerializeField] public int MatchesToDeleteColor { get; private set; }
    }
}