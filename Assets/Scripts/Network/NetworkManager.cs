using System;
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

        private void Receive(byte[] buffer)
        {
            string content = Convert.ToString(buffer);

            try
            {
                var msg = JsonUtility.FromJson<RawMessage>(content);

                Debuger.Log(Author.Network, msg.content);

                NetworkMessageManager.Instance.PostEvent(msg);
            }
            catch (Exception e)
            {
                Debuger.LogException(Author.Network, e);
            }
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