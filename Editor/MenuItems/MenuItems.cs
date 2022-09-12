// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe.Editor -> GibEditor : MenuItems.cs
//
// All Rights Reserved

using UnityEditor;
using UnityEngine;

namespace GibFrame.Editor
{
    internal class MenuItems
    {
        //[MenuItem("GibFrame/Tests/1", false, 0)]
        //internal static void Test1()
        //{
        //}

        [MenuItem("Window/GibFrame/Path Builder", false, 22)]
        internal static void ShowPathBuilderWindow() => EditorWindow.GetWindow(typeof(PathBuilderEditorWindow)).titleContent = new GUIContent("Path Builder");

        [MenuItem("Window/GibFrame/Settings", false, 1 << 10)]
        internal static void ShowSettingsWindow()
        {
            EditorWindow.GetWindow(typeof(GibFrameSettingsEditorWindow)).titleContent = new GUIContent("GibFrame settings");
            GibFrameEditorSettingsManager.LoadSettings();
        }

        [MenuItem("Window/GibFrame/Play default scene %&LEFT", false, 64)]
        internal static void PlayDefaultScene() => DefaultSceneManager.PlayDefaultScene();

        [MenuItem("Window/GibFrame/Load default scene %l", false, 64)]
        internal static void LoadDefaultScene() => DefaultSceneManager.LoadDefaultScene();
    }
}