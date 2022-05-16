using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace BackToLife
{
    public class GameManager : MonoBehaviour
    {
        public bool winState;
        [SerializeField] private int moveStrength;
        [SerializeField] private float cellSize;
        private Vector2 _dimensions;
        private GameGrid _grid;
        private Player _player;
        private EndTile _endTile;
        private TouchController _touchController;
        private PrefabManager _prefabManager;

        public void InitializeGrid(GridPattern pattern)
        {
            _grid = new GameGrid(cellSize ,_dimensions, transform.position);
            GetComponentInChildren<SpriteRenderer>().size = _grid.GetBackgroundSize(Vector2.one * 6 / 16);
            foreach (var cell in pattern.cells)
            {
                var prefab = _prefabManager.GetPrefab(cell.entityType, cell.blockType, cell.tileType);

                var entity = Instantiate(prefab, transform, true);
                entity.cell = _grid.cells[(int) cell.gridPosition.x, (int) cell.gridPosition.y];
                entity.gridPosition = cell.gridPosition;
                entity.type = prefab.type;
                
                switch (entity.type)
                {
                    case Entity.EntityType.Player:
                        _player = (Player)entity;
                        _grid.cells[(int) cell.gridPosition.x, (int) cell.gridPosition.y].currentEntity = _player;
                        break;
                    case Entity.EntityType.Block:
                        var block = (Block) entity;
                        block.blockType = cell.blockType;
                        _grid.cells[(int) cell.gridPosition.x, (int) cell.gridPosition.y].currentEntity = block;
                        break;
                    case Entity.EntityType.Tile:
                        var tile = (Tile)entity;
                        tile.tileType = cell.tileType;
                        _grid.cells[(int) cell.gridPosition.x, (int) cell.gridPosition.y].tile = tile;
                        
                        if(tile.tileType == Tile.TileType.EndTile)
                            _endTile = (EndTile)entity;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            
        }

        public bool GetPatternData(GridPattern pattern)
        {
            _dimensions.x = pattern.nrOfColumns;
            _dimensions.y = pattern.nrOfRows;
            return pattern.Valid;
        }

        private void Awake()
        {
            _touchController = new TouchController();
            _prefabManager = GetComponentInChildren<PrefabManager>();
        }

        private void Update()
        {
            _grid.UpdateCellsInWorld();
            var swipeDir = _touchController.GetSwipeDirection();
            if (swipeDir != Vector2.zero)
                _grid.MoveInDirection(_player, swipeDir, moveStrength);
            _grid.UpdateCellsInGrid();
        }

        private void OnDisable()
        {
            _grid.Deconstruct();
            winState = false;
            _player.Destroy();
            _player = null;
            _endTile.Destroy();
            _endTile = null;
        }

        private bool WinCondition(Entity entity, Vector2 dir)
        {
            return entity == _player && _grid.GetCellFromGridPosition(entity.gridPosition + dir).currentEntity == _endTile;
        }

    }
}
