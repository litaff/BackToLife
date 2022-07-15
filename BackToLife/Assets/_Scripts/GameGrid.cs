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
        public GameCell[,] cells;
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
            }
            else
            {
                var nextEnt = (Block)GetCellFromGridPosition(newPos).currentEntity;
                if (str < nextEnt.blockWeight)
                {
                    //UpdateCellsInGrid();
                    return;
                }
                if (MoveBlock(nextEnt, dir, str - nextEnt.blockWeight))
                {
                    player.gridPosition = newPos;
                }
            }
            //UpdateCellsInGrid();
        }
        
        /// <returns>True if moved by at least one tile</returns>
        private bool MoveBlock(Block block, Vector2 dir, int str, bool chain = false)
        {
            dir = block.Move(dir);
            if (chain)
                dir = dir.normalized;
            var absDir = new Vector2(Mathf.Abs(dir.x), Mathf.Abs(dir.y));
            var iterations = absDir.x > absDir.y ? (int)absDir.x : (int)absDir.y;
            var outcome = false;
            
            // forward chain movement
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
                else if (!(GetCellFromGridPosition(newPos).tile is null))
                {
                    return outcome;
                }
                else
                {
                    var nextEnt = (Block)GetCellFromGridPosition(newPos).currentEntity;
                    if (str < nextEnt.blockWeight)
                    {
                        //UpdateCellsInGrid();
                        return false;
                    }
                    if(block is SlimeBlock) // to stop slippery block from sliding while near slime block
                        if (!MoveBlock(nextEnt, dir, str - nextEnt.blockWeight,true)) continue;
                    if(!(block is SlimeBlock))
                        if (!MoveBlock(nextEnt, dir, str - nextEnt.blockWeight)) continue;
                    outcome = true;
                    block.gridPosition = newPos;
                }
            }
            
            // side chain movement
            if(block is SlimeBlock && outcome)
            {
                var chainPosition = new List<Vector2>
                {
                    block.gridPosition + new Vector2(dir.normalized.y, dir.normalized.x) - dir.normalized,
                    block.gridPosition - new Vector2(dir.normalized.y, dir.normalized.x) - dir.normalized
                };
                var blocksToMove = chainPosition.Select(vector2 => (Block) GetCellFromGridPosition(vector2).currentEntity).ToList();
                foreach (var b in blocksToMove.Where(b => b))
                {
                    MoveBlock(b, dir, str, true);
                }
            }

            UpdateCellsInGrid();
            
            return outcome;
        }
        
        public void Deconstruct()
        {
            foreach (var cell in cells)
            {
                if(cell.currentEntity)
                    cell.currentEntity.Destroy();
                if(cell.tile)
                    cell.tile.Destroy();
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
                    var position = trans.position;
                    position = Vector2.Lerp(
                        position, 
                        updateSpeed < 100 ? NextPosition(position, cell.worldPosition) : cell.worldPosition, 
                        updateSpeed*Time.deltaTime);
                    trans.position = position;

                }
                else if(cell.tile is null)
                {
                    ParticleManager.StopParticle(trans);
                }
            }

            Vector2 NextPosition(Vector2 curr, Vector2 end)
            {
                var dist2D = new Vector2(Mathf.Abs(end.x - curr.x), Mathf.Abs(end.y - curr.y));
                var nextPos = end;
                var delta = _cellSize;
                if (dist2D.x > dist2D.y)
                {
                    if (!(Math.Abs(curr.x + Mathf.Sign(end.x - curr.x) * delta - end.x) > 0.00000001)) return nextPos;
                    delta = delta > dist2D.x ? dist2D.x : delta;
                    nextPos = new Vector2(Mathf.Sign(end.x - curr.x) * delta + curr.x, curr.y);
                }
                else if (dist2D.x < dist2D.y)
                {
                    if (!(Math.Abs(curr.y + Mathf.Sign(end.y - curr.y) * delta - end.y) > 0.00000001)) return nextPos;
                    delta = delta > dist2D.y ? dist2D.y : delta;
                    nextPos = new Vector2(curr.x, Mathf.Sign(end.y - curr.y) * delta + curr.y);
                }
                return nextPos;
            }
        }

        public void InstantUpdateCellsInWorld()
        {
            UpdateCellsInWorld(1000000);
        }

        public void UpdateCellsInGrid()
        {
            var entities = new List<Entity>();
            var tiles = (from GameCell cell in cells where !(cell.tile is null) select cell.tile).ToList();

            foreach (var tile in tiles)
            {
                tile.OnInteract();
            }
            
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
                if (!entity.cell.tile) continue;
                if(entity.cell.tile.GetType() == typeof(TeleportTile))
                    InstantUpdateCellsInWorld();
            }
            
        }

        public GameCell GetCellFromGridPosition(Vector2 pos)
        {
            return cells[(int)pos.x, (int)pos.y];
        }

        public Vector2 GetGridPositionFromCell(GameCell cell)
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
        public bool CellEmpty(GameCell cell)
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
            cells = new GameCell[(int)_dimensions.x, (int)_dimensions.y];
            _gridSize = new Vector2(_cellSize * (int)_dimensions.x, _cellSize * (int)_dimensions.y);
            for (var row = 0; row < (int)_dimensions.y; row++)
            {
                for (var column = 0; column < (int)_dimensions.x; column++)
                {
                    var position = _position - _gridSize/2 +
                                   new Vector2(_cellSize * column + _cellSize/2, _cellSize * row + _cellSize/2);
                    cells[column, row] = new GameCell(position, _cellSize);
                }
            }
        }

        [Serializable]
        public class GameCell
        {
            public float size;
            public Vector2 worldPosition;
            public Entity currentEntity;
            public Tile tile;

            public GameCell(Vector2 pos, float s)
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