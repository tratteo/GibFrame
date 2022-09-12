// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe.Editor -> GibEditor : GibFrameSettingsEditorWindow.cs
//
// All Rights Reserved

using Pury.Editor;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace GibFrame.Editor
{
    internal class GibFrameSettingsEditorWindow : PuryWindow
    {
        private enum Context { DefaultScene,None }
        private GUILayoutOption[] defaultOptions;
        private GUIStyle sectionsStyle;
        private GibFrameEditorSettings settings;
        private Context context;
        private PurySeparator separator;

        protected override void Layout(List<PurySidebar> sidebars)
        {
            settings = GibFrameEditorSettingsManager.LoadSettings();

            maxSize = new Vector2(700, 512);
            minSize = new Vector2(700, 512);
            ContentOrientation = Orientation.Vertical;

            separator = PurySeparator.Towards(Orientation.Horizontal).Thickness(1).Margin(new RectOffset(10, 10, 15, 15)).Colored(new Color(0.5F, 0.5F, 0.5F, 1)).Build();
            sidebars.Add(PurySidebar.Factory().Left(150).Draw(s =>
            {
                var sidebarStyle = new GUIStyle(GUI.skin.button);
                sidebarStyle.margin = new RectOffset(15, 15, 0, 0);
                VerticalGroup(() =>
                {
                    if(GUILayout.Toggle(context == Context.DefaultScene, "Default scene", GUI.skin.button))
                    {
                        context = Context.DefaultScene;
                    }
                    separator.Draw();
                }, "window", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            }));
        }

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

        protected override void DrawContent()
        {
            switch (context)
            {
                case Context.DefaultScene:
                    var marginStyle = new GUIStyle(GUIStyle.none);
                    marginStyle.margin = new RectOffset(10, 10, 10, 10);
                    VerticalGroup(() =>
                    {
                        HorizontalGroup(() =>
                        {
                            EditorGUILayout.LabelField(new GUIContent("Default scene path", "The path of the default scene, starting from the root folder.\nRequires to specify the extension .unity\nExample: Assets/Scenes/Initializer.unity"), defaultOptions);
                            settings.DefaultSceneName = EditorGUILayout.TextField(settings.DefaultSceneName);
                        });
                        separator.Draw();
                        HorizontalGroup(() =>
                        {
                            EditorGUILayout.LabelField(new GUIContent("Load default scene on play", "Load the specified scene whenever the playmode is entered"), defaultOptions);
                            settings.LoadDefaultSceneOnPlay = EditorGUILayout.Toggle(settings.LoadDefaultSceneOnPlay, defaultOptions);
                        });
                        HorizontalGroup(() =>
                        {
                            EditorGUILayout.LabelField(new GUIContent("Restore opened scenes", "Restore the opened scenes when the playmode is exited"), defaultOptions);
                            settings.RestoreOpenedScenes = EditorGUILayout.Toggle(settings.RestoreOpenedScenes, defaultOptions);
                        });
                    }, marginStyle);

                    break;

                default:
                    break;
            }
        }
    }
}