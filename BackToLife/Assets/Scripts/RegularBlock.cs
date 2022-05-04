using UnityEngine;

namespace BackToLife
{
    public class RegularBlock : Entity
    {
        public override Vector2 OnInteract(Vector2 dir)
        {
            return dir;
        }
    }
}