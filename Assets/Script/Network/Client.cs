using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace Game.Network
{
    public class Client
    {
        public Action<string> onReceive;

        private readonly Socket socket;

        private readonly byte[] buffer = new byte[1024];

        public Client(string ipString, int port)
        {
            IPAddress address = IPAddress.Parse(ipString);

            IPEndPoint point = new IPEndPoint(address, port);

            socket = new Socket(Socket.OSSupportsIPv6 ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                socket.Connect(point);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            Receive();
        }

        public void Retry()
        {
            if (socket != null && !socket.Connected)
            {
                socket.Connect(socket.RemoteEndPoint);
            }
        }

        public void Send(string value)
        {
            if (string.IsNullOrEmpty(value)) return;

            socket.Send(Encoding.UTF32.GetBytes(value));

            Receive();
        }

        private void Receive()
        {
            try
            {
                int length = socket.Receive(buffer, 0, 1024, SocketFlags.None);

                string message = Encoding.UTF32.GetString(buffer, 0, length);

                onReceive?.Invoke(message);
            }
            catch
            {

            }
        }

        public void Close()
        {
            if (socket != null && socket.Connected)
            {
                socket.Disconnect(true);
                socket.Close();
            }
        }
    }
}