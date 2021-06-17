using UnityEngine;
using UnityEngine.UI;

public static partial class Extension
{
    public static Material CloneMaterial(this Graphic graphic)
    {
        if (graphic != null && graphic.material != null)
        {
            Material source = graphic.material;

            Material clone = GameObject.Instantiate(source);

            graphic.material = clone;

            return clone;
        }
        return null;
    }

    public static Sprite Sprite(this Texture2D texture)
    {
        return UnityEngine.Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
    }

    public static Texture2D Texture2D(this Texture texture)
    {
        int width = texture.width, height = texture.height;

        Texture2D texture2D = new Texture2D(width, height, TextureFormat.RGBA32, false);

        RenderTexture active = RenderTexture.active;

        RenderTexture render = RenderTexture.GetTemporary(width, height, 32);

        Graphics.Blit(texture, render);

        RenderTexture.active = render;

        texture2D.ReadPixels(new Rect(0, 0, width, height), 0, 0);

        texture2D.Apply();

        RenderTexture.active = active;

        RenderTexture.ReleaseTemporary(render);

        return texture2D;
    }
}