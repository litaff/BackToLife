using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace BackToLife
{
    public class UIManager : MonoBehaviour
    {
        public List<Page> uiPages;
        [SerializeField] private Canvas canvas;
        private readonly List<Page> _uiPages = new List<Page>();
        private GameManager _gameManager;
        private SceneManager _sceneManager;
        private BrowserManager _browserManager;
        private EditorManager _editorManager;

        private void Awake()
        {
            var parent = transform.parent;
            
            if(uiPages.Count != uiPages.Distinct().Count())
                Debug.LogWarning($"{gameObject.name} has more than one page of the same type, " +
                                 "only the first one will be active");

            _gameManager = GetComponentInParent<GameManager>();
            _sceneManager = parent.GetComponentInChildren<SceneManager>();
            _browserManager = parent.GetComponentInChildren<BrowserManager>();
            _editorManager = parent.GetComponentInChildren<EditorManager>();
            
            foreach (var newPage in uiPages.Select(page => Instantiate(page, canvas.transform)))
            {
                if (newPage.type == Page.PageType.Size)
                    _editorManager.sliderHandler = newPage.GetComponent<SizeSliderHandler>();
                if (newPage.type == Page.PageType.CellMod)
                    _editorManager.cellModHandler = newPage.GetComponent<CellModHandler>();
                newPage.SetActive(false);
                var buttons = newPage.GetComponentsInChildren<Button>();
                foreach (var button in buttons)
                    AddListenerToButton(button);
                _uiPages.Add(newPage);
            }
        }

        public void SetPageActive(Page.PageType type, bool state)
        {
            foreach (var page in _uiPages.Where(page => page.type == type))
            {
                page.SetActive(state);
            }
        }

        public void SetAllPagesActive(bool state)
        {
            foreach (var page in _uiPages)
            {
                page.SetActive(state);
            }
        }

        private void Update()
        {
            if (_sceneManager.loadedScene == SceneManager.SceneType.Editor)
            {
                HandleEditorWindows();  
            }
        }

        private void TestingCheck()
        {
            if (_editorManager.testing)
            {
                SetPageActive(Page.PageType.Editor, false);
                _sceneManager.LoadScene(SceneManager.SceneType.Gameplay);
                return;
            }
            SetPageActive(Page.PageType.TestingError,true);
        }
        
        private void HandleEditorWindows()
        {
            if (_uiPages.Where(page => page.type == Page.PageType.CellMod).Any(page => page.gameObject.activeSelf))
            {
                return;
            }
            if (_uiPages.Where(page => page.type == Page.PageType.Size).Any(page => page.gameObject.activeSelf))
            {
                return;
            }
            if (Helper.IsOverUI()) return;
                
            if (!_editorManager.OpenCellMod()) return;
            SetPageActive(Page.PageType.CellMod, true);
        }
        
        private void AddListenerToButton(Button button)
        {
            #region General

            if (button.CompareTag("Tutorial button"))
            {
                button.onClick.AddListener(_gameManager.StartLevel);
                button.onClick.AddListener(_sceneManager.LoadScene(SceneManager.SceneType.Gameplay));
                return;
            }

            if (button.CompareTag("Level browser button"))
            {
                button.onClick.AddListener(_sceneManager.LoadScene(SceneManager.SceneType.Browser));
                return;
            }
            if (button.CompareTag("Main menu button"))
            {
                button.onClick.AddListener(_sceneManager.LoadScene(SceneManager.SceneType.Menu));
                button.onClick.AddListener(_gameManager.EndAll);
                return;
            }

            if (button.CompareTag("Reset button"))
            {
                button.onClick.AddListener(_gameManager.ResetLevel);
                return;
            }

            if (button.CompareTag("Editor button"))
            {
                button.onClick.AddListener(_gameManager.StartEditor);
                button.onClick.AddListener(_sceneManager.LoadScene(SceneManager.SceneType.Editor));
                return;
            }

            #endregion

            #region Size Config Buttons

            if (button.CompareTag("Size confirm button"))
            {
                button.onClick.AddListener(_editorManager.Resize);
                return;
            }

            #endregion

            #region Cell Mod Buttons

            if (button.CompareTag("Delete cell button"))
            {
                button.onClick.AddListener(_editorManager.Delete);
                return;
            }

            if (button.CompareTag("Submit cell button"))
            {
                button.onClick.AddListener(_editorManager.Modify);
                return;
            }

            #endregion

            #region Editor Buttons

            if (button.CompareTag("Size button"))
            {
                button.onClick.AddListener(() => SetPageActive(Page.PageType.Size, true));
                return;
            }
            
            if (button.CompareTag("Test button"))
            {
                button.onClick.AddListener(_editorManager.StartTesting);
                button.onClick.AddListener(TestingCheck);
                return;
            }
            
            if (button.CompareTag("Submit pattern button"))
            {
                return;
            }

            if (button.CompareTag("New pattern button"))
            {
                button.onClick.AddListener(_editorManager.NewPattern);
                return;
            }

            #endregion
        }
        
    }
}