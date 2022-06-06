using UnityEngine;

namespace Game.Attribute
{
    public class LineAttribute : PropertyAttribute
    {
        public const float HEIGHT = 2.0f;

        public float height { get; private set; }

        public Color color { get; private set; }

        public LineAttribute(AttributeColor attributeColor = AttributeColor.Gray, float height = HEIGHT)
        {
            this.color = Config.Color(attributeColor);

            this.height = height;
        }
    }
}