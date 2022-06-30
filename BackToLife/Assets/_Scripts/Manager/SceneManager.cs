using System;
using UnityEngine;
using UnityEngine.Events;

namespace BackToLife
{
    public class SceneManager : MonoBehaviour
    {
        private UIManager _uiManager;
        private LevelManager _levelManager;

        private void Awake()
        {
            var parent = transform.parent;
            _uiManager = parent.GetComponentInChildren<UIManager>();
            _levelManager = parent.GetComponentInChildren<LevelManager>();
        }

        public UnityAction LoadScene(SceneType scene)
        {
            UnityAction action = scene switch
            {
                SceneType.Title => LoadTitle,
                SceneType.Gameplay => LoadGameplay,
                SceneType.Browser => LoadBrowser,
                SceneType.Editor => LoadEditor,
                SceneType.Progress => LoadProgress,
                _ => throw new ArgumentOutOfRangeException(nameof(scene), scene, null)
            };

            return action;
        }

        private void LoadTitle()
        {
            _uiManager.SetPageActive(Page.PageType.Title, true);
        }

        private void LoadGameplay()
        {
            _uiManager.SetPageActive(Page.PageType.Title, false);
            _uiManager.SetPageActive(Page.PageType.Gameplay, true);
        }

        private void LoadBrowser()
        {
            _uiManager.SetPageActive(Page.PageType.Title,false);
            _uiManager.SetPageActive(Page.PageType.Browser,true);
        }

        private void LoadEditor()
        {
        }

        private void LoadProgress()
        {
            _uiManager.SetPageActive(Page.PageType.Win, true);
            _uiManager.SetPageActive(Page.PageType.Gameplay, false);
        }
        
        public enum SceneType
        {
            Title,
            Gameplay,
            Browser,
            Editor,
            Progress
        }
    }
}