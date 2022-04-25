using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BackToLife
{
    [CreateAssetMenu(fileName = "GridPattern", menuName = "ScriptableObjects/GridPattern", order = 1)]
    public class GridPattern : ScriptableObject
    {
        public int nrOfRows;
        public int nrOfColumns;
        public List<Cell> cells;
        public bool Valid { get; set; }

        private void OnValidate()
        {
            Valid = true;
            if (CheckPlayer()) return;
            Valid = false;
            Debug.LogWarning($"[{name}] - GridPattern has to have exactly one player on the grid!");
        }

        private bool CheckPlayer()
        {
            return (from cell in cells where cell.entityType == EntityType.Player select cell).ToList().Count == 1;
        }

        [Serializable]
        public class Cell
        {
            public Vector2 gridPosition;
            public EntityType entityType;
        }
    }
}