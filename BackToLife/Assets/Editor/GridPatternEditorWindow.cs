using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BackToLife
{
    public class GridPatternEditorWindow : EditorWindow
    {
        private GridPattern _target;
        private SerializedObject _serializedObject;
        public Rect windowRect = new Rect(100, 100, 200, 200);
        private bool _windowOpen = false;
        
        public static void Open(GridPattern gridPattern)
        {
            var window = GetWindow<GridPatternEditorWindow>($"Grid Pattern Editor {gridPattern.name}");
            window._target = gridPattern;
            window._serializedObject = new SerializedObject(gridPattern);
        }

        private void OnGUI()
        {
            BeginWindows();
            _target.nrOfRows = EditorGUILayout.IntSlider("Nr Of Rows", _target.nrOfRows, 8, 16);
            _target.nrOfColumns = EditorGUILayout.IntSlider("Nr Of Columns", _target.nrOfColumns, 4, 9);
            // TODO: display grid with selection options
            for (var i = _target.nrOfRows-1; i >= 0; i--)
            {
                EditorGUILayout.BeginHorizontal();
                for (var j = 0; j < _target.nrOfColumns; j++)
                {
                    // TODO: make the GUILayout.Window work or go back to new EditorWindow
                    var cell = GetPatternCellFromPosition(j, i);
                    if(!(cell is null))
                    {
                        if (!GUILayout.Button(_target.regularBlock, GUILayout.Width(30), GUILayout.Height(30)))
                            continue;
                        _windowOpen = true;
                        //PatternCellEditorWindow.TryOpen(cell, _serializedObject);
                    }
                    else
                    {
                        if (!GUILayout.Button("null", GUILayout.Width(30), GUILayout.Height(30))) 
                            continue;
                        _windowOpen = true;
                        //PatternCellEditorWindow.TryOpen(cell, _serializedObject);
                    }
                    if (_windowOpen)
                        windowRect = GUILayout.Window(((i+j)*(i+j+1)+j)/2, windowRect, DoWindow, "hi there");
                    // ----------------------------------^ for unique id from two natural numbers
                }
                EditorGUILayout.EndHorizontal();
            }
            EndWindows();
        }

        private void Old()
        {
            _target.nrOfRows = EditorGUILayout.IntSlider("Nr Of Rows", _target.nrOfRows, 8, 16);
            _target.nrOfColumns = EditorGUILayout.IntSlider("Nr Of Columns", _target.nrOfColumns, 4, 9);

            // TODO: display grid with selection options
            for (var i = _target.nrOfRows-1; i >= 0; i--)
            {
                EditorGUILayout.BeginHorizontal();
                for (var j = 0; j < _target.nrOfColumns; j++)
                {
                    BeginWindows();
                    var cell = GetPatternCellFromPosition(j, i);
                    if(!(cell is null))
                    {
                        if (!GUILayout.Button(_target.regularBlock, GUILayout.Width(30), GUILayout.Height(30)))
                            continue;
                        PatternCellEditorWindow.TryOpen(cell, _serializedObject);
                    }
                    else
                    {
                        if (!GUILayout.Button("null", GUILayout.Width(30), GUILayout.Height(30))) 
                            continue;
                        PatternCellEditorWindow.TryOpen(cell, _serializedObject);
                    }
                    EndWindows();
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        
        private void DoWindow(int unusedWindowID)
        {
            if (GUILayout.Button("Hi"))
                _windowOpen = false;
            GUI.DragWindow();
        }
        
        private GridPattern.PatternCell GetPatternCellFromPosition(int x, int y)
        {
            return _target.cells.FirstOrDefault(cell => cell.gridPosition == new Vector2(x, y));
        }

        public class PatternCellEditorWindow : EditorWindow
        {
            private GridPattern.PatternCell _target;
            private SerializedObject _serializedObject;
            private static PatternCellEditorWindow _window;

            public static EditorWindow TryOpen(GridPattern.PatternCell patternCell, SerializedObject serializedObject)
            {
                if(!IsOpen)
                    return Open(patternCell, serializedObject);
                _window.Close();
                return Open(patternCell, serializedObject);
            }

            private void OnGUI()
            {
                EditorGUILayout.PropertyField(_serializedObject.FindProperty("gridPosition"));
                EditorGUILayout.PropertyField(_serializedObject.FindProperty("entityType"));
                switch (_target.entityType)
                {
                    case Entity.EntityType.Player:
                        break;
                    case Entity.EntityType.Block:
                        EditorGUILayout.PropertyField(_serializedObject.FindProperty("blockType"));
                        break;
                    case Entity.EntityType.Tile:
                        EditorGUILayout.PropertyField(_serializedObject.FindProperty("tileType"));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            private static bool IsOpen => _window != null;

            private static EditorWindow Open(GridPattern.PatternCell patternCell, SerializedObject serializedObject)
            {
                _window = GetWindow<PatternCellEditorWindow>($"Pattern Cell Editor: {GetName(patternCell)}");
                _window._target = patternCell;
                _window._serializedObject = serializedObject;
                return _window;
            }

            private static string GetName(GridPattern.PatternCell patternCell)
            {
                if (patternCell is null)
                    return "null";
                return patternCell.entityType switch
                {
                    Entity.EntityType.Player => "Player",
                    Entity.EntityType.Block => patternCell.blockType switch
                    {
                        Block.BlockType.None => "None",
                        Block.BlockType.Regular => "Regular",
                        Block.BlockType.Slime => "Slime",
                        Block.BlockType.Slippery => "Slippery",
                        _ => throw new ArgumentOutOfRangeException()
                    },
                    Entity.EntityType.Tile => patternCell.tileType switch
                    {
                        Tile.TileType.None => "None",
                        Tile.TileType.EndTile => "End Tile",
                        Tile.TileType.TeleportTile => "Teleport Tile",
                        _ => throw new ArgumentOutOfRangeException()
                    },
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }
    }
}