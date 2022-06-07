// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe.Editor -> GibEditor : GibFrameEditorSettings.cs
//
// All Rights Reserved

using Newtonsoft.Json;
using System.IO;

namespace GibFrame.Editor
{
    public class GibFrameEditorSettingsManager
    {
        internal const string Path = ".gibconfig.json";

        public static GibFrameEditorSettings LoadSettings()
        {
            var data = new GibFrameEditorSettings();
            if (File.Exists(Path))
            {
                data = JsonConvert.DeserializeObject<GibFrameEditorSettings>(File.ReadAllText(Path));
            }
            else
            {
                data = new GibFrameEditorSettings();
                File.WriteAllText(Path, data.Serialize());
            }
            return data;
        }

        public static void SaveSettings(GibFrameEditorSettings data) => File.WriteAllText(Path, data.Serialize());
    }
}