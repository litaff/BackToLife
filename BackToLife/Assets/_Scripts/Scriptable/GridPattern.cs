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
        public List<Cell> cells;
        public bool Valid { get; private set; }

        private void OnValidate()
        {
            Valid = CheckForValidType() && CheckPlayer() && CheckForEndTile();
        }

        

        /// <returns>True if exactly one player</returns>
        private bool CheckPlayer()
        {
            var players = (from cell in cells 
                where cell.entityType == Entity.EntityType.Player 
                select cell).ToList();
            if (players.Count < 1)
                Debug.LogWarning($"{name} has less than one player");
            else if (players.Count > 1)
                Debug.LogWarning($"{name} has more than one player");
            return players.Count == 1;
        }
        /// <returns>True if exactly one end tile</returns>
        private bool CheckForEndTile()
        {
            var tiles = (from cell in cells
                where cell.entityType == Entity.EntityType.Tile
                select cell).ToList();
            if (tiles.Count(tile => tile.tileType == Tile.TileType.EndTile) < 1)
                Debug.LogWarning($"{name} has less than one end tile");
            else if (tiles.Count(tile => tile.tileType == Tile.TileType.EndTile) > 1)
                Debug.LogWarning($"{name} has more than one end tile");
            return tiles.Count == 1;
        }
        /// <returns>True if types are correct</returns>
        private bool CheckForValidType()
        {
            /*var blockNotTile= (from cell in cells
                where cell.entityType == Entity.EntityType.Block
                select cell).ToList();
            var tileNotBlock= (from cell in cells
                where cell.entityType == Entity.EntityType.Tile
                select cell).ToList();
            if (blockNotTile.Any(cell => cell.tileType != Tile.TileType.None))
            {
                Debug.LogWarning($"{name} has a mixed up type at {cells.FindIndex(c => true)}");
                return false;
            }
            // ReSharper disable once InvertIf
            if (tileNotBlock.Any(cell => cell.blockType != Block.BlockType.None))
            {
                Debug.LogWarning($"{name} has a mixed up type at {cells.FindIndex(c => true)}");
                return false;
            }*/

            foreach (var cell in cells)
            {
                if (cell.entityType == Entity.EntityType.Player)
                {
                    cell.blockType = Block.BlockType.None;
                    cell.tileType = Tile.TileType.None;
                }

                if (cell.entityType == Entity.EntityType.Block)
                    cell.tileType = Tile.TileType.None;
                if (cell.entityType == Entity.EntityType.Tile)
                    cell.blockType = Block.BlockType.None;



            }
            
            return true;
        }

        [Serializable]
        public class Cell
        {
            public Vector2 gridPosition;
            public Entity.EntityType entityType;
            public Block.BlockType blockType;
            public Tile.TileType tileType;
        }
    }
}