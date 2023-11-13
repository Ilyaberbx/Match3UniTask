using _Workspace.CodeBase.GamePlay.Progress;
using UnityEngine;

namespace _Workspace.CodeBase.GamePlay.Logic.Service.Color.Data
{
    [System.Serializable]
    public class ColorValueData
    {
        public int Value;
        public ColorData Color;

        public ColorValueData(int value)
        {
            Value = value;
            Color = new ColorData(Random.value, Random.value, Random.value);
        }

        public ColorValueData()
        {
        }
        public ColorValueData(int value, ColorData colorData)
        {
            Value = value;
            Color = colorData;
        }
    }
}