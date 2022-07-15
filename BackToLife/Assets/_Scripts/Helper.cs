using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BackToLife
{
    public static class Helper
    {
        private static Camera _camera;

        public static Camera GetCamera()
        {
            if(!_camera)
                _camera = Camera.main;
            return _camera;
        }
        
        public static string ReverseString(string str) {  
            var chars = str.ToCharArray();  
            var result = new char[chars.Length];  
            for (int i = 0, j = str.Length - 1; i < str.Length; i++, j--) {  
                result[i] = chars[j];  
            }  
            return new string(result);  
        }
        
        private static PointerEventData _eventDataCurrentPosition;
        private static List<RaycastResult> _results;
        public static bool IsOverUI()
        {
            _eventDataCurrentPosition = new PointerEventData(EventSystem.current) {position = Input.mousePosition};
            _results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(_eventDataCurrentPosition,_results);
            return _results.Count > 0;
        }
    }
}