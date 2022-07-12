using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace BackToLife
{
    public class GridPatternEditorWindow : EditorWindow
    {
        private GridPattern _target;

        #region Cell window

        private GridPattern.PatternCell _currentCell;
        private Rect _windowRect = new Rect(100, 100, 250, 150);
        private bool _windowOpen;
        private string _currentWindowID; // first digit is x, second digit is y || format => 9x9y

        #endregion

        #region Textures

        [HideInInspector] public Texture2D player;
        [HideInInspector] public Texture2D regularBlock;
        [HideInInspector] public Texture2D slipperyBlock;
        [HideInInspector] public Texture2D slimeBlock;
        [HideInInspector] public Texture2D teleportTile;
        [HideInInspector] public Texture2D endTile;

        #endregion

        public static void Open(GridPattern gridPattern)
        {
            var window = GetWindow<GridPatternEditorWindow>($"Grid Pattern Editor {gridPattern.name}");
            window._target = gridPattern;
            
            window.player = 
                (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Sprites/Characters/soul_v2.png", 
                    typeof(Texture2D));
            window.regularBlock = 
                (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Sprites/Blocks/regular_block.png", 
                    typeof(Texture2D));
            window.slipperyBlock =
                (Texture2D) AssetDatabase.LoadAssetAtPath("Assets/Sprites/Blocks/slippery_block.png",
                    typeof(Texture2D));
            window.slimeBlock = 
                (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Sprites/Blocks/slime_block.png", 
                typeof(Texture2D));
            window.teleportTile = 
                (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Sprites/Tiles/teleport_tile.png", 
                typeof(Texture2D));
            window.endTile = 
                (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Sprites/Tiles/end_tile.png", 
                typeof(Texture2D));
        }

        private void OnGUI()
        {
            PatternErrors();
            
            _target.nrOfRows = EditorGUILayout.IntSlider("Nr Of Rows", _target.nrOfRows, 8, 16);
            _target.nrOfColumns = EditorGUILayout.IntSlider("Nr Of Columns", _target.nrOfColumns, 4, 9);
            
            for (var i = _target.nrOfRows-1; i >= 0; i--)
            {
                EditorGUILayout.BeginHorizontal();
                for (var j = 0; j < _target.nrOfColumns; j++)
                {
                    var cell = GetPatternCellFromPosition(j, i);
                    if(!(cell is null))
                    {
                        if (!GUILayout.Button(
                                GetTexture(cell.entityType, cell.blockType, cell.tileType),
                                GUILayout.Width(30), GUILayout.Height(30)))
                            continue;
                        if (_windowOpen) continue;
                        _windowOpen = true;
                        _currentCell = cell;
                    }
                    else
                    {
                        if (!GUILayout.Button("null", GUILayout.Width(30), GUILayout.Height(30))) 
                            continue;
                        if (_windowOpen) continue;
                        _windowOpen = true;
                        _currentCell = null;
                    }
                    _currentWindowID = 9.ToString() + j + 9 + i;
                }
                EditorGUILayout.EndHorizontal();
            }
            BeginWindows();
            if (_windowOpen)
                _windowRect = GUILayout.Window(int.Parse(_currentWindowID), _windowRect, DoWindow, "Cell editor");
            EndWindows();
        }
        
        private void DoWindow(int windowID)
        {
            var id = windowID.ToString();
            var cordX = "";
            var cordY = "";
            var y = true;
            
            for (var i = id.Length - 1; i > 0; i--)
            {
                if (y)
                {
                    cordY += id[i].ToString();
                    if (int.Parse(id[i - 1].ToString()) != 9) continue;
                    y = false;
                    i--;
                }
                else
                {
                    cordX += id[i].ToString();
                    if (int.Parse(id[i - 1].ToString()) == 9)
                        break;
                }
            }

            cordX = Helper.ReverseString(cordX);
            cordY = Helper.ReverseString(cordY);

            _currentCell ??= new GridPattern.PatternCell
            {
                gridPosition = new Vector2(int.Parse(cordX), int.Parse(cordY))
            };

            // readonly position
            GUI.enabled = false;
            EditorGUILayout.Vector2Field("Grid Position", _currentCell.gridPosition);
            GUI.enabled = true;
            
            _currentCell.entityType =
                (Entity.EntityType) EditorGUILayout.EnumPopup("Entity type", _currentCell.entityType);
            switch (_currentCell.entityType)
            {
                case Entity.EntityType.Player:
                    break;
                case Entity.EntityType.Block:
                    _currentCell.blockType =
                        (Block.BlockType) EditorGUILayout.EnumPopup("Block type", _currentCell.blockType);
                    break;
                case Entity.EntityType.Tile:
                    _currentCell.tileType = 
                        (Tile.TileType) EditorGUILayout.EnumPopup("Tile type", _currentCell.tileType);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button($"Confirm"))
            {
                _windowOpen = false;
                _target.cells.Add(_currentCell);
            }
            if (GUILayout.Button("Delete"))
            {
                _windowOpen = false;
                _target.cells.Remove(_target.cells.Find(x => x.Equals(_currentCell)));
                _currentCell = null;
            }
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Cancel"))
            {
                _windowOpen = false;
                _currentCell = null;
            }
            GUI.DragWindow();
        }

        private void PatternErrors()
        {
            if(!_target.CheckPlayer())
                EditorGUILayout.HelpBox("The player count is not correct! (expected 1)", MessageType.Error);
            if(!_target.CheckForEndTile())
                EditorGUILayout.HelpBox("The end tile count is not correct! (expected 1)", MessageType.Error);
            if(!_target.CheckForTeleportTile())
                EditorGUILayout.HelpBox("The number of teleport tiles is odd! (expected even)", MessageType.Error);
        }
        
        private Texture2D GetTexture(Entity.EntityType entityType, Block.BlockType blockType, Tile.TileType tileType)
        {
            switch (entityType)
            {
                case Entity.EntityType.Player:
                    return player;
                case Entity.EntityType.Block:
                    switch (blockType)
                    {
                        case Block.BlockType.None:
                            break;
                        case Block.BlockType.Regular:
                            return regularBlock;
                        case Block.BlockType.Slime:
                            return slimeBlock;
                        case Block.BlockType.Slippery:
                            return slipperyBlock;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(blockType), blockType, null);
                    }
                    break;
                case Entity.EntityType.Tile:
                    switch (tileType)
                    {
                        case Tile.TileType.None:
                            break;
                        case Tile.TileType.EndTile:
                            return endTile;
                        case Tile.TileType.TeleportTile:
                            return teleportTile;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(tileType), tileType, null);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(entityType), entityType, null);
            }

            return null;
        }

        private GridPattern.PatternCell GetPatternCellFromPosition(int x, int y)
        {
            return _target.cells.FirstOrDefault(cell => cell.gridPosition == new Vector2(x, y));
        }
    }
}