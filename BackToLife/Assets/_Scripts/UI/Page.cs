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
        public enum PageType
        {
            Title,
            Win,
            NoFun,
            Gameplay,
            Browser
        }
    }
}