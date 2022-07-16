using System;
using UnityEngine;

namespace BackToLife
{
    [Serializable]
    public abstract class Entity : MonoBehaviour
    {
        public Vector2 gridPosition;
        public GameGrid.GameCell cell;
        public EntityType type;

        public void Destroy()
        {
            Destroy(gameObject);
        }
        
        public enum EntityType
        {
            None,
            Player,
            Block,
            Tile
        }
    }
}