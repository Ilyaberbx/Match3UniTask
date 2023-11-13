namespace _Workspace.CodeBase.Infrastructure.Service.SaveLoad
{
    public interface ISavedProgressWriter<in T>
    {
        void UpdateProgress(T progress);
    }
}