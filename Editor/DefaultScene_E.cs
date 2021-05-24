// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe.Editor -> %Namespace% : DefaultScene_E.cs
//
// All Rights Reserved

using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

internal static class DefaultScene_E
{
    internal const string KEY = "c_scene_";
    private static bool isLoadingDefaultScene = false;

    [MenuItem("GibFrame/Play default scene %l")]
    internal static void LoadDefaultScene()
    {
        if (IsDefaultSceneValid())
        {
            isLoadingDefaultScene = true;
            EditorPrefs.SetString(KEY, UnityEngine.SceneManagement.SceneManager.GetSceneAt(0).path);
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene(GibFrameEditorSettings.Data.defaultSceneName);
            EditorApplication.EnterPlaymode();
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
                if (GibFrameEditorSettings.Data.restoreOpenedScenes)
                {
                    string key = EditorPrefs.GetString(KEY);
                    if (File.Exists(key))
                    {
                        EditorSceneManager.OpenScene(key);
                    }
                }

                break;

            case PlayModeStateChange.EnteredPlayMode:

                break;

            case PlayModeStateChange.ExitingEditMode:
                if (GibFrameEditorSettings.Data.loadDefaultSceneOnPlay && !isLoadingDefaultScene)
                {
                    LoadDefaultScene();
                }
                break;

            case PlayModeStateChange.ExitingPlayMode:
                isLoadingDefaultScene = false;
                break;
        }
    }

    private static bool IsDefaultSceneValid()
    {
        return GibFrameEditorSettings.Data.defaultSceneName != string.Empty && File.Exists(GibFrameEditorSettings.Data.defaultSceneName);
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
