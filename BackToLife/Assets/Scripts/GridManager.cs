using System;
using System.Collections.Generic;
using UnityEngine;

namespace BackToLife
{
    [Serializable]
    public class GridManager : MonoBehaviour
    {
        [SerializeField]private List<GridPattern> gridPatterns;

        public bool MorePatterns()
        {
            return gridPatterns.Count > 0;
        }
        
        public GridPattern GetNextPattern()
        {
            var pattern = gridPatterns[0];
            gridPatterns.Remove(pattern);
            return pattern;
        }
    }
}