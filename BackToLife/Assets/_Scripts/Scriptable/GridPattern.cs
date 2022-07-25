using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BackToLife
{
    [CreateAssetMenu(fileName = "GridPattern", menuName = "ScriptableObjects/GridPattern", order = 1)]
    public class GridPattern : ScriptableObject
    {
        [Range(8,16)]
        public int nrOfRows;
        [Range(4,9)]
        public int nrOfColumns;
        public List<PatternCell> cells;

        public bool Valid => CheckForValidType() && CheckPlayer() && CheckForEndTile() && CheckForTeleportTile();

        private void OnValidate()
        {
            CheckForValidType();
            CheckPlayer();
            CheckForEndTile();
            CheckForTeleportTile();
            RemoveCellOutOfBounds();
        }

        public void RemoveCellOutOfBounds()
        {
            foreach (var cell in cells.ToList())
            {
                if (cell.gridPosition.x >= nrOfColumns)
                {
                    cells.Remove(cell);
                }
                else if (cell.gridPosition.y >= nrOfRows)
                {
                    cells.Remove(cell);
                }
            }
        }
        
        /// <returns>True if exactly one player</returns>
        public bool CheckPlayer()
        {
            var players = (from cell in cells 
                where cell.entityType == Entity.EntityType.Player 
                select cell).ToList();
            
            return players.Count == 1;
        }
        /// <returns>True if exactly one end tile</returns>
        public bool CheckForEndTile()
        {
            var tiles = (from cell in cells
                where cell.tileType == Tile.TileType.EndTile
                select cell).ToList();

            return tiles.Count == 1;
        }
        
        /// <returns>True if each teleport tile has a link</returns>
        public bool CheckForTeleportTile()
        {
            var tiles = (from cell in cells
                where cell.tileType == Tile.TileType.TeleportTile
                select cell).ToList();

            return tiles.Count%2 == 0;
        }
        
        /// <returns>True if types are correct</returns>
        private bool CheckForValidType()
        {
            foreach (var cell in cells)
            {
                switch (cell.entityType)
                {
                    case Entity.EntityType.Player:
                        cell.blockType = Block.BlockType.None;
                        cell.tileType = Tile.TileType.None;
                        break;
                    case Entity.EntityType.Block:
                        cell.tileType = Tile.TileType.None;
                        break;
                    case Entity.EntityType.Tile:
                        cell.blockType = Block.BlockType.None;
                        break;
                }
            }
            
            return true;
        }

        [Serializable]
        public class PatternCell
        {
            public Vector2 gridPosition;
            public Entity.EntityType entityType;
            public Block.BlockType blockType;
            public Tile.TileType tileType;

            public PatternCell()
            {
                
            }
            
            public PatternCell(PatternCell cell)
            {
                gridPosition = cell.gridPosition;
                entityType = cell.entityType;
                blockType = cell.blockType;
                tileType = cell.tileType;
            }

            public void ChangeTo(PatternCell cell)
            {
                gridPosition = cell.gridPosition;
                entityType = cell.entityType;
                blockType = cell.blockType;
                tileType = cell.tileType;
            }
        }
    }
}