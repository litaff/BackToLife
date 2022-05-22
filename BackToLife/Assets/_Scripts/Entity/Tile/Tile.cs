using UnityEngine;

namespace BackToLife
{
    public abstract class Tile : Entity
    {
        public TileType tileType;

        public virtual bool OnInteract()
        {
            return false;
        }

        
        public enum TileType
        {
            None,
            EndTile,
            TeleportTile
        }
    }
}