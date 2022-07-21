using UnityEngine;

namespace BackToLife
{
    public abstract class Block : Entity
    {
        public bool moved;
        public int blockWeight;
        public BlockType blockType;

        public virtual Vector2 Move(Vector2 dir)
        {
            return dir;
        }
        
        public enum BlockType
        {
            None,
            Regular,
            Slime,
            Slippery
        }
    }
}