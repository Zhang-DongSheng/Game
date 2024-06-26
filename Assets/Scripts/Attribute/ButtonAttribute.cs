using System;
using System.Reflection;
using UnityEngine;

namespace Game.Attribute
{
    /// <summary>
    /// ��ť
    /// </summary>
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

        public void Call(object target, object parameter)
        {
            try
            {
                Type type = target.GetType();

                MethodInfo method = type.GetMethod(function, Flags);

                if (this.parameter)
                {
                    method.Invoke(target, new object[] { parameter });
                }
                else
                {
                    method.Invoke(target, null);
                }
            }
            catch (Exception e)
            {
                Debuger.LogException(Author.None, e);
            }
        }
    }
}