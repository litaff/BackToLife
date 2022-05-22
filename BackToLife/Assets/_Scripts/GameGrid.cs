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

        public void MoveInDirection(Player player, Vector2 dir, int str)
        {
            
            var newPos = player.gridPosition + dir;

            if (!MoveInGrid(newPos))
                return;
            if (CellEmpty(GetCellFromGridPosition(newPos)))
            {
                player.gridPosition = newPos;
            }
            else if (!(GetCellFromGridPosition(newPos).tile is null))
            {
                player.gridPosition = newPos;
                return;
            }
            else
            {
                var nextEnt = (Block)GetCellFromGridPosition(newPos).currentEntity;
                if (str < nextEnt.blockWeight)
                {
                    UpdateCellsInGrid();
                    return;
                }
                if (MoveBlock(nextEnt, dir, str - nextEnt.blockWeight))
                {
                    player.gridPosition = newPos;
                }
            }
            UpdateCellsInGrid();
        }
        
        /// <returns>True if moved by at least one tile</returns>
        private bool MoveBlock(Block block, Vector2 dir, int str)
        {
            dir = block.Move(dir);
            var absDir = new Vector2(Mathf.Abs(dir.x), Mathf.Abs(dir.y));
            var iterations = absDir.x > absDir.y ? (int)absDir.x : (int)absDir.y;
            var outcome = false;
            for (var i = 0; i < iterations; i++)
            {
                var newPos = block.gridPosition + dir.normalized;
                if (!MoveInGrid(newPos))
                    continue;
                if (CellEmpty(GetCellFromGridPosition(newPos)))
                {
                    outcome = true;
                    block.gridPosition = newPos;
                }
                else
                {
                    if (!(GetCellFromGridPosition(newPos).tile is null)) return outcome; 
                    var nextEnt = (Block)GetCellFromGridPosition(newPos).currentEntity;
                    if (str < nextEnt.blockWeight)
                    {
                        UpdateCellsInGrid();
                        return false;
                    }
                    if (!MoveBlock(nextEnt, dir, str - nextEnt.blockWeight)) continue;
                    outcome = true;
                    block.gridPosition = newPos;
                }
            }
            UpdateCellsInGrid();
            return outcome;
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

        public void UpdateCellsInWorld(float updateSpeed)
        {
            foreach (var cell in cells)
            {
                if(cell.GetTransform() is null)
                    continue;
                var trans = cell.GetTransform();

                if ((Vector2)trans.position != cell.worldPosition)
                {
                    trans.position = Vector2.Lerp(trans.position, cell.worldPosition, updateSpeed*Time.deltaTime);
                }
            }
        }

        public void UpdateCellsInGrid()
        {
            var entities = new List<Entity>();
            foreach (var cell in cells)
            {
                if (cell.currentEntity is null) continue;
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

        
        /// <returns>True if empty</returns>
        public bool CellEmpty(Cell cell)
        {
            return cell.currentEntity is null && cell.tile is null;
        }

        /// <returns>True if pos is in grid</returns>
        public bool MoveInGrid(Vector2 pos)
        {
            return pos.x > -1 && pos.y > -1 && pos.x < (int)_dimensions.x && pos.y < (int)_dimensions.y;
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
            public Tile tile;

            public Cell(Vector2 pos, float s)
            {
                size = s;
                worldPosition = pos;
            }

            public Transform GetTransform()
            {
                Transform transform;
                if (currentEntity is null)
                {
                    if (tile is null)
                        return null;
                    transform = tile.transform;
                    //transform.position = worldPosition;
                }
                else
                    transform = currentEntity.transform;

                return transform;
            }
        }
    }
}