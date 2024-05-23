using Google.Protobuf;
using UnityEngine;

namespace Game.Network
{
    public class NetworkManager : Singleton<NetworkManager>
    {
        public const string Address = "127.0.0.1";

        public const int Port = 88;

        private Client client;

        public void Connection()
        {
            if (client != null)
            {

            }
            else
            {
                client = new AsyncNetworkClient(Address, Port)
                {
                    onReceive = Receive,
                };
            }
        }

        private void Receive(byte[] buffer)
        {
            string content = NetworkConvert.ToString(buffer);

            try
            {
                var raw = JsonUtility.FromJson<RawMessage>(content);

                raw.content = Utility.Cryptogram.Decrypt(raw.content);

                //raw.message = NetworkConvert.Deserialize<IMessage>(msg);

                Debuger.Log(Author.Network, raw.content);

                NetworkMessageManager.Instance.PostEvent(raw);
            }
            catch (System.Exception e)
            {
                Debuger.LogException(Author.Network, e);
            }
        }

        public void Send(int key, IMessage message)
        {
            if (client != null)
            {
                var msg = NetworkConvert.Serialize(message);

                var raw = new RawMessage()
                {
                    key = key,
                    content = Utility.Cryptogram.Encrypt(msg),
                };
                client.Send(JsonUtility.ToJson(raw));
            }
        }

        public void Close()
        {
            if (client != null)
            {
                client.Close();
            }
        }
    }
}