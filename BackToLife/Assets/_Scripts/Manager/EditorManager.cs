using System;
using UnityEngine;
using UnityEngine.UI;

// TODO: Implement editor, accessed from level browser, requires completion to finish making
// TODO: Check if popup is open to disable touch to grid check
namespace BackToLife
{
    public class EditorManager : MonoBehaviour
    {
        public Canvas canvas;
        public SizeSliderHandler sliderHandler;
        public CellModHandler cellModHandler;
        private LevelManager _levelManager;
        private InputManager _inputManager;
        
        public void Resize()
        {
            _levelManager.ResizePattern(sliderHandler.GetSize());
        }

        public void Modify()
        {
            //_levelManager.ModifyCell(cellModHandler.GetPatternCell());
        }
        
        public void Delete()
        {
            //_levelManager.RemoveFromPattern(cellModHandler.GetGridPosition());
        }
        
        private void Awake()
        {
            _levelManager = transform.parent.GetComponentInChildren<LevelManager>();
            _inputManager = new InputManager();
        }

        private void Update()
        {
            print(_levelManager.GetGridPositionFromTouch(_inputManager.GetTouchPosition()));
        }
    }
}