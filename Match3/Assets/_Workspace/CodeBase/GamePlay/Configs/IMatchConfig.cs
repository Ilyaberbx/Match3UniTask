namespace _Workspace.CodeBase.GamePlay.Configs
{
    public interface IMatchConfig
    {
        public int StepsPerColor { get; }
        
        public int MatchesToDeleteColor { get; }
    }
}