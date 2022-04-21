using System;
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
        public Cell[,] Cells;
        private Vector2 _cellSize;
        private Vector2 _gridSize;
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
        
        private void Init()
        {
            Cells = new Cell[nrOfColumns, nrOfRows];
            _camera = Camera.main;
            _gridSize = 2 * _camera.ScreenToWorldPoint(
                new Vector2(Screen.width - horizontalMargin, Screen.height - verticalMargin));
            _cellSize = new Vector2(_gridSize.x / nrOfColumns, _gridSize.y / nrOfRows);
            for (int row = 0; row < nrOfRows; row++)
            {
                for (int column = 0; column < nrOfColumns; column++)
                {
                    var position = _position - _gridSize/2 +
                                   new Vector2(_cellSize.x * column + _cellSize.x/2, _cellSize.y * row + _cellSize.y/2);
                    Cells[column, row] = new Cell(position, _cellSize);
                }
            }
            

        }

        public void UpdateCellsInWorld()
        {
            foreach (var cell in Cells)
            {
                if(cell.currentEntity == null)
                    continue;
                var transform = cell.currentEntity.transform;
                transform.position = cell.worldPosition;
                transform.localScale = cell.size*.9f; // setting scale !!!temp!!!
            }
        }

        public void UpdateCellsInGrid()
        {
            var entities = new List<Entity>();
            foreach (var cell in Cells)
            {
                if (cell.currentEntity == null) continue;
                entities.Add(cell.currentEntity);
                cell.currentEntity = null;
            }

            foreach (var entity in entities)
            {
                Cells[(int) entity.gridPosition.x, (int) entity.gridPosition.y].currentEntity = entity;
                entity.cell = Cells[(int) entity.gridPosition.x, (int) entity.gridPosition.y];
            }
            
        }

        public Cell GetCellFromGridPosition(Vector2 pos)
        {
            return Cells[(int)pos.x, (int)pos.y];
        }

        public Vector2 GetGridPositionFromCell(Cell cell)
        {
            for (int column = 0; column < nrOfColumns; column++)
            {
                for (int row = 0; row < nrOfRows; row++)
                {
                    if (Cells[column, row] == cell)
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