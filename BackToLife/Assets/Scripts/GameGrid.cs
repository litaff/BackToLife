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
        public Cell[,] cells;
        private float _cellSize;
        private Vector2 _dimensions;
        private Vector2 _gridSize;
        private Vector2 _position;

        public GameGrid(float cellSize, Vector2 dim, Vector2 pos)
        {
            _dimensions = dim;
            _position = pos;
            _cellSize = cellSize;
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

        public Vector2 GetBackgroundSize(Vector2 offset)
        {
            return _gridSize + offset;
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
            for (var column = 0; column < (int)_dimensions.x; column++)
            {
                for (var row = 0; row < (int)_dimensions.y; row++)
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
            return pos.x < 0 || pos.y < 0 || pos.x > (int)_dimensions.x-1 || pos.y > (int)_dimensions.y-1;
        }

        public bool CheckForCrampedCell(Entity first, Entity second)
        {
            return first.gridPosition == second.gridPosition;
        }

        private void Init()
        {
            cells = new Cell[(int)_dimensions.x, (int)_dimensions.y];
            _gridSize = new Vector2(_cellSize * (int)_dimensions.x, _cellSize * (int)_dimensions.y);
            for (var row = 0; row < (int)_dimensions.y; row++)
            {
                for (var column = 0; column < (int)_dimensions.x; column++)
                {
                    var position = _position - _gridSize/2 +
                                   new Vector2(_cellSize * column + _cellSize/2, _cellSize * row + _cellSize/2);
                    cells[column, row] = new Cell(position, _cellSize);
                }
            }
            

        }

        [Serializable]
        public class Cell
        {
            public float size;
            public Vector2 worldPosition;
            public Entity currentEntity;

            public Cell(Vector2 pos, float s)
            {
                size = s;
                worldPosition = pos;
            }
        }
    }
}