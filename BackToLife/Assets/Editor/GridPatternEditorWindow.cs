using System;
using UnityEditor;
using UnityEngine;

namespace BackToLife
{
    public class GridPatternEditorWindow : EditorWindow
    {
        private bool listState;
        int listSize;
        private bool[] elementState = new bool[144];
        private GridPattern target;
        private SerializedObject serializedObject;
        
        public static void Open(GridPattern gridPattern)
        {
            var window = GetWindow<GridPatternEditorWindow>("Grid Pattern Editor");
            window.serializedObject = new SerializedObject(gridPattern);
            window.target = gridPattern;
        }

        private void OnGUI()
        {
            if (serializedObject == null) return;
            
            target.nrOfRows = EditorGUILayout.IntSlider("Nr Of Rows", target.nrOfRows, 8, 16);
            target.nrOfColumns = EditorGUILayout.IntSlider("Nr Of Columns", target.nrOfColumns, 4, 9);

            // TODO: display grid with selection options
            
            DisplayCellList(serializedObject.FindProperty("cells"));   
        }
        
        private void DisplayCellList(SerializedProperty list)
        {
            
            EditorGUILayout.BeginHorizontal();
            listState = EditorGUILayout.Foldout(listState, "Cells",true);
            listSize = Mathf.Max(0, EditorGUILayout.IntSlider(listSize,2,target.nrOfColumns*target.nrOfRows));
            EditorGUILayout.EndHorizontal();
            while (listSize > target.cells.Count)
                target.cells.Add(null);
            while (listSize < target.cells.Count)
                target.cells.RemoveAt(target.cells.Count - 1);
            serializedObject.Update();
            if (!listState) return;
            for (var i = 0; i < target.cells.Count; i++) 
            {
                EditorGUI.indentLevel += 1;
                elementState[i] = EditorGUILayout.Foldout(elementState[i], $"Element {i}", true);
                if (!elementState[i])
                {
                    EditorGUI.indentLevel -= 1;
                    continue;
                }
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("gridPosition"));
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("entityType"));
                
                if(target.cells[i].entityType == Entity.EntityType.Block)
                    EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("blockType"));
                if(target.cells[i].entityType == Entity.EntityType.Tile)
                    EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("tileType"));
                EditorGUI.indentLevel -= 1;
            }


        }
    }
}