using _Workspace.CodeBase.GamePlay.Logic.Service.Color.Data;
using _Workspace.CodeBase.GamePlay.Progress;
using UnityEngine;

namespace _Workspace.CodeBase.GamePlay.Logic.Service.Color.Factory
{
    public class ColorDataFactory : IColorDataFactory
    {
        private readonly int _minWeight;
        private readonly int _maxWeight;

        public ColorDataFactory(int minWeight,int maxWeight)
        {
            _minWeight = minWeight;
            _maxWeight = maxWeight;
        }
        public ColorValueData CreateColorWeight() 
            => new(RandomValue(_minWeight, _maxWeight));


        private int RandomValue(int min, int max) 
            => Random.Range(min, max + 1);
    }
}