using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Game.Network
{
    public abstract class Client
    {
        public Action<byte[]> onReceive;

        protected IPEndPoint IP;

        protected Socket socket;

        protected byte[] result;

        protected readonly byte[] buffer = new byte[1024];

        protected readonly StringBuilder builder = new StringBuilder();

        protected readonly NetworkOfficer officer = new NetworkOfficer();

        protected Client(string ipString, int port)
        {
            IPAddress address = IPAddress.Parse(ipString);

            IP = new IPEndPoint(address, port);

            Connection();
        }

        protected Socket Create()
        {
            if (Socket.OSSupportsIPv6)
            {
                return new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            }
            else
            {
                return new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
        }

        protected abstract void Connection();

        protected virtual void Retry()
        {
            Connection();
        }

        protected abstract void Receive();

        public abstract void Send(string value);

        public virtual void Close()
        {
            if (Connected)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            officer.Reset();
        }

        protected bool Connected
        {
            get { return socket != null && socket.Connected; }
        }
    }

    public class NetworkOfficer
    {
        public Thread sender;

        public Thread receiver;

        public readonly Stack<string> stack = new Stack<string>();

        public void Reset()
        {
            if (sender != null)
            {
                sender.Abort();
                sender = null;
            }
            if (receiver != null)
            {
                receiver.Abort();
                receiver = null;
            }
        }
    }
}