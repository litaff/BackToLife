using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace BackToLife
{
    public class GameManager : MonoBehaviour
    {
        public bool winState;
        [SerializeField] private int moveStrength;
        [SerializeField] private float cellSize;
        [SerializeField] private RegularBlock blockPrefab;
        [SerializeField] private HeavyBlock heavyBlockPrefab;
        [SerializeField] private SlipperyBlock slipperyBlockPrefab;
        [SerializeField] private EndTile endTilePrefab;
        [SerializeField] private Player playerPrefab;
        private Vector2 _dimensions;
        private GameGrid _grid;
        private Player _player;
        private EndTile _endTile;
        private TouchController _touchController;

        public void InitializeGrid(GridPattern pattern)
        {
            _grid = new GameGrid(cellSize ,_dimensions, transform.position);
            GetComponentInChildren<SpriteRenderer>().size = _grid.GetBackgroundSize(Vector2.one * 6 / 16);
            Entity prefab = blockPrefab;
            foreach (var cell in pattern.cells)
            {
                switch (cell.entityType)
                {
                    case EntityType.Player:
                        prefab = playerPrefab;
                        break;
                    case EntityType.Regular:
                        prefab = blockPrefab;
                        break;
                    case EntityType.Slippery:
                        prefab = slipperyBlockPrefab;
                        break;
                    case EntityType.UnMovable:
                        break;
                    case EntityType.Heavy:
                        prefab = heavyBlockPrefab;
                        break;
                    case EntityType.EndTile:
                        prefab = endTilePrefab;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                var entity = Instantiate(prefab, transform, true);
                entity.cell = _grid.cells[(int) cell.gridPosition.x, (int) cell.gridPosition.y];
                entity.gridPosition = cell.gridPosition;
                entity.type = prefab.type;
                entity.transform.GetComponent<SpriteRenderer>().size = Vector2.one *
                                                                       _grid.cells[(int) cell.gridPosition.x, (int) cell.gridPosition.y].size;
                _grid.cells[(int) cell.gridPosition.x, (int) cell.gridPosition.y].currentEntity = entity;
                if (prefab.type != EntityType.Player && prefab.type != EntityType.EndTile)
                    continue;
                if(prefab.type == EntityType.EndTile)
                    _endTile = (EndTile)entity;
                else
                    _player = (Player)entity;


            }
        }

        public bool GetPatternData(GridPattern pattern)
        {
            _dimensions.x = pattern.nrOfColumns;
            _dimensions.y = pattern.nrOfRows;
            return pattern.Valid;
        }

        private void Awake()
        {
            _touchController = new TouchController();
        }

        private void Update()
        {
            _grid.UpdateCellsInWorld();
            var swipeDir = _touchController.GetSwipeDirection();
            if (swipeDir != Vector2.zero)
                _grid.MoveInDirection(_player, swipeDir, moveStrength);
            winState = _grid.CheckForCrampedCell(_player, _endTile);
            _grid.UpdateCellsInGrid();
        }

        private void OnDisable()
        {
            _grid.Deconstruct();
            winState = false;
            _player.Destroy();
            _player = null;
            _endTile.Destroy();
            _endTile = null;
        }

        private bool WinCondition(Entity entity, Vector2 dir)
        {
            return entity == _player && _grid.GetCellFromGridPosition(entity.gridPosition + dir).currentEntity == _endTile;
        }

    }
}
