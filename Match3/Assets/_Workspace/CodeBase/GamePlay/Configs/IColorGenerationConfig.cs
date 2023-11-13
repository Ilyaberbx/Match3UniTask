namespace _Workspace.CodeBase.GamePlay.Configs
{
    public interface IColorGenerationConfig
    {
        public int MaxColorsCount { get; }
        public int MinRandomWeight { get; }
        public int MaxRandomWeight { get; }
        public int StartColorsCount { get; }
    }
}