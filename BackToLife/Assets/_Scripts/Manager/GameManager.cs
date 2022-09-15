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
        private EditorManager _editorManager;


        public void ResetLevel()
        {
            _gridManager.enabled = false;
            _gridManager.enabled = true;
            _gridManager.InitializeGrid(_levelManager.ResetLevel());
        }

        private void Awake()
        {
            _inputManager = new InputManager();
            _levelManager = FindObjectOfType<LevelManager>();
            _gridManager = FindObjectOfType<GridManager>();
            _editorManager = FindObjectOfType<EditorManager>();
            _gridManager.enabled = false;
            _editorManager.enabled = false;

            switch (SceneManager.loadedScene)
            {
                case SceneManager.SceneType.Menu:
                    _gridManager.enabled = false;
                    break;
                case SceneManager.SceneType.Gameplay:
                    break;
                case SceneManager.SceneType.Browser:
                    break;
                case SceneManager.SceneType.Editor:
                    StartEditor();
                    break;
                case SceneManager.SceneType.EndLevel:
                    break;
                case SceneManager.SceneType.SubmitLevel:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Start()
        {
            LevelManager.PatternChange += () => _gridManager.InitializeGrid(_levelManager.LoadEditorLevel());
        }

        private void StartLevel()
        {
            _gridManager.enabled = true;
            _gridManager.InitializeGrid(_levelManager.StartLevel());
        }

        private void StartEditor()
        {
            _gridManager.enabled = true;
            _editorManager.enabled = true;
            _gridManager.InitializeGrid(_levelManager.LoadEditorLevel());
        }

        private void Update()
        {
            if (SceneManager.loadedScene == SceneManager.SceneType.Menu)
            {
                _gridManager.enabled = false;
                _editorManager.enabled = false;
            }
            if (SceneManager.loadedScene == SceneManager.SceneType.Editor)
            {
                if (!_gridManager.enabled) return;
                if (_editorManager.testing)
                {
                    _gridManager.GameUpdate(_inputManager.GetSwipeDirection(),gridUpdateSpeed);
                    if (!_gridManager.WinCondition()) return;
                    _editorManager.PatternSubmitAble();
                    //_sceneManager.LoadScene(SceneManager.SceneType.SubmitLevel).Invoke();
                    _gridManager.enabled = false;
                }
                else
                {
                    _gridManager.EditorUpdate();
                }
            }
            if (SceneManager.loadedScene == SceneManager.SceneType.Gameplay)
            {
                if (!_gridManager.enabled) return;
                _gridManager.GameUpdate(_inputManager.GetSwipeDirection(),gridUpdateSpeed);
                if (!_gridManager.WinCondition()) return;
                //_sceneManager.LoadScene(SceneManager.SceneType.EndLevel).Invoke();
                _gridManager.enabled = false;
            }
        }
    }
}
