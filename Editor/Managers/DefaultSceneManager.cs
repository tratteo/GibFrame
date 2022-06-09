// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe.Editor -> GibEditor : DefaultSceneManager.cs
//
// All Rights Reserved

using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace GibFrame.Editor
{
    public static class DefaultSceneManager
    {
        internal const string UserScene = "u_scene";
        internal const string RequestedBool = "d_requested";
        private static bool isLoadingDefaultScene = false;

        public static void PlayDefaultScene()
        {
            if (IsDefaultSceneValid())
            {
                isLoadingDefaultScene = true;
                EditorPrefs.SetString(UserScene, UnityEngine.SceneManagement.SceneManager.GetActiveScene().path);
                EditorPrefs.SetBool(RequestedBool, true);
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                EditorSceneManager.OpenScene(GibFrameEditorSettingsManager.LoadSettings().DefaultSceneName);
                EditorApplication.EnterPlaymode();
            }
            else
            {
                Debug.LogWarning("Unable to load the default scene, check in GibFrame settings that the default scene path is correctly set");
            }
        }

        public static void LoadDefaultScene()
        {
            if (IsDefaultSceneValid())
            {
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                EditorSceneManager.OpenScene(GibFrameEditorSettingsManager.LoadSettings().DefaultSceneName);
            }
            else
            {
                Debug.LogWarning("Unable to load the default scene, check in GibFrame settings that the default scene path is correctly set");
            }
        }

        private static void PlaymodeChange(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.EnteredEditMode:
                    if (GibFrameEditorSettingsManager.LoadSettings().RestoreOpenedScenes && EditorPrefs.GetBool(RequestedBool))
                    {
                        var key = EditorPrefs.GetString(UserScene);
                        if (File.Exists(key))
                        {
                            EditorSceneManager.OpenScene(key);
                        }
                    }
                    EditorPrefs.SetBool(RequestedBool, false);
                    break;

                case PlayModeStateChange.EnteredPlayMode:

                    break;

                case PlayModeStateChange.ExitingEditMode:
                    if (GibFrameEditorSettingsManager.LoadSettings().LoadDefaultSceneOnPlay && !isLoadingDefaultScene)
                    {
                        PlayDefaultScene();
                    }
                    break;

                case PlayModeStateChange.ExitingPlayMode:
                    isLoadingDefaultScene = false;
                    break;
            }
        }

        private static bool IsDefaultSceneValid()
        {
            var data = GibFrameEditorSettingsManager.LoadSettings();
            return !string.IsNullOrWhiteSpace(data.DefaultSceneName) && File.Exists(data.DefaultSceneName);
        }

        [InitializeOnLoad]
        public class Startup
        {
            static Startup()
            {
                EditorApplication.playModeStateChanged += PlaymodeChange;
            }
        }
    }
}