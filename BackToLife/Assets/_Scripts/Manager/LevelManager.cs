using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BackToLife
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private List<GridPattern> gridPatterns;
        [SerializeField] private GridPattern blankPattern;
        private GridPattern _currentPattern;

        private void Awake()
        {
            _currentPattern = GetNextPattern();
        }

        private GridPattern GetNextPattern()
        {
            var pattern = gridPatterns[0];
            gridPatterns.Remove(pattern);
            return pattern;
        }
        
        public GridPattern StartLevel()
        {
            if(_currentPattern == null) return null;
            if (_currentPattern.Valid)
            {
                return _currentPattern;
            }
            Debug.LogError($"{_currentPattern.name} is not valid!");
            return null;
        }

        public GridPattern BlankLevel()
        {
            _currentPattern = Instantiate(blankPattern);
            return _currentPattern;
        }

        public GridPattern ResetLevel()
        {
            return _currentPattern;
        }

        public void ResizePattern(Vector2 size)
        {
            _currentPattern.nrOfColumns = (int)size.x;
            _currentPattern.nrOfRows = (int)size.y;
        }
        
        public void AddToPattern(Vector2 position, Entity.EntityType entityType, 
            Block.BlockType blockType = Block.BlockType.None, 
            Tile.TileType tileType = Tile.TileType.None)
        {
            _currentPattern.cells.Add(new GridPattern.PatternCell
            {
                blockType = blockType,
                entityType = entityType,
                tileType = tileType,
                gridPosition = position
            });
        }

        public void RemoveFromPattern(Vector2 position)
        {
            foreach (var cell in _currentPattern.cells.ToList().Where(cell => cell.gridPosition == position))
            {
                _currentPattern.cells.Remove(cell);
                return;
            }
        }
    }
}