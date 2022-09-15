using System.Collections;
using UnityEngine;

// TODO: add error messages if StartTesting method fails

namespace BackToLife
{
    public class EditorManager : MonoBehaviour
    {
        public Canvas canvas;
        public SizeSliderHandler sliderHandler;
        public CellModHandler cellModHandler;
        public bool testing;
        private bool _submitAble;
        private bool _edited;
        private LevelManager _levelManager;
        private InputManager _inputManager;

        public void NewPattern()
        {
            _levelManager.NewEditorLevel();
            _submitAble = false;
        }
        
        public void StartTesting()
        {
            testing = _levelManager.StartAble;
            _submitAble = false;
        }

        public void PatternSubmitAble()
        {
            testing = false;
            _submitAble = true;
        }

        public void Resize()
        {
            StartCoroutine(EditDelay());
            _levelManager.ResizePattern(sliderHandler.GetSize());
            _submitAble = false;
        }

        public void Modify()
        {
            StartCoroutine(EditDelay());
            _levelManager.ModifyCell(cellModHandler.GetPatternCell());
            _submitAble = false;
        }
        
        public void Delete()
        {
            StartCoroutine(EditDelay());
            _levelManager.RemoveFromPattern(cellModHandler.GetGridPosition());
            _submitAble = false;
        }

        private void OpenCellMod()
        {
            if (Helper.IsOverUI()) return;
            if (testing) return;
            if (_edited) return;
            var touchPosition = _inputManager.GetTouchPosition();
            var gridPosition = _levelManager.GetGridPositionFromTouch(touchPosition);
            if (gridPosition == new Vector2(-1, -1)) return;
            var patternCell = _levelManager.GetPatternCellFromGridPosition(gridPosition);
            cellModHandler.gameObject.SetActive(true); // activate before SetPatternCell
            cellModHandler.SetPatternCell(patternCell, gridPosition);
        }

        private IEnumerator EditDelay()
        {
            _edited = true;
            yield return new WaitForSeconds(1);
            _edited = false;
        }

        private void Awake()
        {
            _levelManager = FindObjectOfType<LevelManager>();
            _inputManager = new InputManager();
        }

        private void Update()
        {
            OpenCellMod();
            if(_submitAble)
                Debug.Log("OK");
        }
    }
}