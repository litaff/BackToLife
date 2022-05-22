using UnityEngine;

namespace BackToLife
{
    public class TeleportTile : Tile
    {
        public TeleportTile linked;
        public bool used;

        public override bool OnInteract()
        {

            if (!(cell.currentEntity is null))
            {
                if (!used && cell.currentEntity.GetType() == typeof(Player))
                {
                    cell.currentEntity.gridPosition = linked.gridPosition;
                    used = true;
                    linked.used = true;
                    return true;
                }
            }
            if (cell.currentEntity is null && linked.cell.currentEntity is null)
            {
                used = false;
            }

            return false;
        }
    }
}