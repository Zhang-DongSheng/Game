using System;

namespace UnityEngine
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class IntervalAttribute : PropertyAttribute
    {
        public float min;

        public float max;

        public IntervalAttribute(float min, float max)
        {
            this.min = min;

            this.max = max;
        }
    }
}