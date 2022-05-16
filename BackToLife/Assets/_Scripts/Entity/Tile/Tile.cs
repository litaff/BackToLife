namespace BackToLife
{
    public class Tile : Entity
    {
        public TileType tileType;
        
        public enum TileType
        {
            None,
            EndTile,
            TeleportTile
        }
    }
}