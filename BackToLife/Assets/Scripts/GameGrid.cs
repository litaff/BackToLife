using System;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

namespace BackToLife
{
    
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

        public void UpdateCells()
        {
            foreach (var cell in Cells)
            {
                var transform = cell.CurrentEntity.transform;
                transform.position = cell.position;
                transform.localScale = cell.size*.9f;
            }
        }
        
        [Serializable]
        public class Cell
        {
            public Vector2 position;
            public Vector2 size;
            public Entity CurrentEntity;

            public Cell(Vector2 pos, Vector2 s)
            {
                position = pos;
                size = s;
            }
        }
    }
}