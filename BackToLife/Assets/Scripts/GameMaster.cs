using System;
using UnityEngine;

namespace BackToLife
{
    public class GameMaster : MonoBehaviour
    {
        public GameManager gameManager;
        public UIManager uiManager;
        public GridManager gridManager;
        public GridPattern currentPattern;

        private void Awake()
        {
            gameManager = GetComponentInChildren<GameManager>();
            gameManager.enabled = false;
            uiManager = GetComponentInChildren<UIManager>();
            uiManager.SetPageActive(Page.PageType.Title, true);
            gridManager = GetComponentInChildren<GridManager>();
            if (gridManager.MorePatterns())
                currentPattern = gridManager.GetNextPattern();
        }

        
        
        private void Update()
        {
            if(gameManager.winState)
                EndLevel();
        }

        public void StartLevel()
        {
            if(currentPattern == null) return;
            uiManager.SetPageActive(Page.PageType.Title, false);
            gameManager.enabled = true;
            if (!gameManager.GetPatternData(currentPattern))
                Debug.LogError($"{currentPattern.name} is not valid!");
            gameManager.InitializeGrid(currentPattern);
        }

        public void GetNextLevel()
        {
            currentPattern = null;
            uiManager.SetPageActive(Page.PageType.Win, false);
            if (!gridManager.MorePatterns())
            {
                uiManager.SetPageActive(Page.PageType.NoFun, true);
                return;
            }
            currentPattern = gridManager.GetNextPattern();
            
        }
        
        private void EndLevel()
        {
            gameManager.enabled = false;
            uiManager.SetPageActive(Page.PageType.Win, true);
        }
        
        
    }
}