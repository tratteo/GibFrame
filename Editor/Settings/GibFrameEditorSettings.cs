// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe.Editor -> GibEditor : GibFrameEditorSettingsData.cs
//
// All Rights Reserved

using Newtonsoft.Json;

namespace GibFrame.Editor
{
    [System.Serializable]
    public class GibFrameEditorSettings
    {
        [JsonProperty("loadDefaultSceneOnPlay")]
        public bool LoadDefaultSceneOnPlay { get; set; }

        [JsonProperty("restoreOpenedScenes")]
        public bool RestoreOpenedScenes { get; set; }

        [JsonProperty("defaultSceneName")]
        public string DefaultSceneName { get; set; }

        public GibFrameEditorSettings()
        {
            LoadDefaultSceneOnPlay = false;
            RestoreOpenedScenes = true;
            DefaultSceneName = string.Empty;
        }

        public string Serialize() => JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}