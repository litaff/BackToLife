using UnityEditor;
using UnityEngine;

namespace BackToLife
{
    [CustomEditor(typeof(GridPattern))]
    public class GridPatternEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Editor"))
            {
                GridPatternEditorWindow.Open((GridPattern)target);
            }
            base.OnInspectorGUI();
        }
    }
}
