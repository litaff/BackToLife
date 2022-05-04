using UnityEngine;

namespace BackToLife
{
    public class SlipperyBlock : Entity
    {
        public override Vector2 OnInteract(Vector2 dir)
        {
            return dir*5;
        }
    }
}