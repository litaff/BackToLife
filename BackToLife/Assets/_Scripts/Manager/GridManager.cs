using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BackToLife
{
    [Serializable]
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private float cellSize;
        [SerializeField] private SpriteRenderer background;
        private Vector2 _dimensions;
        private GameGrid _grid;
        private Player _player;
        private EndTile _endTile;
        private TeleportTile _firstTeleportTile;
        private PrefabManager _prefabManager;
        private LineRenderer _lineRenderer;

        public void GameUpdate(Vector2 swipeDir, float updateSpeed)
        {
            _lineRenderer.positionCount = 0;
            _grid.UpdateCellsInWorld(updateSpeed);
            if (swipeDir != Vector2.zero)
                _grid.MoveInDirection(_player, swipeDir, _player.moveStrength);
            _grid.UpdateCellsInGrid();
        }

        public void EditorUpdate()
        {
            _grid.InstantUpdateCellsInWorld();
            DebugGrid();
            _grid.UpdateCellsInGrid();
        }

        public void InitializeGrid(GridPattern pattern)
        {
            if (pattern is null)
            {
                enabled = false;
                return;
            }
               
            // dispose of whatever is left
            _grid?.Dispose(); 
            _firstTeleportTile = null;
            
            _dimensions.x = pattern.nrOfColumns;
            _dimensions.y = pattern.nrOfRows;
            _grid = new GameGrid(cellSize ,_dimensions, transform.position);
            
            // background init
            background.size = _grid.GetBackgroundSize(Vector2.one * 6 / 16);
            background.enabled = true;

            foreach (var cell in pattern.cells)
            {
                var prefab = _prefabManager.GetPrefab(cell.entityType, cell.blockType, cell.tileType);

                var entity = Instantiate(prefab, transform, true);
                entity.cell = _grid.cells[(int) cell.gridPosition.x, (int) cell.gridPosition.y];
                entity.gridPosition = cell.gridPosition;
                entity.type = prefab.type;
                
                switch (entity.type)
                {
                    case Entity.EntityType.Player:
                        _player = (Player)entity;
                        _grid.cells[(int) cell.gridPosition.x, (int) cell.gridPosition.y].currentEntity = _player;
                        break;
                    case Entity.EntityType.Block:
                        var block = (Block) entity;
                        block.blockType = cell.blockType;
                        _grid.cells[(int) cell.gridPosition.x, (int) cell.gridPosition.y].currentEntity = block;
                        break;
                    case Entity.EntityType.Tile:
                        var tile = (Tile)entity;
                        tile.tileType = cell.tileType;
                        _grid.cells[(int) cell.gridPosition.x, (int) cell.gridPosition.y].tile = tile;
                        switch (tile.tileType)
                        {
                            case Tile.TileType.TeleportTile:
                                if(_firstTeleportTile is null)
                                    _firstTeleportTile = (TeleportTile)tile;
                                else
                                {
                                    LinkTeleportTiles(_firstTeleportTile, (TeleportTile)tile);
                                    _firstTeleportTile = null;
                                }
                                break;
                            case Tile.TileType.EndTile:
                                _endTile = (EndTile)entity;
                                break;
                        }

                        break;
                    default:
                        print($"{entity.type.ToString()}");
                        throw new ArgumentOutOfRangeException();
                }
            }
            _grid.InstantUpdateCellsInWorld();

            void LinkTeleportTiles(TeleportTile firstTile, TeleportTile secondTile)
            {
                firstTile.linked = secondTile;
                secondTile.linked = firstTile;
                print($"Linked {firstTile.gridPosition} and {secondTile.gridPosition}");
            }
        }

        private void DebugGrid()
        {
            var points = new List<Vector3>();
            var gridSize = new Vector2(cellSize * (int)_dimensions.x, cellSize * (int)_dimensions.y);
            var startPos = (Vector2) transform.position - gridSize / 2;
            for (var row = 0; row <= (int)_dimensions.y; row++)
            {
                if (row % 2 == 0)
                {
                    for (var column = 0; column <= (int)_dimensions.x; column++)
                    {
                        var position = startPos +
                                       new Vector2(cellSize * column, cellSize * row);
                        points.Add(position);
                    }
                }
                else
                {
                    for (var column = (int)_dimensions.x; column >= 0; column--)
                    {
                        var position = startPos +
                                       new Vector2(cellSize * column, cellSize * row);
                        points.Add(position);
                    }
                }
            }

            startPos = points.Last();
            for (var column = 0; column <= (int)_dimensions.x; column++)
            {
                if (column % 2 == 0)
                {
                    for (var row = 0; row <= (int)_dimensions.y; row++)
                    {
                        var position = startPos -
                                       new Vector2(cellSize * column, cellSize * row) * new Vector2(_dimensions.y%2 == 0 ? 1 : -1,1);
                        points.Add(position);
                    }
                }
                else
                {
                    for (var row = (int)_dimensions.y; row >= 0; row--)
                    {
                        var position = startPos -
                                       new Vector2(cellSize * column, cellSize * row) * new Vector2(_dimensions.y%2 == 0 ? 1 : -1,1);
                        points.Add(position);
                    }
                }
            }

            _lineRenderer.positionCount = points.Count;
            _lineRenderer.SetPositions(points.ToArray());
        }
        
        private void Awake()
        {
            _prefabManager = GetComponentInChildren<PrefabManager>();
            _lineRenderer = GetComponent<LineRenderer>();
        }

        private void OnDisable()
        {
            _grid?.Dispose();
            if(_player)
                _player.Destroy();
            if(_endTile)
                _endTile.Destroy();
            _endTile = null;
            _player = null;
            _firstTeleportTile = null;
            background.enabled = false;
            _lineRenderer.positionCount = 0;
        }
        
        public bool WinCondition()
        {
            return _grid.GetCellFromGridPosition(_player.gridPosition).tile == _endTile;
        }
    }
}