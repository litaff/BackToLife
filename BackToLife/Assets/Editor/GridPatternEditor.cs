using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.TerrainAPI;
using UnityEngine;
using UnityEngine.UIElements;

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
