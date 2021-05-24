// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe.Editor -> %Namespace% : EditorSettings.cs
//
// All Rights Reserved

using System.IO;
using UnityEditor;
using UnityEngine;

public class GibFrameEditorSettings
{
    public const string DISABLED_COMMUPDATE = "GIB_NO_COMMUPDATE";
    public const string DISABLED_COMMFIXEDUPDATE = "GIB_NO_COMMFIXEDUPDATE";
    public const string DISABLED_COMMLATEUPDATE = "GIB_NO_COMMLATEUPDATE";
    public const string DISABLED_RUNTIME_UPDATER_INSTANTIATE = "GIB_NO_COMM_RUNTIME_INSTANTIATE";
    internal const string PATH = ".gibconfig.json";

    public static GibFrameEditorSettingsData Data { get; private set; }

    internal static void LoadSettings()
    {
        if (File.Exists(PATH))
        {
            Data = JsonUtility.FromJson<GibFrameEditorSettingsData>(File.ReadAllText(PATH));
        }
        else
        {
            GibFrameEditorSettingsData data = new GibFrameEditorSettingsData();
            File.WriteAllText(PATH, JsonUtility.ToJson(data));
            Data = data;
        }
    }

    internal static void SaveSettings()
    {
        File.WriteAllText(PATH, JsonUtility.ToJson(Data));
    }

    private static void OnPlayModeChanged(PlayModeStateChange state)
    {
        switch (state)
        {
            case PlayModeStateChange.EnteredEditMode:
                LoadSettings();
                break;

            case PlayModeStateChange.EnteredPlayMode:

                break;

            case PlayModeStateChange.ExitingEditMode:
                SaveSettings();
                break;

            case PlayModeStateChange.ExitingPlayMode:

                break;
        }
    }

    [InitializeOnLoad]
    public static class Setup
    {
        static Setup()
        {
            LoadSettings();
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
            EditorApplication.quitting += SaveSettings;
        }
    }
}
