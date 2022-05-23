using UnityEngine;

namespace BackToLife
{
    public abstract class Block : Entity
    {
        public BlockType blockType;
        
        public int blockWeight;
        public virtual Vector2 Move(Vector2 dir)
        {
            
            return dir;
        }
        
        public enum BlockType
        {
            None,
            Regular,
            Heavy,
            Slippery,
            UnMovable
        }
    }
}