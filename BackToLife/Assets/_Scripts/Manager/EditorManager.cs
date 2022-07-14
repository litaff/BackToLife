using System;
using UnityEngine;
using UnityEngine.UI;

// TODO: Implement editor, accessed from level browser, requires completion to finish making

namespace BackToLife
{
    public class EditorManager : MonoBehaviour
    {
        public Canvas canvas;
        public SliderHandler sliderHandler;
        private LevelManager _levelManager;

        public static event Action PatternChange;

        public void OnPatternChange()
        {
            _levelManager.ResizePattern(sliderHandler.GetSliderSize());
            PatternChange?.Invoke();
        }
        
        private void Awake()
        {
            _levelManager = transform.parent.GetComponentInChildren<LevelManager>();
            sliderHandler = canvas.GetComponentInChildren<SliderHandler>();
        }

    }
}