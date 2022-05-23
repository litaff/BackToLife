using UnityEngine;

namespace BackToLife
{
    public abstract class Tile : Entity
    {
        public TileType tileType;

        public virtual void OnInteract()
        {
            
        }

        
        public enum TileType
        {
            None,
            EndTile,
            TeleportTile
        }
    }
}