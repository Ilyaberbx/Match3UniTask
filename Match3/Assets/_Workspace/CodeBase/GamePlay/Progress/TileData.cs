namespace _Workspace.CodeBase.GamePlay.Progress
{
    [System.Serializable]
    public class TileData
    {
        public TileItemData ItemData;
        
        public TileData(TileItemData itemData) 
            => ItemData = itemData;
    }
}