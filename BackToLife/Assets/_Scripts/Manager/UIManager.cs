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

        private void Awake()
        {
            if(uiPages.Count != uiPages.Distinct().Count())
                Debug.LogWarning($"{gameObject.name} has more than one page of the same type, " +
                                 $"only the first one will be active");

            _gameManager = GetComponentInParent<GameManager>();
            _sceneManager = transform.parent.GetComponentInChildren<SceneManager>();
            
            foreach (var newPage in uiPages.Select(page => Instantiate(page, canvas.transform)))
            {
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
            if(button.CompareTag("Tutorial button"))
                button.onClick.AddListener(_gameManager.StartLevel);
            if (button.CompareTag("Level browser button"))
                button.onClick.AddListener(_sceneManager.LoadScene(SceneManager.SceneType.Browser));
            if (button.CompareTag("Main menu button"))
            {
                button.onClick.AddListener(_sceneManager.LoadScene(SceneManager.SceneType.Menu));
                button.onClick.AddListener(_gameManager.EndAll);
            }
            if(button.CompareTag("Reset button"))
                button.onClick.AddListener(_gameManager.ResetLevel);
        }
        
    }
}