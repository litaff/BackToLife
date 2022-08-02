using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

// TODO: look further into improving this \/

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
        private EditorManager _editorManager;

        
        private void Awake()
        {
            _inputManager = new InputManager();
            _levelManager = GetComponentInChildren<LevelManager>();
            _gridManager = GetComponentInChildren<GridManager>();
            _sceneManager = GetComponentInChildren<SceneManager>();
            _editorManager = GetComponentInChildren<EditorManager>();
            _gridManager.enabled = false;
            _editorManager.enabled = false;
        }

        private void Start()
        {
            _sceneManager.LoadScene(SceneManager.SceneType.Menu).Invoke();
            LevelManager.PatternChange += () => _gridManager.InitializeGrid(_levelManager.LoadEditorLevel());
        }

        public void StartLevel()
        {
            _gridManager.enabled = true;
            _gridManager.InitializeGrid(_levelManager.StartLevel());
        }

        public void StartEditor()
        {
            _gridManager.enabled = true;
            _editorManager.enabled = true;
            _gridManager.InitializeGrid(_levelManager.LoadEditorLevel());
        }

        public void ResetLevel()
        {
            _gridManager.enabled = false;
            _gridManager.enabled = true;
            _gridManager.InitializeGrid(_levelManager.ResetLevel());
        }

        public void EndAll()
        {
            _gridManager.enabled = false;
        }

        private void Update()
        {
            if (_sceneManager.loadedScene == SceneManager.SceneType.Menu)
            {
                _gridManager.enabled = false;
                _editorManager.enabled = false;
            }
            if (_sceneManager.loadedScene == SceneManager.SceneType.Editor)
            {
                if (!_gridManager.enabled) return;
                if (_editorManager.testing)
                {
                    _gridManager.GameUpdate(_inputManager.GetSwipeDirection(),gridUpdateSpeed);
                    if (!_gridManager.WinCondition()) return;
                    _editorManager.PatternSubmitAble();
                    _sceneManager.LoadScene(SceneManager.SceneType.SubmitLevel).Invoke();
                    _gridManager.enabled = false;
                }
                else
                {
                    _gridManager.EditorUpdate();
                }
            }
            if (_sceneManager.loadedScene == SceneManager.SceneType.Gameplay)
            {
                if (!_gridManager.enabled) return;
                _gridManager.GameUpdate(_inputManager.GetSwipeDirection(),gridUpdateSpeed);
                if (!_gridManager.WinCondition()) return;
                _sceneManager.LoadScene(SceneManager.SceneType.EndLevel).Invoke();
                _gridManager.enabled = false;
            }
        }
    }
}
