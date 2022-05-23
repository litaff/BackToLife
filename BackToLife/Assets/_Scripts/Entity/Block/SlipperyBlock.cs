using UnityEngine;

namespace BackToLife
{
    public class SlipperyBlock : Block
    {
        public int slipperiness;
        public override Vector2 Move(Vector2 dir)
        {
            ParticleManager.PlayParticle(transform,ParticleManager.Particle.ParticleType.Ice,dir);
            return base.Move(slipperiness*dir.normalized);
        }
    }
}