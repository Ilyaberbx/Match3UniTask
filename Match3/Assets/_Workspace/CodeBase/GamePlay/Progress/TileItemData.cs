namespace _Workspace.CodeBase.GamePlay.Progress
{
    [System.Serializable]
    public class TileItemData
    {
        public ColorData ColorData;

        public TileItemData()
        {
        }

        public TileItemData(ColorData colorData)
        {
            ColorData = colorData;
        }
    }
}