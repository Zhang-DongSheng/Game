using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;

namespace Game
{
    public static partial class Extension
    {
        private const BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        /// <summary>
        /// 克隆，必须[System.Serializable]
        /// </summary>
        public static T Clone<T>(this T script) where T : class
        {
            if (typeof(T).IsSerializable)
            {
                using Stream stream = new MemoryStream();
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, script);
                stream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(stream) as T;
            }
            else
            {
                return default;
            }
        }
        /// <summary>
        /// 获取字段
        /// </summary>
        public static object GetField<T>(this T script, string key) where T : class
        {
            try
            {
                Type type = script.GetType();

                FieldInfo field = type.GetField(key, Flags);

                return field.GetValue(script);
            }
            catch (Exception e)
            {
                Debuger.LogException(Author.None, e);
            }
            return null;
        }
        /// <summary>
        /// 修改字段
        /// </summary>
        public static bool SetField<T>(this T script, string key, object value) where T : class
        {
            try
            {
                Type type = script.GetType();

                FieldInfo field = type.GetField(key, Flags);

                object v = Convert.ChangeType(value, field.FieldType);

                field.SetValue(script, v);

                return true;
            }
            catch (Exception e)
            {
                Debuger.LogException(Author.None, e);
            }
            return false;
        }
        /// <summary>
        /// 获取属性
        /// </summary>
        public static object GetProperty<T>(this T script, string key) where T : class
        {
            try
            {
                Type type = script.GetType();

                PropertyInfo property = type.GetProperty(key, Flags);

                return property.GetValue(script);
            }
            catch (Exception e)
            {
                Debuger.LogException(Author.None, e);
            }
            return null;
        }
        /// <summary>
        /// 修改属性
        /// </summary>
        public static bool SetProperty<T>(this T script, string key, object value) where T : class
        {
            try
            {
                Type type = script.GetType();

                PropertyInfo property = type.GetProperty(key, Flags);

                object v = Convert.ChangeType(value, property.PropertyType);

                property.SetValue(script, v);

                return true;
            }
            catch (Exception e)
            {
                Debuger.LogException(Author.None, e);
            }
            return false;
        }
        /// <summary>
        /// 获取成员
        /// </summary>
        public static object GetMember<T>(this T script, string key) where T : class
        {
            try
            {
                Type type = script.GetType();

                foreach (MemberInfo member in type.GetMember(key, Flags))
                {
                    switch (member.MemberType)
                    {
                        case MemberTypes.Field:
                            {
                                FieldInfo field = (FieldInfo)member;

                                return field.GetValue(script);
                            }
                        case MemberTypes.Property:
                            {
                                PropertyInfo property = (PropertyInfo)member;

                                return property.GetValue(script);
                            }
                        default:
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Debuger.LogException(Author.None, e);
            }
            return null;
        }
        /// <summary>
        /// 修改成员
        /// </summary>
        public static bool SetMember<T>(this T script, string key, object value) where T : class
        {
            try
            {
                Type type = script.GetType();

                foreach (MemberInfo member in type.GetMember(key, Flags))
                {
                    switch (member.MemberType)
                    {
                        case MemberTypes.Field:
                            {
                                FieldInfo field = (FieldInfo)member;

                                object v = Convert.ChangeType(value, field.FieldType);

                                field.SetValue(script, v);

                                return true;
                            }
                        case MemberTypes.Property:
                            {
                                PropertyInfo property = (PropertyInfo)member;

                                object v = Convert.ChangeType(value, property.PropertyType);

                                property.SetValue(script, v);

                                return true;
                            }
                        default:
                            break;
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Debuger.LogException(Author.None, e);
            }
            return false;
        }
        /// <summary>
        /// 调用方法
        /// </summary>
        public static object Call<T>(this T script, string function, params object[] parameters)
        {
            try
            {
                Type type = script.GetType();

                MethodInfo method = type.GetMethod(function, Flags);

                return method.Invoke(script, parameters);
            }
            catch (Exception e)
            {
                Debuger.LogException(Author.None, e);
            }
            return null;
        }
        /// <summary>
        /// 生成集合
        /// </summary>
        public static object GenerateCollection(this Type T)
        {
            Type TL = typeof(List<>);

            Type TS = TL.MakeGenericType(new Type[] { T });

            return Activator.CreateInstance(TS, new object[] { });
        }
        /// <summary>
        /// 是否重写方法
        /// </summary>
        public static bool Override<T>(this T script, string function)
        {
            try
            {
                Type type = script.GetType();

                MethodInfo method = type.GetMethod(function, Flags);

                if (method == null) return false;

                if (method.IsVirtual && !method.IsFinal)
                {
                    return method.DeclaringType != method.GetBaseDefinition().DeclaringType;
                }
                return false;
            }
            catch (Exception e)
            {
                Debuger.LogException(Author.None, e);
            }
            return false;
        }
        /// <summary>
        /// 获取父类的所有子类
        /// </summary>
        public static List<Type> GetChildrenTypes(this Type parent, bool inherit = false)
        {
            List<Type> children = new List<Type>();

            Assembly assembly = Assembly.GetAssembly(parent);

            foreach (Type child in assembly.GetTypes())
            {
                if (child.BaseType == parent)
                {
                    children.Add(child);

                    if (inherit)
                    {
                        var posterity = GetChildrenTypes(child, inherit);
                        if (posterity.Count > 0)
                            children.AddRange(posterity);
                    }
                }
            }
            return children;
        }
        /// <summary>
        /// 是否继承父类
        /// </summary>
        public static bool IsInherit(this Type script, Type parent)
        {
            if (!script.IsClass) return false;

            bool inherit = false;

            Type children = script;

            while (children != null)
            {
                if (children.BaseType == null) break;

                if (children.BaseType.Equals(parent))
                {
                    inherit = true;
                    break;
                }
                children = children.BaseType;
            }
            return inherit;
        }
    }
}