using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace BackToLife
{
    public class SliderHandler : MonoBehaviour
    {
        public GameObject row;
        public GameObject column;
        private SizeSlider _rows;
        private SizeSlider _columns;

        public Vector2 GetSliderSize()
        {
            return new Vector2(_columns.slider.value, _rows.slider.value);
        }
        
        private void Awake()
        {
            _rows = new SizeSlider(row.GetComponentInChildren<Slider>(), row.GetComponentInChildren<TMP_Text>());
            _columns = new SizeSlider(column.GetComponentInChildren<Slider>(), column.GetComponentInChildren<TMP_Text>());
        }

        private void Update()
        {
            UpdateSliders();
        }

        private void UpdateSliders()
        {
            _rows.text.text = "Nr of rows:" + _rows.slider.value;
            _columns.text.text = "Nr of Columns: " + _columns.slider.value;
        }

        [Serializable]
        public struct SizeSlider
        {
            public Slider slider;
            public TMP_Text text;

            public SizeSlider(Slider s, TMP_Text t)
            {
                slider = s;
                text = t;
            }
        }
    }
}
