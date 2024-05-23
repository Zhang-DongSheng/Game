using Google.Protobuf;
using System;

namespace Game.Network
{
    [Serializable]
    public class RawMessage
    {
        public int key;

        public string content;

        public IMessage message;
    }
}