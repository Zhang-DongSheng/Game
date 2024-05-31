using System;
using UnityEngine;

namespace Game.Attribute
{
    /// <summary>
    /// Çø¼ä
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class IntervalAttribute : PropertyAttribute
    {
        public float min { get; private set; }

        public float max { get; private set; }

        public IntervalAttribute(float min, float max)
        {
            this.min = min;

            this.max = max;
        }
    }
}