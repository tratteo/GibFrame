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
    internal static class DefaultSceneManager
    {
        internal const string USER_SCENE = "u_scene";
        internal const string REQUESTED_BOOL = "d_requested";
        private static bool isLoadingDefaultScene = false;

        internal static void PlayDefaultScene()
        {
            if (IsDefaultSceneValid())
            {
                isLoadingDefaultScene = true;
                EditorPrefs.SetString(USER_SCENE, UnityEngine.SceneManagement.SceneManager.GetSceneAt(0).path);
                EditorPrefs.SetBool(REQUESTED_BOOL, true);
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                EditorSceneManager.OpenScene(GibFrameEditorSettings.Data.defaultSceneName);
                EditorApplication.EnterPlaymode();
            }
            else
            {
                Debug.LogWarning("Unable to load the default scene, check in GibFrame settings that the default scene path is correctly set");
            }
        }

        internal static void LoadDefaultScene()
        {
            if (IsDefaultSceneValid())
            {
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                EditorSceneManager.OpenScene(GibFrameEditorSettings.Data.defaultSceneName);
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
                    if (GibFrameEditorSettings.Data.restoreOpenedScenes && EditorPrefs.GetBool(REQUESTED_BOOL))
                    {
                        var key = EditorPrefs.GetString(USER_SCENE);
                        if (File.Exists(key))
                        {
                            EditorSceneManager.OpenScene(key);
                        }
                    }
                    EditorPrefs.SetBool(REQUESTED_BOOL, false);
                    break;

                case PlayModeStateChange.EnteredPlayMode:

                    break;

                case PlayModeStateChange.ExitingEditMode:
                    if (GibFrameEditorSettings.Data.loadDefaultSceneOnPlay && !isLoadingDefaultScene)
                    {
                        PlayDefaultScene();
                    }
                    break;

                case PlayModeStateChange.ExitingPlayMode:
                    isLoadingDefaultScene = false;
                    break;
            }
        }

        private static bool IsDefaultSceneValid() => GibFrameEditorSettings.Data.defaultSceneName != string.Empty && File.Exists(GibFrameEditorSettings.Data.defaultSceneName);

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