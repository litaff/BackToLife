using System.Collections;
using UnityEngine;

// TODO: Implement editor, accessed from level browser, requires completion to finish making
namespace BackToLife
{
    public class EditorManager : MonoBehaviour
    {
        public Canvas canvas;
        public SizeSliderHandler sliderHandler;
        public CellModHandler cellModHandler;
        private bool _edited;
        private LevelManager _levelManager;
        private InputManager _inputManager;

        public void Resize()
        {
            StartCoroutine(EditDelay());
            _levelManager.ResizePattern(sliderHandler.GetSize());
        }

        public void Modify()
        {
            StartCoroutine(EditDelay());
            _levelManager.ModifyCell(cellModHandler.GetPatternCell());
        }
        
        public void Delete()
        {
            StartCoroutine(EditDelay());
            _levelManager.RemoveFromPattern(cellModHandler.GetGridPosition());
        }

        public bool OpenCellMod()
        {
            if (_edited) return false;
            var touchPosition = _inputManager.GetTouchPosition();
            var gridPosition = _levelManager.GetGridPositionFromTouch(touchPosition);
            if (gridPosition == new Vector2(-1, -1)) return false;
            var patternCell = _levelManager.GetPatternCellFromGridPosition(gridPosition);
            cellModHandler.SetPatternCell(patternCell, gridPosition);
            return true;
        }

        public IEnumerator EditDelay()
        {
            _edited = true;
            yield return new WaitForSeconds(1);
            _edited = false;
        }

        private void Awake()
        {
            _levelManager = transform.parent.GetComponentInChildren<LevelManager>();
            _inputManager = new InputManager();
        }

        private void Update()
        {
            
        }
    }
}