using UnityEngine;

namespace BackToLife
{
    public class TeleportTile : Tile
    {
        public TeleportTile linked;
        public bool used;

        public override void OnInteract()
        {

            if (!(cell.currentEntity is null))
            {
                if (!used && cell.currentEntity.GetType() == typeof(Player))
                {
                    ParticleManager.PlayParticle(transform,ParticleManager.Particle.ParticleType.Magic, new Vector2(0,-1));
                    ParticleManager.PlayParticle(linked.transform,ParticleManager.Particle.ParticleType.Magic, new Vector2(0,-1));
                    cell.currentEntity.gridPosition = linked.gridPosition;
                    used = true;
                    linked.used = true;
                }
            }
            if (cell.currentEntity is null && linked.cell.currentEntity is null)
            {
                used = false;
            }
        }
    }
}