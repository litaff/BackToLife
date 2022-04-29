using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BackToLife
{
    public class GameManager : MonoBehaviour
    {
        public int nrOfRows;
        public int nrOfColumns;
        public int moveStrength;
        public float horizontalMargin;
        public float verticalMargin;
        public Block blockPrefab;
        public Block heavyBlockPrefab;
        public Block endTilePrefab;
        public Player playerPrefab;
        public bool winState;
        private GameGrid _grid;
        private TouchController _touchController;
        private Entity _player;
        private Entity _endTile;

        private void Awake()
        {
            _touchController = new TouchController();
        }

        private void Update()
        {
            _grid.UpdateCellsInWorld();
            var swipeDir = _touchController.GetSwipeDirection();
            if (swipeDir != Vector2.zero)
                MoveInDirection(_player, swipeDir, moveStrength);
            winState = _grid.CheckForCrampedCell(_player, _endTile);
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

        public void InitializeGrid(GridPattern pattern)
        {
            _grid = new GameGrid(nrOfRows,nrOfColumns, transform.position, horizontalMargin,verticalMargin);
            GetComponentInChildren<SpriteRenderer>().transform.localScale = _grid._gridSize;
            Entity prefab = blockPrefab;
            foreach (var cell in pattern.cells)
            {
                switch (cell.entityType)
                {
                    case EntityType.Player:
                        prefab = playerPrefab;
                        break;
                    case EntityType.Regular:
                        prefab = blockPrefab;
                        break;
                    case EntityType.Slippery:
                        break;
                    case EntityType.UnMovable:
                        break;
                    case EntityType.Heavy:
                        prefab = heavyBlockPrefab;
                        break;
                    case EntityType.EndTile:
                        prefab = endTilePrefab;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                var entity = Instantiate(prefab, transform, true);
                entity.cell = _grid.cells[(int) cell.gridPosition.x, (int) cell.gridPosition.y];
                entity.gridPosition = cell.gridPosition;
                entity.type = prefab.type;
                _grid.cells[(int) cell.gridPosition.x, (int) cell.gridPosition.y].currentEntity = entity;
                if (prefab.type != EntityType.Player && prefab.type != EntityType.EndTile)
                    continue;
                if(prefab.type == EntityType.EndTile)
                    _endTile = entity;
                else
                    _player = entity;


            }
        }

        public bool GetPatternData(GridPattern pattern)
        {
            nrOfColumns = pattern.nrOfColumns;
            nrOfRows = pattern.nrOfRows;
            return pattern.Valid;
        }

        private bool MoveInDirection(Entity entity, Vector2 dir, int moveStr)
        {
            if (WinCondition(entity, dir))
            {
                entity.gridPosition += dir;
                return true;
            }
            if(moveStr < entity.weight)
                return false;
            if (_grid.MoveInGrid(entity.gridPosition + dir))
            {
                Debug.LogWarning($"{entity} + tried moving of grid!");
                return false;
            }

            if (!_grid.CellEmpty(_grid.GetCellFromGridPosition(entity.gridPosition + dir)))
            {
                if (!MoveInDirection(_grid.GetCellFromGridPosition(entity.gridPosition + dir).currentEntity, dir,
                        moveStr - entity.weight))
                {
                    Debug.LogWarning($"{entity} + tried moving to full cell!");
                    return false;
                }
            }
            entity.gridPosition += dir;
            Debug.Log(entity.gridPosition);
            return true;
        }

        private bool WinCondition(Entity entity, Vector2 dir)
        {
            return entity == _player && _grid.GetCellFromGridPosition(entity.gridPosition + dir).currentEntity == _endTile;
        }
    }
}
