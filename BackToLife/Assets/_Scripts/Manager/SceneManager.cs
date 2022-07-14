using System;
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
                _ => throw new ArgumentOutOfRangeException(nameof(scene), scene, null)
            };

            return action;
        }

        private void LoadMenu()
        {
            loadedScene = SceneType.Menu;
            _uiManager.SetAllPagesActive(false);
            _uiManager.SetPageActive(Page.PageType.Menu, true);
        }

        private void LoadGameplay()
        {
            loadedScene = SceneType.Gameplay;
            _uiManager.SetAllPagesActive(false);
            _uiManager.SetPageActive(Page.PageType.Gameplay, true);
        }

        private void LoadBrowser()
        {
            loadedScene = SceneType.Browser;
            _uiManager.SetAllPagesActive(false);
            _uiManager.SetPageActive(Page.PageType.Browser,true);
        }

        private void LoadEditor()
        {
            loadedScene = SceneType.Editor;
            _uiManager.SetAllPagesActive(false);
            _uiManager.SetPageActive(Page.PageType.Editor,true);
        }

        private void LoadEndLevel()
        {
            loadedScene = SceneType.EndLevel;
            _uiManager.SetAllPagesActive(false);
            _uiManager.SetPageActive(Page.PageType.EndLevel, true);
        }
        
        
        public enum SceneType
        {
            Menu,
            Gameplay,
            Browser,
            Editor,
            EndLevel
        }
    }
}