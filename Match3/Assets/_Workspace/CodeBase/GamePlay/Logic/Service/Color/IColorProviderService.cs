namespace _Workspace.CodeBase.GamePlay.Logic.Service.Color
{
    public interface IColorProviderService
    {
        UnityEngine.Color GetRandomColor();
        void GenerateNewColor();
        void RemoveColor(UnityEngine.Color color);
        bool HasColors();
    }
}