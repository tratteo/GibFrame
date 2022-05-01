// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe.Editor -> GibEditor : GibFrameSettingsEditorWindow.cs
//
// All Rights Reserved

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GibEditor
{
    internal class GibFrameSettingsEditorWindow : EditorWindow
    {
        private GUILayoutOption[] defaultOptions;
        private GUIStyle sectionsStyle;

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
                var addSymbols = new List<string>();
                var removeSymbols = new List<string>();
                if (!GibFrameEditorSettings.Data.enableCommonUpdate)
                {
                    addSymbols.Add(GibFrameEditorSettings.DISABLE_COMMUPDATE);
                }
                else
                {
                    removeSymbols.Add(GibFrameEditorSettings.DISABLE_COMMUPDATE);
                }
                if (!GibFrameEditorSettings.Data.enableCommonFixedUpdate)
                {
                    addSymbols.Add(GibFrameEditorSettings.DISABLE_COMMFIXEDUPDATE);
                }
                else
                {
                    removeSymbols.Add(GibFrameEditorSettings.DISABLE_COMMFIXEDUPDATE);
                }
                if (!GibFrameEditorSettings.Data.enableCommonLateUpdate)
                {
                    addSymbols.Add(GibFrameEditorSettings.DISABLE_COMMLATEUPDATE);
                }
                else
                {
                    removeSymbols.Add(GibFrameEditorSettings.DISABLE_COMMLATEUPDATE);
                }
                if (addSymbols.Count > 0)
                {
                    AddDefineSymbols(addSymbols.ToArray());
                }
                if (removeSymbols.Count > 0)
                {
                    RemoveDefineSymbols(removeSymbols.ToArray());
                }
                GibFrameEditorSettings.SaveSettings();
            }
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent("Auto updater instantiate", "Automatically instantiate the CommonUpdateManager when needed in runtime, if not present in scene"), defaultOptions);
            GibFrameEditorSettings.Data.runtimeCommonUpdateInstantiate = EditorGUILayout.Toggle(GibFrameEditorSettings.Data.runtimeCommonUpdateInstantiate, defaultOptions);
            EditorGUILayout.EndHorizontal();
            if (EditorGUI.EndChangeCheck())
            {
                if (!GibFrameEditorSettings.Data.runtimeCommonUpdateInstantiate)
                {
                    AddDefineSymbols(GibFrameEditorSettings.DISABLE_RUNTIME_UPDATER_INSTANTIATE);
                }
                else
                {
                    RemoveDefineSymbols(GibFrameEditorSettings.DISABLE_RUNTIME_UPDATER_INSTANTIATE);
                }
                GibFrameEditorSettings.SaveSettings();
            }
        }
    }
}
