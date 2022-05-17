using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = Unity.Mathematics.Random;

namespace BackToLife
{
    [Serializable]
    public class InputManager
    {
        private Vector2 _startTouchPos = Vector2.zero;
        private Vector2 _touchPos = Vector2.zero;
        private Vector2 _endTouchPos = Vector2.zero;
        private bool _swiping = false;

        /// <summary>
        /// Discard return value if == Vector2.zero
        /// </summary>
        public Vector2 GetSwipeDirection()
        {
            if (Input.touchCount != 1)
                return Vector2.zero;
            if (Input.touchCount == 0)
            {
                ResetTouch();
                return Vector2.zero;
            }
            /*if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Clicked on the UI");
                ResetTouch();
                return Vector2.zero;
            }*/
            
            if(IsOverUI())
            {
                Debug.Log("Clicked on the UI");
                ResetTouch();
                return Vector2.zero;
            }
            
            var touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _swiping = true;
                    _startTouchPos = touch.position;
                    break;
                case TouchPhase.Moved:
                    _touchPos = touch.position;
                    break;
                case TouchPhase.Ended:
                {
                    if (!_swiping) return Vector2.zero;
                    _endTouchPos = touch.position;
                    var dir = (_endTouchPos - _startTouchPos).normalized;
                    _swiping = false;
                    if (_startTouchPos == _endTouchPos)
                        return Vector2.zero;
                    return Mathf.Abs(dir.x) > Mathf.Abs(dir.y)
                        ? new Vector2(Mathf.Sign(dir.x), 0)
                        : new Vector2(0, Mathf.Sign(dir.y));
                }
                case TouchPhase.Stationary:
                    break;
                case TouchPhase.Canceled:
                    ResetTouch();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return Vector2.zero;
        }


        private PointerEventData _eventDataCurrentPosition;
        private List<RaycastResult> _results;
        private bool IsOverUI()
        {
            _eventDataCurrentPosition = new PointerEventData(EventSystem.current) {position = Input.mousePosition};
            _results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(_eventDataCurrentPosition,_results);
            return _results.Count > 0;
        }
        private void ResetTouch()
        {
            _swiping = false;
            _startTouchPos = Vector2.zero;
            _touchPos = Vector2.zero;
            _endTouchPos = Vector2.zero;
        }
    }
}
