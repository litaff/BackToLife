using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BackToLife
{
    public class CellModHandler : MonoBehaviour
    {
        public GameObject entityType;
        public GameObject blockType;
        public GameObject tileType;

        private Vector2 gridPosition;
        private TMP_Dropdown _entityType;
        private TMP_Dropdown _blockType;
        private TMP_Dropdown _tileType;
        
        // this needs grid position
        public GridPattern.PatternCell GetPatternCell()
        {
            return new GridPattern.PatternCell
            {
                gridPosition = gridPosition,
                entityType = (Entity.EntityType) _entityType.value,
                blockType = (Block.BlockType) _blockType.value,
                tileType = (Tile.TileType) _tileType.value
            };
        }
        // this needs grid position
        public Vector2 GetGridPosition()
        {
            return gridPosition;
        }

        public void SetPatternCell(GridPattern.PatternCell patternCell, Vector2 gridPos)
        {
            if (patternCell is null)
            {
                gridPosition = gridPos;
                _entityType.value = 0;
            }
            else
            {
                gridPosition = patternCell.gridPosition;
                _entityType.value = (int)patternCell.entityType;
                _blockType.value = (int)patternCell.blockType;
                _tileType.value = (int)patternCell.tileType;
            }
        }
        
        private void Awake()
        {
            _entityType = entityType.GetComponent<TMP_Dropdown>();
            _blockType = blockType.GetComponent<TMP_Dropdown>();
            _tileType = tileType.GetComponent<TMP_Dropdown>();

            _entityType.options = new List<TMP_Dropdown.OptionData>
            {
                new TMP_Dropdown.OptionData(Entity.EntityType.None.ToString()),
                new TMP_Dropdown.OptionData(Entity.EntityType.Player.ToString()),
                new TMP_Dropdown.OptionData(Entity.EntityType.Block.ToString()),
                new TMP_Dropdown.OptionData(Entity.EntityType.Tile.ToString())
            };

            _blockType.options = new List<TMP_Dropdown.OptionData>
            {
                new TMP_Dropdown.OptionData(Block.BlockType.None.ToString()),
                new TMP_Dropdown.OptionData(Block.BlockType.Regular.ToString()),
                new TMP_Dropdown.OptionData(Block.BlockType.Slime.ToString()),
                new TMP_Dropdown.OptionData(Block.BlockType.Slippery.ToString())
            };

            _tileType.options = new List<TMP_Dropdown.OptionData>
            {
                new TMP_Dropdown.OptionData(Tile.TileType.None.ToString()),
                new TMP_Dropdown.OptionData(Tile.TileType.EndTile.ToString()),
                new TMP_Dropdown.OptionData(Tile.TileType.TeleportTile.ToString())
            };
        }

        private void Update()
        {
            switch (_entityType.value)
            {
                case 0:
                case 1:
                    blockType.SetActive(false);
                    tileType.SetActive(false);
                    _blockType.value = 0;
                    _tileType.value = 0;
                    break;
                case 2:
                    blockType.SetActive(true);
                    tileType.SetActive(false);
                    _tileType.value = 0;
                    break;
                case 3:
                    blockType.SetActive(false);
                    tileType.SetActive(true);
                    _blockType.value = 0;
                    break;
            }
        }
    }
}