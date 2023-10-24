using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

namespace Data
{
    public static class Convert
    {
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

        public static string SerializeToJson<T>(T target)
        {
            return JsonUtility.ToJson(target);
        }

        public static T DeserializeFromJson<T>(string json)
        {
            if (!string.IsNullOrEmpty(json))
            {
                return JsonUtility.FromJson<T>(json);
            }
            return default;
        }

        public static byte[] StringToByte(string value)
        {
            return Encoding.Default.GetBytes(value);
        }

        public static string ByteToString(byte[] buffer)
        {
            return Encoding.Default.GetString(buffer);
        }
    }
}