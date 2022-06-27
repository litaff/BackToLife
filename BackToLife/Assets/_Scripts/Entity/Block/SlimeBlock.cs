using UnityEngine;

namespace BackToLife
{
    public class SlimeBlock : Block
    {

        public override Vector2 Move(Vector2 dir)
        {
            ParticleManager.PlayParticle(transform,ParticleManager.Particle.ParticleType.Slime,dir);
            return base.Move(dir);
        }
    }
}