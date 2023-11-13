namespace _Workspace.CodeBase.Infrastructure.Service.SaveLoad
{
    public interface ISavedProgressReader<T> where T : class
    {
        void LoadProgress(T progress);
    }
}