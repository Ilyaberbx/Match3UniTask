using System.Collections.Generic;
using _Workspace.CodeBase.GamePlay.Logic.Service.Color.Data;

namespace _Workspace.CodeBase.GamePlay.Progress
{
    [System.Serializable]
    public class MatchGameProgress
    {
        public MatchBoardData BoardData;
        public List<ColorValueData> ColorsWeightData;
        public List<ColorValueData> CollectedColorsData;
        public int Steps;

        public MatchGameProgress()
        {
            BoardData = new MatchBoardData();
            ColorsWeightData = new List<ColorValueData>();
            CollectedColorsData = new List<ColorValueData>();
            Steps = 0;
        }
    }
}