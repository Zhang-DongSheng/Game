using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Game
{
    public static partial class Utility
    {
        public class Class
        {
            public static object Create(string name)
            {
                try
                {
                    Type type = Type.GetType(name);

                    return Activator.CreateInstance(type);
                }
                catch
                {
                    return default;
                }
            }
        }
    }
}