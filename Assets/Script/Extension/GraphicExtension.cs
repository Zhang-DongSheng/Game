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
}
