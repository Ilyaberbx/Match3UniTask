using _Workspace.CodeBase.Extensions;
using UnityEngine;

namespace _Workspace.CodeBase.Infrastructure.Service.SaveLoad
{
    public class PrefsSaveLoadService : ISaveLoadService
    {
        public T LoadProgress<T>(string key) where T : class
            => PlayerPrefs.GetString(key)?.ToDeserialized<T>();

        public void SaveProgress<T>(string key, T progress) where T : class
            => PlayerPrefs.SetString(key, progress.ToJson());

        public void ClearUp(string key) 
            => PlayerPrefs.DeleteKey(key);
    }
}