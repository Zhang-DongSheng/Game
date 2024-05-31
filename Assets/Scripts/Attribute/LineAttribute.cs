using UnityEngine;

namespace Game.Attribute
{
    /// <summary>
    /// �߶�
    /// </summary>
    public class LineAttribute : PropertyAttribute
    {
        public float height { get; private set; }

        public Color color;

        public LineAttribute(string hexadecimal, float height = 2f)
        {
            if (ColorUtility.TryParseHtmlString(hexadecimal, out color))
            {

            }
            else
            {
                color = Color.magenta;
            }
            this.height = height;
        }
    }
}