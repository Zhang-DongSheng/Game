using System;
using UnityEngine;

namespace Game.Attribute
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ReadonlyAttribute : PropertyAttribute
    {
        public bool editor;

        public ReadonlyAttribute()
        {
            this.editor = false;
        }

        public ReadonlyAttribute(bool editor)
        {
            this.editor = editor;
        }
    }
}