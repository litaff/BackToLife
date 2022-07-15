using UnityEngine;

namespace BackToLife
{
    public class CellModHandler : MonoBehaviour
    {
        public GameObject entityType;
        public GameObject blockType;
        public GameObject tileType;
        // this needs grid position
        public GridPattern.PatternCell GetPatternCell()
        {
            return new GridPattern.PatternCell();
        }
        // this needs grid position
        public Vector2 GetGridPosition()
        {
            return new Vector2();
        }
    }
}