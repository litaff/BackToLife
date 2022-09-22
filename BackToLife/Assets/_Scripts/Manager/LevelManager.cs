using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// TODO: transfer level data from editor

namespace BackToLife
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private GridPattern blankPattern;
        private static GridPattern _currentPattern;
        private static GridPattern _editorPattern;
        public bool StartAble => _currentPattern.Valid;
        public static event Action PatternChange;

        public GridPattern LoadLevel()
        {
            return _currentPattern;
        }

        public void NewEditorLevel()
        {
            _editorPattern = Instantiate(blankPattern);
            PatternChange?.Invoke();
        }

        public GridPattern ResetLevel()
        {
            return _currentPattern;
        }

        public void LoadEditorLevel()
        {
            if(_editorPattern == null)
                NewEditorLevel();

            _currentPattern = _editorPattern;
        }

        public void ResizePattern(Vector2 size)
        {
            _editorPattern.nrOfColumns = (int)size.x;
            _editorPattern.nrOfRows = (int)size.y;
            PatternChange?.Invoke();
        }
        
        public void ModifyCell(GridPattern.PatternCell patternCell)
        {
            foreach (var cell in _editorPattern.cells.Where(cell => cell.gridPosition == patternCell.gridPosition))
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
            foreach (var cell in _editorPattern.cells.ToList().Where(cell => cell.gridPosition == position))
            {
                _editorPattern.cells.Remove(cell);
                PatternChange?.Invoke();
                return;
            }
        }
        
        /// <returns> touch out of grid if return value is -1,-1 </returns>
        public Vector2 GetGridPositionFromTouch(Vector2 touchPosition)
        {
            var gridSize = new Vector2(0.5f * _editorPattern.nrOfColumns, 0.5f * _editorPattern.nrOfRows); // 0.5 = cell size
            var gridPosition = new Vector2(-1,-1);
            for (var row = 0; row < _editorPattern.nrOfRows; row++)
            {
                for (var column = 0; column < _editorPattern.nrOfColumns; column++)
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
            return _editorPattern.cells.FirstOrDefault(cell => cell.gridPosition == gridPosition);
        }

        public void OnDestroy()
        {
            PatternChange = null;
        }

        private void Awake()
        {
            PatternChange += LoadEditorLevel;
        }
    }
}