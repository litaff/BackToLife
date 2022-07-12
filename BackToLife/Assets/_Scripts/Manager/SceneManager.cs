using System;
using UnityEngine;
using UnityEngine.Events;

namespace BackToLife
{
    public class SceneManager : MonoBehaviour
    {
        private UIManager _uiManager;

        private void Awake()
        {
            var parent = transform.parent;
            _uiManager = parent.GetComponentInChildren<UIManager>();
        }

        public UnityAction LoadScene(SceneType scene)
        {
            UnityAction action = scene switch
            {
                SceneType.Menu => LoadTitle,
                SceneType.Gameplay => LoadGameplay,
                SceneType.Browser => LoadBrowser,
                SceneType.Editor => LoadEditor,
                SceneType.EndLevel => LoadEndLevel,
                _ => throw new ArgumentOutOfRangeException(nameof(scene), scene, null)
            };

            return action;
        }

        private void LoadTitle()
        {
            _uiManager.SetAllPagesActive(false);
            _uiManager.SetPageActive(Page.PageType.Menu, true);
        }

        private void LoadGameplay()
        {
            _uiManager.SetAllPagesActive(false);
            _uiManager.SetPageActive(Page.PageType.Gameplay, true);
        }

        private void LoadBrowser()
        {
            _uiManager.SetAllPagesActive(false);
            _uiManager.SetPageActive(Page.PageType.Browser,true);
        }

        private void LoadEditor()
        {
            _uiManager.SetAllPagesActive(false);
        }

        private void LoadEndLevel()
        {
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