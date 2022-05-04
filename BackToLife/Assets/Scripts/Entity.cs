using System;
using UnityEngine;

namespace BackToLife
{
    [Serializable]
    public abstract class Entity : MonoBehaviour
    {
        public Vector2 gridPosition;
        public GameGrid.Cell cell;
        public EntityType type;
        public int weight;

        public virtual Vector2 OnInteract(Vector2 dir)
        {
            return dir;
        }
        
        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}