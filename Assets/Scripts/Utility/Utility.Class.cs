using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Game
{
    public static partial class Utility
    {
        /// <summary>
        /// ¿‡
        /// </summary>
        public static class Class
        {
            public static object Create(string name)
            {
                Type type = Type.GetType(name) ?? Type.GetType(GetFullName(name));

                if (type == null)
                {
                    Debuger.LogError(Author.Utility, string.Format("The Type of {0}, can't Find!", name));
                    return null;
                }
                else
                {
                    return Create(type);
                }
            }

            public static object Create(Type type, bool defaults = false)
            {
                object instance = null;

                if (type.IsArray)
                {
                    instance = Activator.CreateInstance(type, new object[0] { });
                }
                else if (type.IsValueType)
                {
                    instance = Activator.CreateInstance(type);
                }
                else
                {
                    BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

                    ConstructorInfo[] constructors = type.GetConstructors(flags);

                    for (int i = 0; i < constructors.Length; i++)
                    {
                        ParameterInfo[] parameters = constructors[i].GetParameters();

                        object[] args = new object[parameters.Length];

                        for (int j = 0; j < parameters.Length; j++)
                        {
                            if (defaults)
                            {
                                args[j] = Create(parameters[j].ParameterType);
                            }
                            else
                            {
                                args[j] = null;
                            }
                        }
                        try
                        {
                            instance = Activator.CreateInstance(type, flags, null, args, null);
                            if (instance == null)
                                continue;
                            else
                                break;
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
                if (instance == null)
                {
                    Debuger.LogError(Author.Utility, string.Format("The type of {0}, can't be created!", type));
                }
                return instance;
            }

            public static List<Type> GetChildren(Type parent, bool recursion = false)
            {
                if (parent.IsNotPublic ||
                    parent.IsSealed ||
                    parent.IsEnum)
                {
                    return null;
                }
                List<Type> children = new List<Type>();

                Assembly assembly = Assembly.GetAssembly(parent);

                foreach (var child in assembly.GetTypes())
                {
                    if (child.BaseType == parent)
                    {
                        children.Add(child);

                        if (recursion)
                        {
                            var _children = GetChildren(child, recursion);

                            if (_children != null && _children.Count > 0)
                            {
                                children.AddRange(_children);
                            }
                        }
                    }
                }
                return children;
            }

            public static bool Compare<T>(object x, object y)
            {
                if (x == null && y == null) return true;

                if (x == null || y == null) return false;

                var fields = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                foreach (var field in fields)
                {
                    if (!field.GetValue(x).Equals(field.GetValue(y)))
                    {
                        return false;
                    }
                }
                return true;
            }

            public static string GetPath(Type type)
            {
                string script = type.Name;

                string[] files = Directory.GetFiles(Application.dataPath, "*.cs", SearchOption.AllDirectories);

                string name;

                foreach (var file in files)
                {
                    name = Path.GetFileNameWithoutExtension(file);

                    if (name == script)
                    {
                        return file;
                    }
                }
                return string.Empty;
            }

            public static string GetFullName(string name, Predicate<Type> match = null)
            {
                var assembly = Assembly.GetExecutingAssembly();

                var types = assembly.GetTypes();

                foreach (var type in types)
                {
                    if (match != null && !match(type)) continue;

                    if (name == type.Name)
                    {
                        return type.FullName;
                    }
                }
                return name;
            }
        }
    }
}