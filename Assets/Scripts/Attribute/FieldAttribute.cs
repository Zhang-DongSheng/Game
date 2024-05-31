using System;
using UnityEngine;

namespace Game.Attribute
{
    /// <summary>
    /// 自定义显示
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class DisplayAttribute : PropertyAttribute
    {
        public string name { get; private set; }

        public bool active { get; private set; }

        public DisplayAttribute(string name = null, bool active = true)
        {
            this.active = active;

            this.name = name;
        }
    }
    /// <summary>
    /// 后缀
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class SuffixAttribute : PropertyAttribute
    {
        public string suffix { get; private set; }

        public SuffixAttribute(string suffix)
        {
            this.suffix = suffix;
        }
    }
    /// <summary>
    /// 只读属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ReadonlyAttribute : PropertyAttribute
    {
        public bool editor { get; private set; }

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