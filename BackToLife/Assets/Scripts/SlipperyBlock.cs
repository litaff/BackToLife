using UnityEngine;

namespace BackToLife
{
    public class SlipperyBlock : Block
    {
        public int slipperiness;
        public override Vector2 Move(Vector2 dir)
        {
            return slipperiness*dir.normalized;
        }
    }
}