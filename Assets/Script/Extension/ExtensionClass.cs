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
        /// 获取
        /// </summary>
        /// <param name="key"></param>
        public static object GetField<T>(this T script, string key) where T : class
        {
            try
            {
                Type type = script.GetType();

                FieldInfo field = type.GetField(key, Flags);

                object value = field.GetValue(script);

                if (Convert.IsDBNull(value))
                {
                    return null;
                }
                else
                {
                    return value;
                }
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
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
            catch
            {
                return false;
            }
        }
    }
}