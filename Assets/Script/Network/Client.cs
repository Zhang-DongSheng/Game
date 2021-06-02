using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Game.Network
{
    public class Client
    {
        public Action<string> onReceive;

        private Socket socket;

        private readonly IPEndPoint IP;

        private readonly byte[] buffer = new byte[1024];

        public Client(string ipString, int port)
        {
            IPAddress address = IPAddress.Parse(ipString);

            IP = new IPEndPoint(address, port);

            Connection();
        }

        public void Send(string value)
        {
            if (string.IsNullOrEmpty(value)) return;

            if (Connected == false)
            {
                Retry(); return;
            }
            socket.Send(Convert.ToBytes(value));

            Receive();
        }

        public void Close()
        {
            if (Connected)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }

        private void Receive()
        {
            try
            {
                int length = socket.Receive(buffer, 0, 1024, SocketFlags.None);

                string message = Convert.ToString(buffer, 0, length);

                onReceive?.Invoke(message);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void Connection()
        {
            try
            {
                if (Socket.OSSupportsIPv6)
                {
                    socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
                }
                else
                {
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                }
                socket.Connect(IP);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            Receive();
        }

        private void Retry()
        {
            Connection();
        }

        private bool Connected
        {
            get { return socket != null && socket.Connected; }
        }
    }
}