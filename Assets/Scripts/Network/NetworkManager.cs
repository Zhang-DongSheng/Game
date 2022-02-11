using System.Collections;
using System.Collections.Generic;
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

        private void Receive(string value)
        {
            NetworkEventHandle handle = new NetworkEventHandle()
            {
                content = value,
            };
            NetworkEventManager.PostEvent(NetworkEventKey.Test, handle);
        }

        public void Send(string value)
        {
            if (client != null)
            {
                client.Send(value);
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