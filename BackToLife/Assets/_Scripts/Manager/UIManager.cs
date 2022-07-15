using System;
using System.Collections.Generic;
using System.Linq;
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
                                 $"only the first one will be active");

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

        private void AddListenerToButton(Button button)
        {
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
            
            if (button.CompareTag("Size button"))
            {
                button.onClick.AddListener(() => SetPageActive(Page.PageType.Size, true));
                return;
            }

            if (button.CompareTag("Size confirm button"))
            {
                button.onClick.AddListener(_editorManager.Resize);
                button.onClick.AddListener(() => SetPageActive(Page.PageType.Size, false));
                return;
            }

            if (button.CompareTag("Delete cell button"))
            {
                button.onClick.AddListener(_editorManager.Delete);
                button.onClick.AddListener(() => SetPageActive(Page.PageType.CellMod, false));
                return;
            }

            if (button.CompareTag("Submit cell button"))
            {
                button.onClick.AddListener(_editorManager.Modify);
                button.onClick.AddListener(() => SetPageActive(Page.PageType.CellMod, false));
                return;
            }
        }
        
    }
}