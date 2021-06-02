using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace Game.Network
{
    public class Client
    {
        public Action<string> onReceive;

        private Socket socket;

        private readonly IPEndPoint IP;

        private readonly byte[] buffer = new byte[1024];

        private readonly NetworkOfficer officer = new NetworkOfficer();

        public Client(string ipString, int port)
        {
            IPAddress address = IPAddress.Parse(ipString);

            IP = new IPEndPoint(address, port);

            Connection();
        }

        private void Retry()
        {
            Connection();
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

                officer.Reset();

                officer.receiver = new Thread(Receive)
                {
                    IsBackground = true,
                };
                officer.receiver.Start();

                officer.sender = new Thread(Send)
                {
                    IsBackground = true,
                };
                officer.sender.Start();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void Receive()
        {
            while (true)
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
                    break;
                }
            }
        }

        private void Send()
        {
            while (true)
            {
                try
                {
                    if (officer.status)
                    {
                        officer.status = false;

                        socket.Send(Convert.ToBytes(officer.value));
                    }
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    break;
                }
            }
        }

        public void Send(string value)
        {
            if (string.IsNullOrEmpty(value)) return;

            if (!Connected)
            {
                Retry();
            }
            officer.value = value;

            officer.status = true;
        }

        public void Close()
        {
            if (Connected)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            officer.Reset();
        }

        private bool Connected
        {
            get { return socket != null && socket.Connected; }
        }

        ~Client()
        {
            Close();
        }
    }

    public class NetworkOfficer
    {
        public Thread sender;

        public Thread receiver;

        public string value;

        public bool status;

        public void Reset()
        {
            if (sender != null && sender.IsAlive)
            {
                sender.Abort(); sender = null;
            }
            if (receiver != null && receiver.IsAlive)
            {
                receiver.Abort(); receiver = null;
            }
        }
    }
}