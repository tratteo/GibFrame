// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe.Editor -> GibEditor : GibFrameSettingsEditorWindow.cs
//
// All Rights Reserved

using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GibFrame.Editor
{
    internal class GibFrameSettingsEditorWindow : EditorWindow
    {
        private GUILayoutOption[] defaultOptions;
        private GUIStyle sectionsStyle;
        private GibFrameEditorSettings settings;

        internal static void AddDefineSymbols(params string[] symbols)
        {
            var definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            var allDefines = definesString.Split(';').ToList();
            allDefines.AddRange(symbols.Except(allDefines));
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, string.Join(";", allDefines.ToArray()));
        }

        internal static void RemoveDefineSymbols(params string[] symbols)
        {
            var definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            var allDefines = definesString.Split(';').ToList();
            allDefines.RemoveAll(s => symbols.Contains(s));
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, string.Join(";", allDefines.ToArray()));
        }

        private void OnDisable()
        {
            GibFrameEditorSettingsManager.SaveSettings(settings);
        }

        private void OnEnable()
        {
            sectionsStyle = new GUIStyle
            {
                fontSize = 16
            };
            sectionsStyle.normal.textColor = Color.white;
            defaultOptions = new GUILayoutOption[] { GUILayout.Width(200) };
            settings = GibFrameEditorSettingsManager.LoadSettings();
        }

        private void OnGUI()
        {
            GUILayout.Space(10);
            EditorGUILayout.LabelField(new GUIContent("Default scene loading"), sectionsStyle);
            GUILayout.Space(5);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent("Default scene path", "The path of the default scene, starting from the root folder.\nRequires to specify the extension .unity\nExample: Assets/Scenes/Initializer.unity"), defaultOptions);
            settings.DefaultSceneName = EditorGUILayout.TextField(settings.DefaultSceneName);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent("Load default scene on play", "Load the specified scene whenever the playmode is entered"), defaultOptions);
            settings.LoadDefaultSceneOnPlay = EditorGUILayout.Toggle(settings.LoadDefaultSceneOnPlay, defaultOptions);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent("Restore opened scenes", "Restore the opened scenes when the playmode is exited"), defaultOptions);
            settings.RestoreOpenedScenes = EditorGUILayout.Toggle(settings.RestoreOpenedScenes, defaultOptions);
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(10);
        }
    }
}