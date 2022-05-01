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
        public bool enableCommonUpdate;
        public bool enableCommonFixedUpdate;
        public bool enableCommonLateUpdate;
        public bool runtimeCommonUpdateInstantiate;

        public GibFrameEditorSettingsData()
        {
            loadDefaultSceneOnPlay = false;
            restoreOpenedScenes = true;
            defaultSceneName = "";

            runtimeCommonUpdateInstantiate = true;
            enableCommonFixedUpdate = true;
            enableCommonLateUpdate = true;
            enableCommonUpdate = true;
        }
    }
}
