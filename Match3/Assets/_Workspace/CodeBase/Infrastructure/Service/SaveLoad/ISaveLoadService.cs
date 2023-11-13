namespace _Workspace.CodeBase.Infrastructure.Service.SaveLoad
{
    public interface ISaveLoadService
    {
        T LoadProgress<T>(string key) where T : class;
        void SaveProgress<T>(string key, T progress) where T : class;

        void ClearUp(string key);
    }
}