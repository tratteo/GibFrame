// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe.Editor -> %Namespace% : GibFrameSettings_W.cs
//
// All Rights Reserved

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

internal class GibFrameSettings_W : EditorWindow
{
    private GUILayoutOption[] defaultOptions;
    private GUIStyle sectionsStyle;

    [MenuItem("Edit/GibFrame settings", false, 1 << 8)]
    internal static void ShowWindow()
    {
        GetWindow(typeof(GibFrameSettings_W)).titleContent = new GUIContent("GibFrame settings");
    }

    internal static void AddDefineSymbols(IEnumerable<string> symbols)
    {
        string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        List<string> allDefines = definesString.Split(';').ToList();
        allDefines.AddRange(symbols.Except(allDefines));
        PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, string.Join(";", allDefines.ToArray()));
    }

    internal static void RemoveDefineSymbols(IEnumerable<string> symbols)
    {
        string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        List<string> allDefines = definesString.Split(';').ToList();
        allDefines.RemoveAll(s => symbols.Contains(s));
        PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, string.Join(";", allDefines.ToArray()));
    }

    private void OnEnable()
    {
        sectionsStyle = new GUIStyle
        {
            fontSize = 16
        };
        sectionsStyle.normal.textColor = Color.white;

        defaultOptions = new GUILayoutOption[] { GUILayout.Width(200) };
    }

    private void OnGUI()
    {
        GUILayout.Space(10);
        EditorGUILayout.LabelField(new GUIContent("Default scene loading"), sectionsStyle);
        GUILayout.Space(5);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Default scene path", "The path of the default scene, starting from the root folder.\nRequires to specify the extension .unity\nExample: Assets/Scenes/Initializer.unity"), defaultOptions);
        GibFrameEditorSettings.Data.defaultSceneName = EditorGUILayout.TextField(GibFrameEditorSettings.Data.defaultSceneName);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Load default scene on play", "Load the specified scene whenever the playmode is entered"), defaultOptions);
        GibFrameEditorSettings.Data.loadDefaultSceneOnPlay = EditorGUILayout.Toggle(GibFrameEditorSettings.Data.loadDefaultSceneOnPlay, defaultOptions);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Restore opened scenes", "Restore the opened scenes when the playmode is exited"), defaultOptions);
        GibFrameEditorSettings.Data.restoreOpenedScenes = EditorGUILayout.Toggle(GibFrameEditorSettings.Data.restoreOpenedScenes, defaultOptions);
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10);
        EditorGUILayout.LabelField(new GUIContent("Common Update"), sectionsStyle);
        GUILayout.Space(5);
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Enable Common Update", "Restore the opened scenes when the playmode is exited"), defaultOptions);
        GibFrameEditorSettings.Data.enableCommonUpdate = EditorGUILayout.Toggle(GibFrameEditorSettings.Data.enableCommonUpdate, defaultOptions);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Enable Common Fixed Update", "Restore the opened scenes when the playmode is exited"), defaultOptions);
        GibFrameEditorSettings.Data.enableCommonFixedUpdate = EditorGUILayout.Toggle(GibFrameEditorSettings.Data.enableCommonFixedUpdate, defaultOptions);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Enable Common Late Update", "Restore the opened scenes when the playmode is exited"), defaultOptions);
        GibFrameEditorSettings.Data.enableCommonLateUpdate = EditorGUILayout.Toggle(GibFrameEditorSettings.Data.enableCommonLateUpdate, defaultOptions);
        EditorGUILayout.EndHorizontal();
        if (EditorGUI.EndChangeCheck())
        {
            List<string> addSymbols = new List<string>();
            List<string> removeSymbols = new List<string>();
            EditorUtility.SetDirty(this);
            if (!GibFrameEditorSettings.Data.enableCommonUpdate)
            {
                addSymbols.Add("GIB_NO_COMMUPDATE");
            }
            else
            {
                removeSymbols.Add("GIB_NO_COMMUPDATE");
            }
            if (!GibFrameEditorSettings.Data.enableCommonFixedUpdate)
            {
                addSymbols.Add("GIB_NO_COMMFIXEDUPDATE");
            }
            else
            {
                removeSymbols.Add("GIB_NO_COMMFIXEDUPDATE");
            }
            if (!GibFrameEditorSettings.Data.enableCommonLateUpdate)
            {
                addSymbols.Add("GIB_NO_COMMLATEUPDATE");
            }
            else
            {
                removeSymbols.Add("GIB_NO_COMMLATEUPDATE");
            }
            if (addSymbols.Count > 0)
            {
                AddDefineSymbols(addSymbols);
            }
            if (removeSymbols.Count > 0)
            {
                RemoveDefineSymbols(removeSymbols);
            }
            GibFrameEditorSettings.SaveSettings();
        }
        //if (GUILayout.Button(new GUIContent("Clear")))
        //{
        //    TryClear();
        //}
        //GUILayout.Space(15);
        //EditorGUILayout.LabelField(new GUIContent("Waypoints"), sectionsStyle);
        //GUILayout.Space(5);
        //if (GUILayout.Button(new GUIContent("New", "Add a new waypoint")))
        //{
        //    TrySpawnWaypoint();
        //}
        //if (GUILayout.Button(new GUIContent("Delete", "Delete all the selected waypoints")))
        //{
        //    TryDeleteWaypoints();
        //}

        //GUILayout.Space(10);
        //EditorGUILayout.LabelField(new GUIContent("Start Points"), sectionsStyle);
        //GUILayout.Space(5);
        //sProb = EditorGUILayout.Slider("Pick probability", sProb, 0F, 1F);
        //if (GUILayout.Button(new GUIContent("New", "Add a new start waypoint")))
        //{
        //    TrySpawnStartPoint(sProb);
        //}
        //if (GUILayout.Button(new GUIContent("Delete", "Delete all the selected start points")))
        //{
        //    TryDeleteStartPoints();
        //}
        //GUILayout.Space(15);
        //EditorGUILayout.LabelField(new GUIContent("Pathing"), sectionsStyle);
        //GUILayout.Space(5);
        //from = EditorGUILayout.ObjectField("From", from, typeof(Waypoint), true) as Waypoint;
        //SerializedObject so = new SerializedObject(this);
        //SerializedProperty toProp = so.FindProperty("to");
        //EditorGUILayout.PropertyField(toProp, new GUIContent("To"), true, null);
        //so.ApplyModifiedProperties();
        //if (GUILayout.Button(new GUIContent("Link")))
        //{
        //    TryLinkWaypoints();
        //}
        //if (GUILayout.Button(new GUIContent("Unlink")))
        //{
        //    TryUnlinkWaypoints();
        //}
        //if (GUILayout.Button(new GUIContent("Clear")))
        //{
        //    from = null;
        //    to = new List<Waypoint> { null };
        //}

        //if (Selection.gameObjects.Length == 1 && path == null)
        //{
        //    Path p = Selection.gameObjects[0].GetComponent<Path>();
        //    if (p != null)
        //    {
        //        path = p;
        //    }
        //}
    }
}
