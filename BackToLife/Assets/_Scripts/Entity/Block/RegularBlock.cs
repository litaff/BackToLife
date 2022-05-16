using UnityEngine;

namespace BackToLife
{
    public class RegularBlock : Block
    {

        public override Vector2 Move(Vector2 dir)
        {
            return dir;
        }
    }
}