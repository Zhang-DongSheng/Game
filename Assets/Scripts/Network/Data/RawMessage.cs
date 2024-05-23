using Google.Protobuf;

namespace Game.Network
{
    [System.Serializable]
    public class RawMessage
    {
        public int key;

        public string content;

        public IMessage message;
    }

    public static class ProtoBufUtils
    {
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
    }
}