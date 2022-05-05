using UnityEngine;

namespace BackToLife
{
    public abstract class Block : Entity
    {
        public virtual Vector2 Move(Vector2 dir)
        {
            return dir;
        }
    }
}