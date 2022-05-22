﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace BackToLife
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField]private List<GridPattern> gridPatterns;

        private UIManager _uiManager;
        private GridPattern _currentPattern;

        private void Awake()
        {
            _uiManager = gameObject.transform.parent.GetComponentInChildren<UIManager>();
            _currentPattern = GetNextPattern();
        }

        private void Start()
        {
            _uiManager.SetPageActive(Page.PageType.Title, true);
        }
        

        private bool MorePatterns()
        {
            return gridPatterns.Count > 0;
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
            _uiManager.SetPageActive(Page.PageType.Title, false);
            if (_currentPattern.Valid)
            {
                _uiManager.SetPageActive(Page.PageType.Game, true);
                return _currentPattern;
            }
            Debug.LogError($"{_currentPattern.name} is not valid!");
            return null;

        }

        public GridPattern GetNextLevel()
        {
            _currentPattern = null;
            _uiManager.SetPageActive(Page.PageType.Win, false);
            if (!MorePatterns())
            {
                _uiManager.SetPageActive(Page.PageType.NoFun, true);
                _uiManager.SetPageActive(Page.PageType.Game, false);
                return null;
            }
            _uiManager.SetPageActive(Page.PageType.Game, true);
            _currentPattern = GetNextPattern();
            return _currentPattern;

        }
        
        public void EndLevel()
        {
            _uiManager.SetPageActive(Page.PageType.Win, true);
            _uiManager.SetPageActive(Page.PageType.Game, false);
        }

        public GridPattern ResetLevel()
        {
            return _currentPattern;
        }
    }
}