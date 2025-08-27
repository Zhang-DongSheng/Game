using System;
using UnityEngine;

namespace Game.Attribute
{
    /// <summary>
    /// 自定义显示
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class FieldNameAttribute : PropertyAttribute
    {
        public string name { get; private set; }

        public bool modify { get; private set; }

        public FieldNameAttribute(string name = null, bool modify = true)
        {
            this.name = name;

            this.modify = modify;
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