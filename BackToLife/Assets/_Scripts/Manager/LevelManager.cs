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
        [SerializeField] private GridPattern editorPattern;
        private GridPattern _currentPattern;
        public bool StartAble => _currentPattern.Valid;
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

        public void NewEditorLevel()
        {
            editorPattern = Instantiate(blankPattern);
            PatternChange?.Invoke();
        }

        public GridPattern ResetLevel()
        {
            return _currentPattern;
        }

        public GridPattern LoadEditorLevel()
        {
            return editorPattern ? editorPattern : editorPattern = Instantiate(blankPattern);
        }

        public void ResizePattern(Vector2 size)
        {
            editorPattern.nrOfColumns = (int)size.x;
            editorPattern.nrOfRows = (int)size.y;
            PatternChange?.Invoke();
        }
        
        public void ModifyCell(GridPattern.PatternCell patternCell)
        {
            foreach (var cell in editorPattern.cells.Where(cell => cell.gridPosition == patternCell.gridPosition))
            {
                cell.entityType = patternCell.entityType;
                cell.blockType = patternCell.blockType;
                cell.tileType = patternCell.tileType;
                PatternChange?.Invoke();
                return;
            }
            editorPattern.cells.Add(patternCell);
            PatternChange?.Invoke();
        }

        public void RemoveFromPattern(Vector2 position)
        {
            foreach (var cell in editorPattern.cells.ToList().Where(cell => cell.gridPosition == position))
            {
                editorPattern.cells.Remove(cell);
                PatternChange?.Invoke();
                return;
            }
        }
        
        /// <returns> touch out of grid if return value is -1,-1 </returns>
        public Vector2 GetGridPositionFromTouch(Vector2 touchPosition)
        {
            var gridSize = new Vector2(0.5f * editorPattern.nrOfColumns, 0.5f * editorPattern.nrOfRows); // 0.5 = cell size
            var gridPosition = new Vector2(-1,-1);
            for (var row = 0; row < editorPattern.nrOfRows; row++)
            {
                for (var column = 0; column < editorPattern.nrOfColumns; column++)
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
            return editorPattern.cells.FirstOrDefault(cell => cell.gridPosition == gridPosition);
        }
    }
}