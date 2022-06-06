using System;
using System.Reflection;
using UnityEngine;

namespace Game.Attribute
{
    public class GraduallyAttribute : PropertyAttribute
    {
        const BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public int step;

        public string function;

        public GraduallyAttribute(string function, int step = 1)
        {
            this.function = function;

            this.step = 1;
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