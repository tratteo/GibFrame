using UnityEditor;
using UnityEngine;

internal class MenuItems
{
    [MenuItem("GibFrame/Path Builder", false, 22)]
    internal static void ShowPathBuilderWindow()
    {
        EditorWindow.GetWindow(typeof(PathBuilder_W)).titleContent = new GUIContent("Path Builder");
    }

    [MenuItem("GibFrame/Guarded/Verify Scene", false, 0)]
    internal static void VerifySceneGuardedObjects()
    {
        Guarded_E.VerifyGuardedObjects(true, false);
    }

    [MenuItem("GibFrame/Guarded/Verify Prefabs", false, 0)]
    internal static void VerifyPrefabsGuardedObjects()
    {
        Guarded_E.VerifyGuardedObjects(false, true);
    }

    [MenuItem("GibFrame/Guarded/Verify %&v", false, 0)]
    internal static void VerifyGuardedObjects()
    {
        Guarded_E.VerifyGuardedObjects(true, true);
    }

    [MenuItem("GibFrame/Settings", false, 1 << 10)]
    internal static void ShowSettingsWindow()
    {
        EditorWindow.GetWindow(typeof(GibFrameSettings_W)).titleContent = new GUIContent("GibFrame settings");
        GibFrameEditorSettings.LoadSettings();
    }

    [MenuItem("GibFrame/Play default scene %&LEFT", false, 0)]
    internal static void PlayDefaultScene()
    {
        DefaultScene_E.PlayDefaultScene();
    }

    [MenuItem("GibFrame/Load default scene %l", false, 0)]
    internal static void LoadDefaultScene()
    {
        DefaultScene_E.LoadDefaultScene();
    }
}
