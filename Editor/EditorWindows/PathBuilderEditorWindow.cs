// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe.Editor -> GibEditor : PathBuilderEditorWindow.cs
//
// All Rights Reserved

using GibFrame.PathBuilder;
using Pury.Editor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GibFrame.Editor
{
    internal class PathBuilderEditorWindow : PuryWindow
    {
        private enum Context
        { Setup, Build }

        public List<Waypoint> to;
        private int startPointsIndex = 0;
        private int waypointsIndex = 0;
        private Path path;
        private Waypoint from;
        private float sProb = 1F;
        private GUIStyle sectionsStyle;
        private SerializedObject so;
        private GameObject waypointPrefab;
        private Context context;
        private PurySeparator separator;
        private PurySeparator horizontalSeparator;

        protected override void Layout(List<PurySidebar> sidebars)
        {
            to = new List<Waypoint>();
            from = null;
            sectionsStyle = new GUIStyle
            {
                fontSize = 16,
                margin = new RectOffset(0, 0, 15, 15)
            };
            sectionsStyle.normal.textColor = Color.white;
            so = new SerializedObject(this);
            separator = PurySeparator.Towards(Orientation.Vertical).Thickness(2).Margin(new RectOffset(10, 10, 0, 0)).Build();
            horizontalSeparator = PurySeparator.Towards(Orientation.Horizontal).Thickness(1.5F).Margin(new RectOffset(5, 5, 15, 15)).Build();

            waypointPrefab = GenerateWaypoint().gameObject;

            autoRepaintOnSceneChange = true;
            ContentOrientation = Orientation.Vertical;

            minSize = new Vector2(700, 512);
            maxSize = new Vector2(700, 512);

            sidebars.Add(PurySidebar.Factory().Top().Draw(s =>
            {
                var buttonSyle = new GUIStyle(GUI.skin.button);
                buttonSyle.margin = new RectOffset(25, 25, 5, 5);
                var barStyle = new GUIStyle(GUI.skin.window);
                barStyle.padding = new RectOffset(5, 5, 5, 5);
                HorizontalGroup(() =>
                {
                    Center(() =>
                    {
                        var content = EditorGUIUtility.TrTextContentWithIcon("Setup", path ? "d_winbtn_mac_max" : "d_winbtn_mac_close");
                        if (GUILayout.Toggle(context == Context.Setup, content, buttonSyle, GUILayout.Width(250)))
                        {
                            context = Context.Setup;
                        }
                        if (GUILayout.Toggle(context == Context.Build, "Build", buttonSyle, GUILayout.Width(250)))
                        {
                            context = Context.Build;
                        }
                    });
                }, barStyle, GUILayout.MaxHeight(30), GUILayout.ExpandWidth(true));
            }));
        }

        protected override void DrawContent()
        {
            so.Update();
            var groupStyle = new GUIStyle(GUIStyle.none)
            {
                margin = new RectOffset(10, 10, 10, 10),
                padding = new RectOffset(0, 0, 0, 0)
            };
            switch (context)
            {
                case Context.Setup:
                    VerticalGroup(() =>
                    {
                        HorizontalGroup(() =>
                        {
                            path = EditorGUILayout.ObjectField(path, typeof(Path), false) as Path;
                            EditorGUI.BeginDisabledGroup(!path);
                            if (GUILayout.Button(EditorGUIUtility.TrIconContent("TreeEditor.Trash"), GUILayout.Width(2 * EditorGUIUtility.singleLineHeight)))
                            {
                                if (path && EditorUtility.DisplayDialog($"Delete script", $"Are you sure you want to delete the path?\n", "Yes", "Cancel"))
                                {
                                    DestroyImmediate(path);
                                }
                            }
                            EditorGUI.EndDisabledGroup();
                        });
                        if (GUILayout.Button(new GUIContent("Attach", "Attach a Path component to the selected GameObject")))
                        {
                            TryAttachPathScript();
                        }
                        if (GUILayout.Button(new GUIContent("Clear")))
                        {
                            path = null;
                        }
                        horizontalSeparator.Draw();
                        GUILayout.Label(EditorGUIUtility.TrTextContentWithIcon(
                            $"To select an existing {nameof(Path)} script, select it in the hierarchy and hit the attach button", "console.infoicon"));
                    }, groupStyle);
                    break;

                case Context.Build:
                    VerticalGroup(() =>
                    {
                        HorizontalGroup(() =>
                        {
                            Center(() =>
                            {
                                VerticalGroup(() =>
                                {
                                    EditorGUILayout.LabelField(new GUIContent("Waypoints"), sectionsStyle);
                                    GravitateEnd(() =>
                                    {
                                        if (GUILayout.Button(new GUIContent("New", "Add a new waypoint")))
                                        {
                                            TrySpawnWaypoint();
                                        }
                                        if (GUILayout.Button(new GUIContent("Delete", "Delete all the selected waypoints")))
                                        {
                                            TryDeleteWaypoints();
                                        }
                                    });
                                }, GUILayout.MaxWidth(350));
                                separator.Draw();

                                VerticalGroup(() =>
                                {
                                    EditorGUILayout.LabelField(new GUIContent("Start Points"), sectionsStyle);

                                    GravitateEnd(() =>
                                    {
                                        sProb = EditorGUILayout.Slider("Pick probability", sProb, 0F, 1F);
                                        if (GUILayout.Button(new GUIContent("New", "Add a new start waypoint")))
                                        {
                                            TrySpawnStartPoint(sProb);
                                        }
                                        if (GUILayout.Button(new GUIContent("Delete", "Delete all the selected start points")))
                                        {
                                            TryDeleteStartPoints();
                                        }
                                    });
                                }, GUILayout.MaxWidth(350));
                            });
                        }, GUILayout.MaxHeight(80));
                        horizontalSeparator.Draw();
                        GUILayout.Space(5);
                        EditorGUILayout.LabelField(new GUIContent("Pathing"), sectionsStyle);
                        from = EditorGUILayout.ObjectField("From", from, typeof(Waypoint), true) as Waypoint;
                        var toProp = so.FindProperty("to");
                        EditorGUILayout.PropertyField(toProp, new GUIContent("To"), true, null);

                        if (GUILayout.Button(new GUIContent("Link")))
                        {
                            TryLinkWaypoints();
                        }
                        if (GUILayout.Button(new GUIContent("Unlink")))
                        {
                            TryUnlinkWaypoints();
                        }
                        if (GUILayout.Button(new GUIContent("Clear selection")))
                        {
                            from = null;
                            to = new List<Waypoint>();
                        }
                        horizontalSeparator.Draw();
                        if (GUILayout.Button(new GUIContent("Clear all")))
                        {
                            TryClear();
                        }
                    }, groupStyle);
                    break;

                default:
                    break;
            }
            so.ApplyModifiedProperties();
        }

        private void OnDisable()
        {
            if (waypointPrefab)
            {
                DestroyImmediate(waypointPrefab);
            }
        }

        #region Management

        private void TryClear()
        {
            if (path != null)
            {
                path.Clear();
            }
        }

        private void TryUnlinkWaypoints()
        {
            if (from != null)
            {
                foreach (var w in to)
                {
                    from.RemoveOption(w);
                }
            }
        }

        private void TryDeleteStartPoints()
        {
            foreach (var obj in Selection.gameObjects)
            {
                var w = obj.GetComponent<Waypoint>();
                if (path.RemoveStartPoint(w))
                {
                    DestroyImmediate(w.gameObject);
                }
            }
        }

        private void TryDeleteWaypoints()
        {
            foreach (var obj in Selection.gameObjects)
            {
                var w = obj.GetComponent<Waypoint>();
                if (path.RemoveWaypoint(w))
                {
                    DestroyImmediate(w.gameObject);
                }
            }
        }

        private void TryLinkWaypoints()
        {
            if (from != null)
            {
                foreach (var w in to)
                {
                    if (w != null)
                    {
                        from.AddOption(w);
                    }
                }
            }
        }

        private Waypoint GenerateWaypoint()
        {
            GameObject obj = new GameObject("waypoint_parent");
            obj.hideFlags = HideFlags.HideInHierarchy;
            return obj.AddComponent<Waypoint>();
        }

        private void TrySpawnWaypoint()
        {
            if (path == null)
            {
                Debug.LogWarning("Select or attach a path first!");
                return;
            }

            var obj = Instantiate(waypointPrefab, Vector3.zero, Quaternion.identity);
            obj.transform.SetParent(path.transform);
            var way = obj.GetComponent<Waypoint>();
            path.AddWaypoint(way);
            obj.name = "waypoint_" + waypointsIndex++.ToString();
            foreach (var o in Selection.gameObjects)
            {
                var point = o.GetComponent<Waypoint>();
                if (point != null)
                {
                    point.AddOption(way);
                }
            }
            if (Selection.gameObjects.Length == 1)
            {
                obj.transform.position = Selection.gameObjects[0].transform.position + Vector3.forward * 0.1F;
            }
            Selection.activeGameObject = obj;
        }

        private void TrySpawnStartPoint(float prob)
        {
            if (path == null)
            {
                Debug.LogWarning("Select or attach a path first!");
                return;
            }
            var obj = Instantiate(waypointPrefab, Vector3.zero, Quaternion.identity);
            obj.transform.SetParent(path.transform);
            obj.name = "startpoint_" + startPointsIndex++.ToString();
            var way = obj.GetComponent<Waypoint>();
            path.AddStartPoint(way, prob);
            Selection.activeGameObject = obj;
        }

        private void TryAttachPathScript()
        {
            if (Selection.gameObjects.Length > 1)
            {
                Debug.LogError("Select only one game objects to attach the Path to!");
            }
            else if (Selection.gameObjects.Length == 1)
            {
                if (!Selection.gameObjects[0].TryGetComponent<Path>(out var attachedpath))
                {
                    attachedpath = Selection.gameObjects[0].AddComponent<Path>();
                }
                if (!path)
                {
                    path = attachedpath;
                }
                else
                {
                    Debug.Log("Path script already attached to the selected GameObject");
                }
            }
        }

        #endregion Management
    }
}