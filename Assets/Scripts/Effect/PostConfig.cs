using UnityEngine;

namespace Game.Effect
{
    public static class PostConfig
    {
        public static string LoadShader(PostEffect effect)
        {
            switch (effect)
            {
                case PostEffect.Bit4:
                    return "Hidden/Aubergine/4Bit";
                default:
                    return string.Format("Hidden/Aubergine/{0}", effect);
            }
        }
    }
    [System.Serializable]
    public class PostProperty
    {
        public string key;

        public PostPropertyType type;

        public int number;

        public float value;

        public Color color;

        public Vector4 vector;

        public Texture2D texture;

        public void Procession(ref Material material)
        {
            if (string.IsNullOrEmpty(key)) return;

            switch (type)
            {
                case PostPropertyType.Float:
                    material.SetFloat(key, value);
                    break;
                case PostPropertyType.Int:
                    material.SetInt(key, number);
                    break;
                case PostPropertyType.Color:
                    material.SetColor(key, color);
                    break;
                case PostPropertyType.Vector:
                    material.SetVector(key, vector);
                    break;
                case PostPropertyType.Texture:
                    material.SetTexture(key, texture);
                    break;
            }
        }
    }

    public enum PostPropertyType
    {
        Float,
        Int,
        Color,
        Vector,
        Texture,
    }

    public enum PostEffect
    {
        Amnesia,
        Bit4,
        BlackAndWhite,
        Bleach,
        BloomSimple,
        BlurH,
        BlurV,
        Charcoal,
        CrossHatch,
        Desaturate,
        Displace,
        Dream_Color,
        Dream_Grey,
        EdgeDetect,
        Emboss,
        FakeSSAO,
        FoggyScreen,
        Frost,
        Godrays1,
        HDR,
        Holywood,
        LensCircle,
        LightShafts,
        LightWave,
        LineArt,
        Negative,
        NightVision,
        NightVisionV2,
        Noise,
        Pencil,
        Pixelated,
        Posterize,
        Pulse,
        RadialBlur,
        Raindrops,
        Ripple,
        Scanlines,
        Scratch,
        ScreenWaves,
        SecurityCamera,
        Sincity,
        SobelEdge,
        SobelOutlineV1,
        SobelOutlineV2,
        SobeloutlineV3,
        SobeloutlineV4,
        SobeloutlineV5,
        SobeloutlineV6,
        SobeloutlineV7,
        Spherical,
        StereoAnaglyph_AmberBlue,
        StereoAnaglyph_GreenMagenta,
        Thermalvisior,
        ThermalvisionV2,
        Thicken,
        Tiles,
        TilesXY,
        ToneMap,
        Vignette,
        Wiggle,
    }
}