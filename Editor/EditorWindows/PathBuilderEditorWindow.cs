// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe.Editor -> GibEditor : PathBuilderEditorWindow.cs
//
// All Rights Reserved

using System.Collections.Generic;
using GibFrame.PathBuilder;
using UnityEditor;
using UnityEngine;

namespace GibEditor
{
    internal class PathBuilderEditorWindow : EditorWindow
    {
        public List<Waypoint> to;
        private int startPointsIndex = 0;
        private int waypointsIndex = 0;
        private GameObject waypoint;
        private Path path;
        private Waypoint from;
        private float sProb = 1F;
        private GUIStyle sectionsStyle;

        private void OnEnable()
        {
            waypoint = Resources.Load("GibFrame/PathBuilder/Waypoint_P") as GameObject;
            sectionsStyle = new GUIStyle();
            to = new List<Waypoint> { null };
            from = null;
            sectionsStyle.fontSize = 18;
            sectionsStyle.normal.textColor = Color.white;
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField(new GUIContent("Setup"), sectionsStyle);
            GUILayout.Space(5);
            path = EditorGUILayout.ObjectField(path, typeof(Path), false) as Path;

            if (GUILayout.Button(new GUIContent("Attach", "Attach a Path component to the selected GameObject")))
            {
                TryAttachPathScript();
            }

            if (GUILayout.Button(new GUIContent("Clear")))
            {
                TryClear();
            }
            GUILayout.Space(15);
            EditorGUILayout.LabelField(new GUIContent("Waypoints"), sectionsStyle);
            GUILayout.Space(5);
            if (GUILayout.Button(new GUIContent("New", "Add a new waypoint")))
            {
                TrySpawnWaypoint();
            }
            if (GUILayout.Button(new GUIContent("Delete", "Delete all the selected waypoints")))
            {
                TryDeleteWaypoints();
            }

            GUILayout.Space(10);
            EditorGUILayout.LabelField(new GUIContent("Start Points"), sectionsStyle);
            GUILayout.Space(5);
            sProb = EditorGUILayout.Slider("Pick probability", sProb, 0F, 1F);
            if (GUILayout.Button(new GUIContent("New", "Add a new start waypoint")))
            {
                TrySpawnStartPoint(sProb);
            }
            if (GUILayout.Button(new GUIContent("Delete", "Delete all the selected start points")))
            {
                TryDeleteStartPoints();
            }
            GUILayout.Space(15);
            EditorGUILayout.LabelField(new GUIContent("Pathing"), sectionsStyle);
            GUILayout.Space(5);
            from = EditorGUILayout.ObjectField("From", from, typeof(Waypoint), true) as Waypoint;
            SerializedObject so = new SerializedObject(this);
            SerializedProperty toProp = so.FindProperty("to");
            EditorGUILayout.PropertyField(toProp, new GUIContent("To"), true, null);
            so.ApplyModifiedProperties();
            if (GUILayout.Button(new GUIContent("Link")))
            {
                TryLinkWaypoints();
            }
            if (GUILayout.Button(new GUIContent("Unlink")))
            {
                TryUnlinkWaypoints();
            }
            if (GUILayout.Button(new GUIContent("Clear")))
            {
                from = null;
                to = new List<Waypoint> { null };
            }

            if (Selection.gameObjects.Length == 1 && path == null)
            {
                Path p = Selection.gameObjects[0].GetComponent<Path>();
                if (p != null)
                {
                    path = p;
                }
            }
        }

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
                foreach (Waypoint w in to)
                {
                    from.RemoveOption(w);
                }
            }
        }

        private void TryDeleteStartPoints()
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                Waypoint w = obj.GetComponent<Waypoint>();
                if (path.RemoveStartPoint(w))
                {
                    DestroyImmediate(w.gameObject);
                }
            }
        }

        private void TryDeleteWaypoints()
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                Waypoint w = obj.GetComponent<Waypoint>();
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
                foreach (Waypoint w in to)
                {
                    if (w != null)
                    {
                        from.AddOption(w);
                    }
                }
            }
        }

        private void TrySpawnWaypoint()
        {
            if (path == null)
            {
                Debug.LogWarning("Select or attach a path first!");
                return;
            }
            GameObject obj = Instantiate(waypoint, Vector3.zero, Quaternion.identity);
            obj.transform.SetParent(path.transform);
            Waypoint way = obj.GetComponent<Waypoint>();
            path.AddWaypoint(way);
            obj.name = "Waypoint_" + (waypointsIndex++).ToString();
            foreach (GameObject o in Selection.gameObjects)
            {
                Waypoint point = o.GetComponent<Waypoint>();
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
            GameObject obj = Instantiate(waypoint, Vector3.zero, Quaternion.identity);
            obj.transform.SetParent(path.transform);
            obj.name = "Startpoint_" + (startPointsIndex++).ToString();
            Waypoint way = obj.GetComponent<Waypoint>();
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
                if (Selection.gameObjects[0].GetComponent<Path>() == null)
                {
                    path = Selection.gameObjects[0].AddComponent<Path>();
                }
                else
                {
                    Debug.Log("Path script already attached to the selected GameObject");
                }
            }
        }
    }
}
