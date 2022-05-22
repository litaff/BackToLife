using System;
using System.Collections.Generic;
using UnityEngine;

namespace BackToLife
{
    [Serializable]
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private float cellSize;
        private Vector2 _dimensions;
        private GameGrid _grid;
        private Player _player;
        private EndTile _endTile;
        private PrefabManager _prefabManager;
        private TeleportTile _firstTeleportTile;

        public void GameUpdate(Vector2 swipeDir, float updateSpeed)
        {
            _grid.UpdateCellsInWorld(updateSpeed);
            if (swipeDir != Vector2.zero)
                _grid.MoveInDirection(_player, swipeDir, _player.moveStrength);
            _grid.UpdateCellsInGrid();
        }

        public void InitializeGrid(GridPattern pattern)
        {
            if (pattern is null)
            {
                enabled = false;
                return;
            }
            
            _dimensions.x = pattern.nrOfColumns;
            _dimensions.y = pattern.nrOfRows;
            _grid = new GameGrid(cellSize ,_dimensions, transform.position);
            gameObject.transform.parent.GetComponentInChildren<SpriteRenderer>().size = _grid.GetBackgroundSize(Vector2.one * 6 / 16);
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
                        throw new ArgumentOutOfRangeException();
                }
            }
            _grid.InstantUpdateCellsInWorld();

            void LinkTeleportTiles(TeleportTile firstTile, TeleportTile secondTile)
            {
                firstTile.linked = secondTile;
                secondTile.linked = firstTile;
            }
        }
        
        
        
        private void Awake()
        {
            _prefabManager = gameObject.transform.parent.GetComponentInChildren<PrefabManager>();
        }
        
        private void OnDisable()
        {
            _grid?.Deconstruct();
            if(_player)
                _player.Destroy();
            if(_endTile)
                _endTile.Destroy();
            _endTile = null;
            _player = null;
        }
        
        public bool WinCondition()
        {
            return _grid.GetCellFromGridPosition(_player.gridPosition).tile == _endTile;
        }
    }
}