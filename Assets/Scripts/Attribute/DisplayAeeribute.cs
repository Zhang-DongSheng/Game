using System;

namespace UnityEngine
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DisplayAttribute : PropertyAttribute
    {
        public string name;

        public bool active;

        public DisplayAttribute(string name = null, bool active = true)
        {
            this.active = active;

            this.name = name;
        }
    }
}