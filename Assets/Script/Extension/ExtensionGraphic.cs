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
}
