using System.Collections.Generic;

namespace UnityEditor.PostListener
{
    public static class TextureListener
    {
        private static readonly Dictionary<TextureKind, List<string>> dictionary = new Dictionary<TextureKind, List<string>>()
        {
            {
                TextureKind.Texture, new List<string>()
                {
                   "Assets/Art/Texture/"
                }
            },
            {
                TextureKind.Image, new List<string>()
                {
                    "Assets/Resources/"
                }
            },
        };

        public static void Start(TextureImporter texture)
        {
            string path = AssetDatabase.GetAssetPath(texture);

            TextureKind kind = Kind(path);

            if (kind == TextureKind.None) return;

            switch (kind)
            {
                case TextureKind.Texture:
                    texture.textureType = TextureImporterType.Default;
                    texture.textureShape = TextureImporterShape.Texture2D;
                    break;
                case TextureKind.Image:
                    texture.textureType = TextureImporterType.Sprite;
                    break;
            }
            texture.sRGBTexture = true;
            bool alpha = texture.DoesSourceTextureHaveAlpha();
            texture.alphaSource = alpha ? TextureImporterAlphaSource.FromInput : TextureImporterAlphaSource.None;
            texture.alphaIsTransparency = alpha;
            texture.npotScale = TextureImporterNPOTScale.None;
            texture.isReadable = false;
            texture.mipmapEnabled = false;
            PlatformSettings(texture, "ios");
            PlatformSettings(texture, "android");
        }

        private static void PlatformSettings(TextureImporter texture, string platform)
        {
            TextureImporterPlatformSettings settings = texture.GetPlatformTextureSettings(platform);
            settings.overridden = true;
            settings.maxTextureSize = texture.maxTextureSize;
            settings.format = TextureImporterFormat.ASTC_4x4;
            texture.SetPlatformTextureSettings(settings);
        }

        private static TextureKind Kind(string path)
        {
            TextureKind kind = TextureKind.None;

            foreach (var _temp in dictionary)
            {
                for (int i = 0; i < _temp.Value.Count; i++)
                {
                    if (path.StartsWith(_temp.Value[i]))
                    {
                        kind = _temp.Key;
                        break;
                    }
                }
                if (kind != TextureKind.None) break;
            }
            return kind;
        }

        enum TextureKind
        {
            None,
            Texture,
            Image,
        }
    }
}