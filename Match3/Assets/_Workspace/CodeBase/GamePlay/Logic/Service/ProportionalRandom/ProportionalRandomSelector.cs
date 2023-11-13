using System.Collections.Generic;
using UnityEngine;

namespace _Workspace.CodeBase.GamePlay.Logic.Service.ProportionalRandom
{
    public class ProportionalRandomSelector<T> 
    {
        private readonly Dictionary<T, int> _percentageItemsMap;

        public ProportionalRandomSelector() 
            => _percentageItemsMap = new();

        public void AddPercentageItem(T item, int percentage) 
            => _percentageItemsMap.Add(item, percentage);

        public T SelectItem() {
            
            int poolSize = 0;
            
            foreach (int i in _percentageItemsMap.Values) {
                poolSize += i;
            }
            
            int randomNumber = Random.Range(0, poolSize);
            
            int accumulatedProbability = 0;
            
            foreach (KeyValuePair<T, int> pair in _percentageItemsMap) {
                accumulatedProbability += pair.Value;
                if (randomNumber <= accumulatedProbability)
                    return pair.Key;
            }
        
            return default; 
        }

    }
}