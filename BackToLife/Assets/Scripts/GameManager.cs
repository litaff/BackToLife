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
        public float horizontalMargin;
        public float verticalMargin;
        public Block blockPrefab;
        public Player playerPrefab;
        private Player _player;
        private GameGrid _grid;
        private TouchController _touchController;
        private void Awake()
        {
            _touchController = new TouchController();
            InitializeGrid();
/*
            for (int row = 0; row < nrOfRows; row++)
            {
                for (int column = 0; column < nrOfColumns; column++)
                {
                    if(row*column%3 != 0)
                        continue;
                    var b = Instantiate(block, transform, true);
                    _grid.Cells[column,row].CurrentEntity = b;
                }
            }*/
            _player = Instantiate(playerPrefab, transform, true);
            _player.cell = _grid.Cells[0, 0];
            _grid.Cells[0, 0].currentEntity = _player;
            
            
        }

        private void Update()
        {
            _grid.UpdateCellsInWorld();
            var swipeDir = _touchController.GetSwipeDirection();
            if(swipeDir != Vector2.zero)
                MoveInDirection(_player, swipeDir);
            _grid.UpdateCellsInGrid();
        }

        private void MoveInDirection(Entity entity, Vector2 dir)
        {
            if (_grid.MoveInGrid(entity.gridPosition + dir))
            {
                Debug.LogWarning($"{entity} + tried moving of grid!");
                return;
            }
            if (!_grid.CellEmpty(_grid.GetCellFromGridPosition(entity.gridPosition + dir)))
                return;
            entity.gridPosition += dir;
            Debug.Log(entity.gridPosition);

        }

        private void InitializeGrid()
        {
            _grid = new GameGrid(nrOfRows,nrOfColumns, transform.position, horizontalMargin,verticalMargin);
        }
        
    }
}
