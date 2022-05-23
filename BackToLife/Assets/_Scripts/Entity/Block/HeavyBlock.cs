using UnityEngine;

namespace BackToLife
{
    public class HeavyBlock : Block
    {

        public override Vector2 Move(Vector2 dir)
        {
            ParticleManager.PlayParticle(transform,ParticleManager.Particle.ParticleType.Default,dir);
            return base.Move(dir);
        }
    }
}