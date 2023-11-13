using _Workspace.CodeBase.GamePlay.Progress;

namespace _Workspace.CodeBase.Service.Progress
{
    public interface IProgressService
    {
        MatchGameProgress MatchProgress { get; set; }
    }
}