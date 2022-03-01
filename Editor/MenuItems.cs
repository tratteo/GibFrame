﻿// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe.Editor -> GibEditor : MenuItems.cs
//
// All Rights Reserved

using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace GibEditor
{
    internal class MenuItems
    {
        private static AddRequest packageManagerRequest;

        [MenuItem("GibFrame/Path Builder", false, 22)]
        internal static void ShowPathBuilderWindow()
        {
            EditorWindow.GetWindow(typeof(PathBuilderEditorWindow)).titleContent = new GUIContent("Path Builder");
        }

        [MenuItem("GibFrame/Guarded/Verify Scene", false, 0)]
        internal static void VerifySceneGuardedObjects()
        {
            GuardedManager.VerifyGuardedObjects(true, false);
        }

        [MenuItem("GibFrame/Guarded/Verify Prefabs", false, 0)]
        internal static void VerifyPrefabsGuardedObjects()
        {
            GuardedManager.VerifyGuardedObjects(false, true);
        }

        [MenuItem("GibFrame/Guarded/Verify %&v", false, 0)]
        internal static void VerifyGuardedObjects()
        {
            GuardedManager.VerifyGuardedObjects(true, true);
        }

        [MenuItem("GibFrame/Settings", false, 1 << 10)]
        internal static void ShowSettingsWindow()
        {
            EditorWindow.GetWindow(typeof(GibFrameSettingsEditorWindow)).titleContent = new GUIContent("GibFrame settings");
            GibFrameEditorSettings.LoadSettings();
        }

        [MenuItem("GibFrame/Play default scene %&LEFT", false, 0)]
        internal static void PlayDefaultScene()
        {
            DefaultSceneManager.PlayDefaultScene();
        }

        [MenuItem("GibFrame/Load default scene %l", false, 0)]
        internal static void LoadDefaultScene()
        {
            DefaultSceneManager.LoadDefaultScene();
        }

        [MenuItem("GibFrame/Check for updates")]
        internal static void CheckForUpdates()
        {
            packageManagerRequest = Client.Add("https://github.com/tratteo/GibFrame.git");
            EditorUtility.DisplayProgressBar("Update", "Checking for updates...", 0.75F);
            EditorApplication.update += UpdateRequestHandler;
        }

        private static void UpdateRequestHandler()
        {
            if (packageManagerRequest.IsCompleted)
            {
                EditorUtility.ClearProgressBar();
                if (packageManagerRequest.Status == StatusCode.Success)
                    EditorUtility.DisplayDialog("Update", "GibFrame updated successfully", "Ok");
                else if (packageManagerRequest.Status >= StatusCode.Failure)
                    EditorUtility.DisplayDialog("Update", $"Unable to update GibFrame, error: {packageManagerRequest.Error.message}", "Ok");

                EditorApplication.update -= UpdateRequestHandler;
            }
        }
    }
}
