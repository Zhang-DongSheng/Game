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
}