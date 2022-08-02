using System;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Events;

namespace BackToLife
{
    public class SceneManager : MonoBehaviour
    {
        public SceneType loadedScene;
        private UIManager _uiManager;

        private void Awake()
        {
            _uiManager = transform.parent.GetComponentInChildren<UIManager>();
        }

        public UnityAction LoadScene(SceneType scene)
        {
            UnityAction action = scene switch
            {
                SceneType.Menu => LoadMenu,
                SceneType.Gameplay => LoadGameplay,
                SceneType.Browser => LoadBrowser,
                SceneType.Editor => LoadEditor,
                SceneType.EndLevel => LoadEndLevel,
                SceneType.SubmitLevel => LoadSubmitLevel,
                _ => throw new ArgumentOutOfRangeException(nameof(scene), scene, null)
            };

            return action;
        }

        private void LoadMenu()
        {
            print($"Unloaded: {loadedScene}");
            loadedScene = SceneType.Menu;
            //_uiManager.SetAllPagesActive(false);
            _uiManager.SetPageActive(Page.PageType.Menu, true);
            print($"Loaded: {loadedScene}");
            }

        private void LoadGameplay()
        {
            print($"Unloaded: {loadedScene}");
            loadedScene = SceneType.Gameplay;
            //_uiManager.SetAllPagesActive(false);
            _uiManager.SetPageActive(Page.PageType.Gameplay, true);
            print($"Loaded: {loadedScene}");        }

        private void LoadBrowser()
        {
            print($"Unloaded: {loadedScene}");
            loadedScene = SceneType.Browser;
            //_uiManager.SetAllPagesActive(false);
            _uiManager.SetPageActive(Page.PageType.Browser,true);
            print($"Loaded: {loadedScene}");        }

        private void LoadEditor()
        {
            print($"Unloaded: {loadedScene}");
            loadedScene = SceneType.Editor;
            //_uiManager.SetAllPagesActive(false);
            _uiManager.SetPageActive(Page.PageType.Editor,true);
            print($"Loaded: {loadedScene}");        }

        private void LoadEndLevel()
        {
            print($"Unloaded: {loadedScene}");
            loadedScene = SceneType.EndLevel;
            //_uiManager.SetAllPagesActive(false);
            _uiManager.SetPageActive(Page.PageType.EndLevel, true);
            print($"Loaded: {loadedScene}");        }
        
        private void LoadSubmitLevel()
        {
            print($"Unloaded: {loadedScene}");
            loadedScene = SceneType.SubmitLevel;
            //_uiManager.SetAllPagesActive(false);
            _uiManager.SetPageActive(Page.PageType.CompleteTest, true);
            print($"Loaded: {loadedScene}");
            
        }
        
        
        public enum SceneType
        {
            Menu,
            Gameplay,
            Browser,
            Editor,
            EndLevel,
            SubmitLevel
        }
    }
}