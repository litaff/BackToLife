using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

// TODO: sync gridUpdateSpeed with editorManager

namespace BackToLife
{
    public class GameManager : MonoBehaviour
    {
        public bool winState;
        public float gridUpdateSpeed;
        public static bool testRun;
        private InputManager _inputManager;
        private GridManager _gridManager;
        private LevelManager _levelManager;


        public void ResetLevel()
        {
            _gridManager.enabled = false;
            _gridManager.enabled = true;
            _gridManager.InitializeGrid(_levelManager.LoadLevel());
        }

        private void Awake()
        {
            _inputManager = new InputManager();
            _levelManager = FindObjectOfType<LevelManager>();
            _gridManager = FindObjectOfType<GridManager>();
            StartLevel();
        }

        private void StartLevel()
        {
            _gridManager.enabled = true;

            _gridManager.InitializeGrid(_levelManager.LoadLevel());
        }

        private void Update()
        {
            if (!_gridManager.enabled) return;
            _gridManager.GameUpdate(_inputManager.GetSwipeDirection(),gridUpdateSpeed);
            if (!_gridManager.WinCondition()) return;

            SceneManager.LoadScene(testRun ? SceneManager.SceneType.SubmitLevel : SceneManager.SceneType.EndLevel);
        }
    }
}
