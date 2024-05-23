using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

namespace Data
{
    public static class Convert
    {
        static readonly Encoding encoding = new UTF8Encoding(false);

        public static byte[] Serialize<T>(T target) where T : class
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter binary = new BinaryFormatter();
                binary.Serialize(stream, target);
                return stream.ToArray();
            }
        }

        public static T Deserialize<T>(byte[] buffer) where T : class
        {
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                BinaryFormatter binary = new BinaryFormatter();
                return binary.Deserialize(stream) as T;
            }
        }

        public static string ToJson<T>(T target)
        {
            return JsonUtility.ToJson(target);
        }

        public static T FromJson<T>(string json)
        {
            if (!string.IsNullOrEmpty(json))
            {
                return JsonUtility.FromJson<T>(json);
            }
            return default;
        }

        public static byte[] StringToBytes(string value)
        {
            return encoding.GetBytes(value);
        }

        public static string BytesToString(byte[] buffer)
        {
            return encoding.GetString(buffer);
        }
    }
}