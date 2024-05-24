using Google.Protobuf;
using Protobuf;
using System.Text;

namespace Game.Network
{
    public static class NetworkConvert
    {
        static readonly Encoding encoding = new UTF8Encoding(false);

        public static byte[] ToBytes(string value)
        {
            return encoding.GetBytes(value);
        }

        public static string ToString(byte[] buffer)
        {
            return encoding.GetString(buffer);
        }

        public static string Serialize<T>(T target) where T : IMessage
        {
            if (target != null)
            {
                return JsonFormatter.Default.Format(target);
            }
            return null;
        }

        public static T Deserialize<T>(string json) where T : IMessage, new()
        {
            if (!string.IsNullOrEmpty(json))
            {
                return JsonParser.Default.Parse<T>(json);
            }
            return default;
        }

        public static IMessage Deserialize(RawMessage raw)
        {
            switch (raw.key)
            {
                case NetworkMessageDefine.C2SLoginRequest:
                    return Deserialize<C2SLoginRequest>(raw.content);
            }
            return default;
        }
    }
}
