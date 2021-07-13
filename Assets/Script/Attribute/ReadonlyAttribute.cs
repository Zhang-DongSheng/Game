using System;

namespace UnityEngine
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ReadonlyAttribute : PropertyAttribute
    {
        public bool editor;

        public ReadonlyAttribute(bool editor)
        {
            this.editor = editor;
        }
    }
}