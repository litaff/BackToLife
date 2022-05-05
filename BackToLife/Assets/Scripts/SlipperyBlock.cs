using UnityEngine;

namespace BackToLife
{
    public class SlipperyBlock : Block
    {
        public int slipperiness;
        public override Vector2 Move(Vector2 dir)
        {
            return new Vector2(Mathf.Sign(dir.x)*slipperiness,Mathf.Sign(dir.y)*slipperiness)*dir.normalized;
        }
    }
}