// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe.Editor -> %Namespace% : EditorSettingsData.cs
//
// All Rights Reserved

[System.Serializable]
public class GibFrameEditorSettingsData
{
    public string defaultSceneName;
    public bool loadDefaultSceneOnPlay;
    public bool restoreOpenedScenes;

    public bool enableCommonUpdate;
    public bool enableCommonFixedUpdate;
    public bool enableCommonLateUpdate;

    public GibFrameEditorSettingsData()
    {
        loadDefaultSceneOnPlay = false;
        restoreOpenedScenes = true;
        enableCommonFixedUpdate = true;
        enableCommonLateUpdate = true;
        enableCommonUpdate = true;
        defaultSceneName = "";
    }
}
