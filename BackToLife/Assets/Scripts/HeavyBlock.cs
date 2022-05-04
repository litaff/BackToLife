using UnityEngine;

namespace BackToLife
{
    public class HeavyBlock : Entity
    {
        public override Vector2 OnInteract(Vector2 dir)
        {
            return dir;
        }
    }
}