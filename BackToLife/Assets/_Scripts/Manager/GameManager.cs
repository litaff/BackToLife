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
        private InputManager _inputManager;
        private GridManager _gridManager;

        private LevelManager _levelManager;

        
        private void Awake()
        {
            _inputManager = new InputManager();
            _levelManager = GetComponentInChildren<LevelManager>();
            _gridManager = GetComponentInChildren<GridManager>();
            _gridManager.enabled = false;
        }

        public void StartLevel()
        {
            _gridManager.enabled = true;
            _gridManager.InitializeGrid(_levelManager.StartLevel());
            
        }

        public void GetNextLevel()
        {
            _gridManager.enabled = true;
            _gridManager.InitializeGrid(_levelManager.GetNextLevel());
        }

        public void ResetLevel()
        {
            _gridManager.enabled = false;
            _gridManager.enabled = true;
            _gridManager.InitializeGrid(_levelManager.ResetLevel());
        }

        private void Update()
        {
            if (!_gridManager.enabled) return;
            _gridManager.GameUpdate(_inputManager.GetSwipeDirection());
            if (!_gridManager.WinCondition()) return;
            _levelManager.EndLevel();
            _gridManager.enabled = false;
        }
    }
}
