using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public static partial class Extension
    {
        /// <summary>
        /// 生成新材质球
        /// </summary>
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
}