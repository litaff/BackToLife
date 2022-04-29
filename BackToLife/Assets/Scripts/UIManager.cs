using System;
using UnityEngine;

namespace BackToLife
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Page titlePage;
        [SerializeField] private Page winPage;
        [SerializeField] private Page noFunPage;

        private void Awake()
        {
            titlePage.SetActive(false);
            winPage.SetActive(false);
            noFunPage.SetActive(false);
        }

        public void SetPageActive(Page.PageType type, bool state)
        {
            switch (type)
            {
                case Page.PageType.Title:
                    titlePage.SetActive(state);
                    break;
                case Page.PageType.Win:
                    winPage.SetActive(state);
                    break;
                case Page.PageType.NoFun:
                    noFunPage.SetActive(state);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
        
        
    }
}