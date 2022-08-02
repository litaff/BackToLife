using UnityEngine;

namespace BackToLife
{
    public class Page : MonoBehaviour
    {
        public PageType type;

        public void SetActive(bool state)
        {
            gameObject.SetActive(state);
        }

        public void Close()
        {
            SetActive(false);   
        }
        
        public enum PageType
        {
            Menu,
            EndLevel,
            Gameplay,
            Browser,
            Editor,
            Size,
            CellMod,
            CompleteTest,
            TestingError
        }
    }
}