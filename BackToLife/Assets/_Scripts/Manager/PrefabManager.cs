using System;
using UnityEngine;

namespace BackToLife
{
    public class PrefabManager : MonoBehaviour
    {
        [SerializeField] private RegularBlock regularBlockPrefab;
        [SerializeField] private SlimeBlock slimeBlockPrefab;
        [SerializeField] private SlipperyBlock slipperyBlockPrefab;
        [SerializeField] private EndTile endTilePrefab;
        [SerializeField] private TeleportTile teleportTilePrefab;
        [SerializeField] private Player playerPrefab;
        
        public Entity GetPrefab(Entity.EntityType entityType, Block.BlockType blockType, Tile.TileType tileType)
        {
            switch (entityType)
            {
                case Entity.EntityType.Player:
                    return playerPrefab;

                case Entity.EntityType.Block:
                    switch (blockType)
                    {
                        case Block.BlockType.None:
                            break;
                        case Block.BlockType.Regular:
                            return regularBlockPrefab;

                        case Block.BlockType.Slime:
                            return slimeBlockPrefab;

                        case Block.BlockType.Slippery:
                            return slipperyBlockPrefab;

                        case Block.BlockType.UnMovable:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(blockType), blockType, null);
                    }
                    break;
                case Entity.EntityType.Tile:
                    switch (tileType)
                    {
                        case Tile.TileType.None:
                            break;
                        case Tile.TileType.EndTile:
                            return endTilePrefab;
 
                        case Tile.TileType.TeleportTile:
                            return teleportTilePrefab;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(tileType), tileType, null);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(entityType), entityType, null);
            }

            return null;
        }
    }
}