// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe.Editor -> GibEditor : GibFrameEditorSettings.cs
//
// All Rights Reserved

using System.IO;
using UnityEditor;
using UnityEngine;

namespace GibFrame.Editor
{
    internal class GibFrameEditorSettings
    {
        internal const string PATH = ".gibconfig.json";

        public static GibFrameEditorSettingsData Data { get; private set; } = null;

        internal static void LoadSettings()
        {
            if (File.Exists(PATH))
            {
                Data = JsonUtility.FromJson<GibFrameEditorSettingsData>(File.ReadAllText(PATH));
            }
            else
            {
                var data = new GibFrameEditorSettingsData();
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
}