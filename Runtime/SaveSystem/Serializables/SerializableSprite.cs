using UnityEngine;

namespace GibFrame.SaveSystem.Serializables
{
    [System.Serializable]
    public class SerializableSprite
    {
        private int height;
        private int width;
        private byte[] data = null;

        public SerializableSprite(Sprite sprite)
        {
            if (sprite != null)
            {
                Texture2D tex = sprite.texture;
                RenderTexture renderTex = RenderTexture.GetTemporary(tex.width, tex.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
                Graphics.Blit(tex, renderTex);
                RenderTexture previous = RenderTexture.active;
                RenderTexture.active = renderTex;
                Texture2D readableText = new Texture2D(tex.width, tex.height);
                readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
                readableText.Apply();
                RenderTexture.active = previous;
                RenderTexture.ReleaseTemporary(renderTex);

                height = readableText.height;
                width = readableText.width;
                data = ImageConversion.EncodeToPNG(readableText);
            }
        }

        public static implicit operator Sprite(SerializableSprite serializable) => serializable.GetSprite();

        public static implicit operator SerializableSprite(Sprite sprite) => new SerializableSprite(sprite);

        public Sprite GetSprite()
        {
            if (data == null)
            {
                return null;
            }
            Texture2D tex = new Texture2D(this.width, this.height);
            ImageConversion.LoadImage(tex, data);
            Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), Vector2.one);
            return sprite;
        }
    }
}