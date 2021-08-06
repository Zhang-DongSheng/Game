using System.Collections.Generic;

namespace UnityEditor.Listener
{
    public static class TextureListener
    {
        private const int MAXSIZE = 2048;

        private static readonly Dictionary<TextureKind, List<string>> dictionary = new Dictionary<TextureKind, List<string>>()
        {
            {
                TextureKind.TexturePowerof2, new List<string>()
                {
                    "Assets/Art/Texture/Noise",
                }
            },
            {
                TextureKind.Texture, new List<string>()
                {
                   "Assets/Art/Texture/",
                   "Assets/Art/Model/",
                }
            },
            {
                TextureKind.Image, new List<string>()
                {
                    "Assets/Resources/",
                    "Assets/Art/UI/",
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
                case TextureKind.TexturePowerof2:
                    texture.textureType = TextureImporterType.Default;
                    texture.textureShape = TextureImporterShape.Texture2D;
                    break;
                case TextureKind.Image:
                    texture.textureType = TextureImporterType.Sprite;
                    break;
                case TextureKind.NormalMap:
                    texture.textureType = TextureImporterType.NormalMap;
                    texture.textureShape = TextureImporterShape.Texture2D;
                    break;
            }
            texture.sRGBTexture = true;
            bool alpha = texture.DoesSourceTextureHaveAlpha();
            texture.alphaSource = alpha ? TextureImporterAlphaSource.FromInput : TextureImporterAlphaSource.None;
            texture.alphaIsTransparency = alpha;
            texture.npotScale = kind == TextureKind.TexturePowerof2 ? TextureImporterNPOTScale.ToNearest : TextureImporterNPOTScale.None;
            texture.isReadable = false;
            texture.mipmapEnabled = false;
            texture.crunchedCompression = true;
            PlatformSettings(texture, "ios");
            PlatformSettings(texture, "android");
        }

        private static void PlatformSettings(TextureImporter texture, string platform)
        {
            TextureImporterPlatformSettings settings = texture.GetPlatformTextureSettings(platform);
            settings.overridden = true;
            if (settings.maxTextureSize > MAXSIZE)
                settings.maxTextureSize = texture.maxTextureSize > MAXSIZE ? MAXSIZE : texture.maxTextureSize;
            switch (platform)
            {
                case "ios":
                    settings.format = texture.alphaIsTransparency ? TextureImporterFormat.ASTC_4x4 : TextureImporterFormat.PVRTC_RGB4;
                    break;
                case "android":
                    settings.format = texture.alphaIsTransparency ? TextureImporterFormat.ASTC_4x4 : TextureImporterFormat.ETC2_RGB4;
                    break;
                default:
                    settings.format = texture.alphaIsTransparency ? TextureImporterFormat.ASTC_4x4 : TextureImporterFormat.ETC2_RGB4;
                    break;
            }
            settings.compressionQuality = (int)TextureCompressionQuality.Normal;
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
            Image,
            Texture,
            TexturePowerof2,
            NormalMap,
        }
    }
}