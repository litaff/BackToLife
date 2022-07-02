using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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
        // TODO: move this \/ stupid mess to GridPatternEditorWindow.cs you dumb banana
        [HideInInspector] public Texture player;
        [HideInInspector] public Texture regularBlock;
        [HideInInspector] public Texture slipperyBlock;
        [HideInInspector] public Texture slimeBlock;
        [HideInInspector] public Texture teleportTile;
        [HideInInspector] public Texture endTile;

        public bool Valid => CheckForValidType() && CheckPlayer() && CheckForEndTile() && CheckForTeleportTile();

        private void OnValidate()
        {
            CheckForValidType();
            CheckPlayer();
            CheckForEndTile();
            CheckForTeleportTile();
            RemoveCellOutOfBounds();
            player = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Sprites/Characters/soul_v2.png", typeof(Texture2D));
            regularBlock = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Sprites/Blocks/regular_block.png", typeof(Texture2D));
            slipperyBlock = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Sprites/Blocks/slippery_block.png", typeof(Texture2D));
            slimeBlock = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Sprites/Blocks/slime_block.png", typeof(Texture2D));
            teleportTile = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Sprites/Tiles/teleport_tile.png", typeof(Texture2D));
            endTile = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Sprites/Tiles/end_tile.png", typeof(Texture2D));
        }

        private void RemoveCellOutOfBounds()
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
                where cell.tileType == Tile.TileType.EndTile
                select cell).ToList();
            if (tiles.Count < 1)
                Debug.LogWarning($"{name} has less than one end tile");
            else if (tiles.Count > 1)
                Debug.LogWarning($"{name} has more than one end tile");
            return tiles.Count == 1;
        }
        
        /// <returns>True if each teleport tile has a link</returns>
        private bool CheckForTeleportTile()
        {
            var tiles = (from cell in cells
                where cell.tileType == Tile.TileType.TeleportTile
                select cell).ToList();
            if(tiles.Count%2 != 0)
                Debug.LogWarning($"{name} has an orphan teleport tile");

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
        }
    }
}