using System.Collections.Generic;
using System.Linq;
using _Workspace.CodeBase.Extensions;
using _Workspace.CodeBase.GamePlay.Configs;
using _Workspace.CodeBase.GamePlay.Logic.Service.Color.Data;
using _Workspace.CodeBase.GamePlay.Logic.Service.Color.Factory;
using _Workspace.CodeBase.GamePlay.Logic.Service.ProportionalRandom;
using _Workspace.CodeBase.GamePlay.Progress;
using _Workspace.CodeBase.Infrastructure.Service.SaveLoad;
using _Workspace.CodeBase.Service.StaticData;
using ModestTree;
using UnityEngine;

namespace _Workspace.CodeBase.GamePlay.Logic.Service.Color
{
    public class ColorProviderService : IColorProviderService, ISavedProgressReader<MatchGameProgress>,
        ISavedProgressWriter<MatchGameProgress>
    {
        private readonly IColorGenerationConfig _generationConfig;
        private readonly ColorDataFactory _colorFactory;
        private List<ColorValueData> _colorsWeight;

        public ColorProviderService(IStaticDataService staticData)
        {
            _generationConfig = staticData.GetColorConfig();

            _colorsWeight = new List<ColorValueData>();
            _colorFactory = new ColorDataFactory(_generationConfig.MinRandomWeight, _generationConfig.MaxRandomWeight);
        }

        private void Initialize()
        {
            for (int i = 0; i < _generationConfig.StartColorsCount; i++) 
                GenerateNewColor();
        }

        public void GenerateNewColor()
        {
            if (_colorsWeight.Count >= _generationConfig.MaxColorsCount)
                return;

            Debug.Log("New Color");
            ColorValueData colorWeight = _colorFactory.CreateColorWeight();
            _colorsWeight.Add(colorWeight);
        }

        public bool HasColors() 
            => _colorsWeight.Count > 0;

        public UnityEngine.Color GetRandomColor()
        {
            ProportionalRandomSelector<ColorValueData> randomSelector =
                new ProportionalRandomSelector<ColorValueData>();

            foreach (ColorValueData colorWeight in _colorsWeight)
                randomSelector.AddPercentageItem(colorWeight, colorWeight.Value);

            return randomSelector.SelectItem().Color.ToUnityColor();
        }

        public void RemoveColor(UnityEngine.Color color)
        {
            List<ColorValueData> colorsToRemove =
                _colorsWeight.Where(colorWeight => colorWeight.Color.ToUnityColor() == color).ToList();

            foreach (ColorValueData colorWeight in colorsToRemove)
                _colorsWeight.Remove(colorWeight);
        }

        public void LoadProgress(MatchGameProgress progress)
        {
            if (progress.ColorsWeightData.IsEmpty())
            {
                Initialize();
                return;
            }

            _colorsWeight = progress.ColorsWeightData;
        }

        public void UpdateProgress(MatchGameProgress progress) 
            => progress.ColorsWeightData = _colorsWeight;
    }
}