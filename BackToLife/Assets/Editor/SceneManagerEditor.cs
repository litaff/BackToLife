using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BackToLife
{
    [CustomEditor(typeof(SceneManager))]
    public class SceneManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField($"Loaded scene: {SceneManager.loadedScene}");
            base.OnInspectorGUI();
        }
    }
}
