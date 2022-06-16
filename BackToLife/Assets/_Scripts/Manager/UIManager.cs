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

        private void Awake()
        {
            if(uiPages.Count != uiPages.Distinct().Count())
                Debug.LogWarning($"{gameObject.name} has more than one page of the same type, " +
                                 $"only the first one will be active");

            _gameManager = GetComponentInParent<GameManager>();
            
            foreach (var newPage in uiPages.Select(page => Instantiate(page, canvas.transform)))
            {
                newPage.SetActive(false);
                var button = newPage.GetComponentInChildren<Button>();
                switch (newPage.type)
                {
                    case Page.PageType.Title:
                        button.onClick.AddListener(_gameManager.StartLevel);
                        break;
                    case Page.PageType.Win:
                        button.onClick.AddListener(_gameManager.GetNextLevel);
                        break;
                    case Page.PageType.NoFun:
                        break;
                    case Page.PageType.Game:
                        button.onClick.AddListener(_gameManager.ResetLevel);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
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
        
        
    }
}