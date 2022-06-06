using System;
using System.Reflection;
using UnityEngine;

namespace Game.Attribute
{
    public class ButtonAttribute : PropertyAttribute
    {
        const BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public string function;

        public bool parameter;

        public ButtonAttribute(string function, bool parameter = true)
        {
            this.function = function;

            this.parameter = parameter;
        }

        public void Call(object target)
        {
            try
            {
                Type type = target.GetType();

                MethodInfo method = type.GetMethod(function, Flags);

                method.Invoke(target, new object[] { });
            }
            catch (Exception e)
            {
                Debuger.LogException(Author.None, e);
            }
        }
    }
}