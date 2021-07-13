using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Game
{
    public static partial class Extension
    {
        private const BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        /// <summary>
        /// ��¡������[System.Serializable]
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
        /// ��ȡ�ֶ�
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
                UnityEngine.Debug.LogException(e);
            }
            return null;
        }
        /// <summary>
        /// �޸��ֶ�
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
                UnityEngine.Debug.LogException(e);
            }
            return false;
        }
        /// <summary>
        /// ��ȡ����
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
                UnityEngine.Debug.LogException(e);
            }
            return null;
        }
        /// <summary>
        /// �޸�����
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
                UnityEngine.Debug.LogException(e);
            }
            return false;
        }
        /// <summary>
        /// ��ȡ��Ա
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
                UnityEngine.Debug.LogException(e);
            }
            return null;
        }
        /// <summary>
        /// �޸ĳ�Ա
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
                UnityEngine.Debug.LogException(e);
            }
            return false;
        }
        /// <summary>
        /// ���÷���
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
                UnityEngine.Debug.LogException(e);
            }
            return null;
        }
    }
}