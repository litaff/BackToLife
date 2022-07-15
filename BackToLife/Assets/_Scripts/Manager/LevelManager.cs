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
        public static event Action PatternChange;

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
            PatternChange?.Invoke();
        }
        
        public void ModifyCell(GridPattern.PatternCell patternCell)
        {
            foreach (var cell in _currentPattern.cells.Where(cell => cell.gridPosition == patternCell.gridPosition))
            {
                cell.entityType = patternCell.entityType;
                cell.blockType = patternCell.blockType;
                cell.tileType = patternCell.tileType;
                PatternChange?.Invoke();
                return;
            }
            _currentPattern.cells.Add(patternCell);
            PatternChange?.Invoke();
        }

        public void RemoveFromPattern(Vector2 position)
        {
            foreach (var cell in _currentPattern.cells.ToList().Where(cell => cell.gridPosition == position))
            {
                _currentPattern.cells.Remove(cell);
                PatternChange?.Invoke();
                return;
            }
        }
        
        /// <returns> touch out of grid if return value is -1,-1 </returns>
        public Vector2 GetGridPositionFromTouch(Vector2 touchPosition)
        {
            var gridSize = new Vector2(0.5f * _currentPattern.nrOfColumns, 0.5f * _currentPattern.nrOfRows); // 0.5 = cell size
            var gridPosition = new Vector2(-1,-1);
            for (var row = 0; row < _currentPattern.nrOfRows; row++)
            {
                for (var column = 0; column < _currentPattern.nrOfColumns; column++)
                {
                    // GridManager is on 0,0 so transform.position can be used
                    // bottom left position of each cell
                    var pos = (Vector2)transform.position - gridSize/2 +
                                   new Vector2(0.5f * column, 0.5f * row);
                    if (!(pos.x < touchPosition.x) || !(pos.x + .5f > touchPosition.x)) continue;
                    if (pos.y < touchPosition.y && pos.y + .5f > touchPosition.y)
                    {
                        return new Vector2(column, row);
                    }
                }
            }
            return gridPosition;
        }

        /// <returns> returns null if no cell at gridPosition </returns>
        public GridPattern.PatternCell GetPatternCellFromGridPosition(Vector2 gridPosition)
        {
            return _currentPattern.cells.FirstOrDefault(cell => cell.gridPosition == gridPosition);
        }
    }
}