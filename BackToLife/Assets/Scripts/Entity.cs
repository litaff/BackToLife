using UnityEngine;

namespace BackToLife
{
    public abstract class Entity : MonoBehaviour
    {
        public Vector2 gridPosition;
        public GameGrid.Cell cell;
    }
}