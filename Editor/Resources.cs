using UnityEngine;

namespace GibFrame.Editor
{
    internal static class Resources
    {
        public const string PackagePath = "Packages/com.tratteo.gibframe";
        public const string PackageEditorPath = "Packages/com.tratteo.gibframe/Editor";
        public const string Path = "Packages/com.tratteo.gibframe/Editor/Icons";

        public static class Icons
        {
            public class IconRequest
            {
                public Texture2D icon;

                public IconRequest(Texture2D icon)
                {
                    this.icon = icon;
                }

                public static implicit operator Texture2D(IconRequest request) => request.Get();

                public IconRequest Resize(int width, int height)
                {
                    var rt = new RenderTexture(width, height, 32);
                    RenderTexture.active = rt;
                    Graphics.Blit(icon, rt);
                    var result = new Texture2D(width, height);
                    result.ReadPixels(new Rect(0, 0, width, height), 0, 0);
                    result.Apply();
                    icon = result;
                    return this;
                }

                public Texture2D Get() => icon;
            }
        }
    }
}