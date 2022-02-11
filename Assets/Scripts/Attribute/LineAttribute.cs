namespace UnityEngine
{
    public class LineAttribute : PropertyAttribute
    {
        public const float HEIGHT = 2.0f;

        public float height { get; private set; }

        public Color color { get; private set; }

        public LineAttribute(AttributeColor attributeColor = AttributeColor.Gray, float height = HEIGHT)
        {
            this.color = AttributeConfig.Color(attributeColor);

            this.height = height;
        }
    }
}