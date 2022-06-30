using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

// TODO: move everything related to buttons/scene management to SceneManager.cs

namespace BackToLife
{
    public class GameManager : MonoBehaviour
    {
        public bool winState;
        public float gridUpdateSpeed;
        private InputManager _inputManager;
        private GridManager _gridManager;
        private LevelManager _levelManager;
        private SceneManager _sceneManager;

        
        private void Awake()
        {
            _inputManager = new InputManager();
            _levelManager = GetComponentInChildren<LevelManager>();
            _gridManager = GetComponentInChildren<GridManager>();
            _sceneManager = GetComponentInChildren<SceneManager>();
            _gridManager.enabled = false;
        }

        private void Start()
        {
            _sceneManager.LoadScene(SceneManager.SceneType.Title).Invoke();
        }

        public void StartLevel()
        {
            _gridManager.enabled = true;
            _gridManager.InitializeGrid(_levelManager.StartLevel());
            _sceneManager.LoadScene(SceneManager.SceneType.Gameplay).Invoke();
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
            _gridManager.GameUpdate(_inputManager.GetSwipeDirection(),gridUpdateSpeed);
            if (!_gridManager.WinCondition()) return;
            _sceneManager.LoadScene(SceneManager.SceneType.Progress).Invoke();
            _gridManager.enabled = false;
        }
    }
}
