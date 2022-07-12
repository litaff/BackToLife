using System;
using System.Collections.Generic;
using UnityEngine;

namespace BackToLife
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField]private List<GridPattern> gridPatterns;

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

        public GridPattern ResetLevel()
        {
            return _currentPattern;
        }
    }
}