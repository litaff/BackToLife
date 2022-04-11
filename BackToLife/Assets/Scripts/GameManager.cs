using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BackToLife
{
    public class GameManager : MonoBehaviour
    {
        public int nrOfRows;
        public int nrOfColumns;
        public float horizontalMargin;
        public float verticalMargin;
        public Block block;
        private GameGrid grid;
        private void Awake()
        {
            InitializeGrid();
            foreach (var cell in grid.Cells)
            {
                var b = Instantiate(block, transform, true);
                cell.CurrentEntity = b;
            }
        }

        private void Update()
        {
            grid.UpdateCells();
        }

        private void InitializeGrid()
        {
            grid = new GameGrid(nrOfRows,nrOfColumns, transform.position, horizontalMargin,verticalMargin);
        }
        
    }
}
