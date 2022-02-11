using System;

namespace UnityEngine
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class CurveAttribute : PropertyAttribute
    {
        public Color color { get; private set; }

        public Rect ranges { get; private set; }

        public CurveAttribute(Vector2 origination, Vector2 destination, AttributeColor attributeColor)
        {
            this.color = AttributeConfig.Color(attributeColor);

            this.ranges = new Rect(origination.x, origination.y, destination.x - origination.x, destination.y - origination.y);
        }

        public CurveAttribute(AttributeColor attributeColor) : this(Vector2.zero, Vector2.one, attributeColor) { }

        public CurveAttribute(float x, float y, float z, float w, AttributeColor attributeColor) : this(new Vector2(x, y), new Vector2(z, w), attributeColor) { }
    }
}