using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

// TODO: Get rid of pages and use scenes with ui already setup

namespace BackToLife
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        private GameManager _gameManager;
        private BrowserManager _browserManager;
        private EditorManager _editorManager;

        private void Awake()
        {/*
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
            }*/
        }

        private void TestingCheck()
        {
            if (_editorManager.testing)
            {
                //_sceneManager.LoadScene(SceneManager.SceneType.Gameplay);
                return;
            }
        }

        private void AddListenerToButton(Button button)
        {
            #region General

            if (button.CompareTag("Level browser button"))
            {
                //button.onClick.AddListener(_sceneManager.LoadScene(SceneManager.SceneType.Browser));
                return;
            }

            if (button.CompareTag("Reset button"))
            {
                button.onClick.AddListener(_gameManager.ResetLevel);
                return;
            }

            #endregion
            

            #region Editor Buttons

            if (button.CompareTag("Test button"))
            {
                button.onClick.AddListener(_editorManager.StartTesting);
                button.onClick.AddListener(TestingCheck);
                return;
            }

            #endregion
        }
        
    }
}