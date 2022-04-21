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
        private Player _player;
        [SerializeField]private GameGrid _grid;
        private TouchController _touchController;
        private void Awake()
        {
            _touchController = new TouchController();
            InitializeGrid();
            _grid.Cells[1, 1].currentEntity = Instantiate(blockPrefab, transform, true);
            _grid.Cells[1, 1].currentEntity.cell = _grid.Cells[1, 1];
            _grid.Cells[1, 1].currentEntity.gridPosition = Vector2.one;
            _player = Instantiate(playerPrefab, transform, true);
            _player.cell = _grid.Cells[0, 0];
            _grid.Cells[0, 0].currentEntity = _player;
            
            
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

        private void InitializeGrid()
        {
            _grid = new GameGrid(nrOfRows,nrOfColumns, transform.position, horizontalMargin,verticalMargin);
        }
        
    }
}
