// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe.Editor -> GibEditor : GibFrameEditorSettingsData.cs
//
// All Rights Reserved

namespace GibEditor
{
    [System.Serializable]
    internal class GibFrameEditorSettingsData
    {
        public string defaultSceneName;
        public bool loadDefaultSceneOnPlay;
        public bool restoreOpenedScenes;

        public GibFrameEditorSettingsData()
        {
            loadDefaultSceneOnPlay = false;
            restoreOpenedScenes = true;
            defaultSceneName = "";
        }
    }
}