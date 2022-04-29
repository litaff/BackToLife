﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using UnityEngine;

namespace BackToLife
{
    [Serializable]
    public class GameGrid
    {
        public int nrOfRows;
        public int nrOfColumns;
        public float horizontalMargin;
        public float verticalMargin;
        public Cell[,] cells;
        private Vector2 _cellSize;
        public Vector2 _gridSize;
        private Camera _camera;
        private Vector2 _position;

        public GameGrid(int nrOfRows, int nrOfColumns, Vector2 position, float horizontalMargin = 0, float verticalMargin = 0)
        {
            this.nrOfRows = nrOfRows;
            this.nrOfColumns = nrOfColumns;
            this.horizontalMargin = horizontalMargin;
            this.verticalMargin = verticalMargin;
            _position = position;
            Init();
        }

        public void Deconstruct()
        {
            foreach (var cell in cells)
            {
                if(cell.currentEntity == null) continue;
                
                cell.currentEntity.Destroy();
            }
        }
        
        private void Init()
        {
            cells = new Cell[nrOfColumns, nrOfRows];
            _camera = Camera.main;
            _gridSize = 2 * _camera.ScreenToWorldPoint(
                new Vector2(Screen.width - horizontalMargin, Screen.height - verticalMargin));
            _camera.aspect = 0.5625f;
            _cellSize = new Vector2(_gridSize.x / nrOfColumns, _gridSize.y / nrOfRows);
            _cellSize = _cellSize.x > _cellSize.y ? new Vector2(_cellSize.y, _cellSize.y) : new Vector2(_cellSize.x, _cellSize.x);
            _gridSize = new Vector2(_cellSize.x * nrOfColumns, _cellSize.y * nrOfRows);
                for (int row = 0; row < nrOfRows; row++)
            {
                for (int column = 0; column < nrOfColumns; column++)
                {
                    var position = _position - _gridSize/2 +
                                   new Vector2(_cellSize.x * column + _cellSize.x/2, _cellSize.y * row + _cellSize.y/2);
                    cells[column, row] = new Cell(position, _cellSize);
                }
            }
            

        }

        public void UpdateCellsInWorld()
        {
            foreach (var cell in cells)
            {
                if(cell.currentEntity == null)
                    continue;
                var transform = cell.currentEntity.transform;
                transform.position = cell.worldPosition;
            }
        }

        public void UpdateCellsInGrid()
        {
            var entities = new List<Entity>();
            foreach (var cell in cells)
            {
                if (cell.currentEntity == null) continue;
                entities.Add(cell.currentEntity);
                cell.currentEntity = null;
            }

            foreach (var entity in entities)
            {
                cells[(int) entity.gridPosition.x, (int) entity.gridPosition.y].currentEntity = entity;
                entity.cell = cells[(int) entity.gridPosition.x, (int) entity.gridPosition.y];
            }
            
        }

        public Cell GetCellFromGridPosition(Vector2 pos)
        {
            return cells[(int)pos.x, (int)pos.y];
        }

        public Vector2 GetGridPositionFromCell(Cell cell)
        {
            for (int column = 0; column < nrOfColumns; column++)
            {
                for (int row = 0; row < nrOfRows; row++)
                {
                    if (cells[column, row] == cell)
                        return new Vector2(column, row);
                }
            }

            return new Vector2(0, 0);
        }
        
        public bool CellEmpty(Cell cell)
        {
            return cell.currentEntity == null;
        }

        public bool MoveInGrid(Vector2 pos)
        {
            return pos.x < 0 || pos.y < 0 || pos.x > nrOfColumns-1 || pos.y > nrOfRows-1;
        }

        public bool CheckForCrampedCell(Entity first, Entity second)
        {
            return first.gridPosition == second.gridPosition;
        }
        
        [Serializable]
        public class Cell
        {
            public Vector2 worldPosition;
            public Vector2 size;
            public Entity currentEntity;

            public Cell(Vector2 pos, Vector2 s)
            {
                worldPosition = pos;
                size = s;
            }
        }
    }
}