using System.Collections;
using UnityEngine;

// TODO: add error messages if StartTesting method fails, sync gridUpdateSpeed with gameManager

namespace BackToLife
{
    public class EditorManager : MonoBehaviour
    {
        public float gridUpdateSpeed;
        public SizeSliderHandler sliderHandler;
        public CellModHandler cellModHandler;
        private bool _edited;
        private LevelManager _levelManager;
        private InputManager _inputManager;
        private GridManager _gridManager;
        private UIManager _uiManager;
        
        private bool GridFocused => !(sliderHandler.gameObject.activeSelf || cellModHandler.gameObject.activeSelf);

        public void TestLevel()
        {
            if (!_levelManager.StartAble)
            {
                _uiManager.ShowMessage("A level must contain exactly one player and end tile!", UIManager.MsgType.Warning);
                return;
            }

            _levelManager.LoadEditorLevel();
            
            GameManager.testRun = true;
            
            SceneManager.LoadScene(SceneManager.SceneType.Gameplay);
        }
        
        public void NewPattern()
        {
            _levelManager.NewEditorLevel();
        }

        public void Resize()
        {
            StartCoroutine(EditDelay(.5f));
            _levelManager.ResizePattern(sliderHandler.GetSize());
        }

        public void Modify()
        {
            StartCoroutine(EditDelay(.5f));
            _levelManager.ModifyCell(cellModHandler.GetPatternCell());
        }

        public void Delete()
        {
            StartCoroutine(EditDelay(.5f));
            _levelManager.RemoveFromPattern(cellModHandler.GetGridPosition());
        }

        private void OpenCellMod()
        {
            if (!GridFocused) return;
            if (_edited) return;
            var touchPosition = _inputManager.GetTouchPosition();
            var gridPosition = _levelManager.GetGridPositionFromTouch(touchPosition);
            if (gridPosition == new Vector2(-1, -1)) return;
            var patternCell = _levelManager.GetPatternCellFromGridPosition(gridPosition);
            cellModHandler.gameObject.SetActive(true); // activate before SetPatternCell
            cellModHandler.SetPatternCell(patternCell, gridPosition);
        }

        private IEnumerator EditDelay(float time)
        {
            _edited = true;
            yield return new WaitForSeconds(time);
            _edited = false;
        }

        private void Awake()
        {
            _levelManager = FindObjectOfType<LevelManager>();
            _gridManager = FindObjectOfType<GridManager>();
            _uiManager = FindObjectOfType<UIManager>();
            _inputManager = new InputManager();
            _levelManager.LoadEditorLevel();
            _gridManager.InitializeGrid(_levelManager.LoadLevel());
            LevelManager.PatternChange += () => _gridManager.InitializeGrid(_levelManager.ResetLevel());
        }

        private void Update()
        {
            _gridManager.EditorUpdate(); 
            
            OpenCellMod();
        }
    }
}