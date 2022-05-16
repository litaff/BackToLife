using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace BackToLife
{
    public class TouchController
    {
        private Vector2 _startTouchPos = Vector2.zero;
        private Vector2 _touchPos = Vector2.zero;
        private Vector2 _endTouchPos = Vector2.zero;

        /// <summary>
        /// Discard return value if == Vector2.zero
        /// </summary>
        public Vector2 GetSwipeDirection()
        {
            if (Input.touchCount != 1)
                return Vector2.zero;

            var touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _startTouchPos = touch.position;
                    break;
                case TouchPhase.Moved:
                    _touchPos = touch.position;
                    break;
                case TouchPhase.Ended:
                {
                    _endTouchPos = touch.position;
                    var dir = (_endTouchPos - _startTouchPos).normalized;
                    return Mathf.Abs(dir.x) > Mathf.Abs(dir.y)
                        ? new Vector2(Mathf.Sign(dir.x), 0)
                        : new Vector2(0, Mathf.Sign(dir.y));
                }
                case TouchPhase.Stationary:
                    break;
                case TouchPhase.Canceled:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return Vector2.zero;
        }
    }
}
