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
        public Player playerPrefab;
        [SerializeField]private GridPattern _pattern;
        [SerializeField] private GameGrid _grid;
        private TouchController _touchController;
        private Entity _player;
        
        private void Awake()
        {
            _touchController = new TouchController();
            if (!GetPatternData())
                Debug.LogError($"{_pattern.name} is not valid!");
            InitializeGrid();
            /*_grid.cells[1, 1].currentEntity = Instantiate(blockPrefab, transform, true);
            _grid.cells[1, 1].currentEntity.cell = _grid.cells[1, 1];
            _grid.cells[1, 1].currentEntity.gridPosition = Vector2.one;
            _player = Instantiate(playerPrefab, transform, true);
            _player.cell = _grid.cells[0, 0];
            _grid.cells[0, 0].currentEntity = _player;*/
            
            
        }

        private void Update()
        {
            _grid.UpdateCellsInWorld();
            var swipeDir = _touchController.GetSwipeDirection();
            if(swipeDir != Vector2.zero)
                MoveInDirection(_player, swipeDir,moveStrength);
            _grid.UpdateCellsInGrid();
        }

        private bool MoveInDirection(Entity entity, Vector2 dir, int moveStr)
        {
            if(moveStr <= 0)
                return false;
            if (_grid.MoveInGrid(entity.gridPosition + dir))
            {
                Debug.LogWarning($"{entity} + tried moving of grid!");
                return false;
            }

            if (!_grid.CellEmpty(_grid.GetCellFromGridPosition(entity.gridPosition + dir)))
            {
                if (!MoveInDirection(_grid.GetCellFromGridPosition(entity.gridPosition + dir).currentEntity, dir,
                        moveStr - 1))
                {
                    Debug.LogWarning($"{entity} + tried moving to full cell!");
                    return false;
                }
            }
            entity.gridPosition += dir;
            Debug.Log(entity.gridPosition);
            return true;
        }

        private bool GetPatternData()
        {
            nrOfColumns = _pattern.nrOfColumns;
            nrOfRows = _pattern.nrOfRows;
            return _pattern.Valid;
        }
        private void InitializeGrid()
        {
            _grid = new GameGrid(nrOfRows,nrOfColumns, transform.position, horizontalMargin,verticalMargin);
            GetComponentInChildren<SpriteRenderer>().transform.localScale = _grid._gridSize;
            Entity prefab = blockPrefab;
            var type = EntityType.Regular;
            foreach (var cell in _pattern.cells)
            {
                switch (cell.entityType)
                {
                    case EntityType.Player:
                        prefab = playerPrefab;
                        type = EntityType.Player;
                        break;
                    case EntityType.Regular:
                        prefab = blockPrefab;
                        type = EntityType.Regular;
                        break;
                    case EntityType.Slippery:
                        break;
                    case EntityType.UnMovable:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                var entity = Instantiate(prefab, transform, true);
                entity.cell = _grid.cells[(int) cell.gridPosition.x, (int) cell.gridPosition.y];
                entity.gridPosition = cell.gridPosition;
                entity.type = type;
                _grid.cells[(int) cell.gridPosition.x, (int) cell.gridPosition.y].currentEntity = entity;
                if(type != EntityType.Player) continue;
                _player = entity;
            }
        }
        
    }
}
