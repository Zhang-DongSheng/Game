using System;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace Game.Network
{
    public sealed class NetworkClient : Client
    {
        public NetworkClient(string ipString, int port) : base(ipString, port)
        {

        }

        protected override void Connection()
        {
            try
            {
                socket = Create();

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
                Debuger.LogException(Author.Network, e);
            }
        }

        protected override void Receive()
        {
            while (true)
            {
                try
                {
                    int length = socket.Receive(buffer, 0, 1024, SocketFlags.None);

                    if (length == 0) break;

                    result = new byte[length];

                    Array.Copy(buffer, result, length);

                    onReceive?.Invoke(result);
                }
                catch (ThreadAbortException e)
                {
                    Debuger.LogException(Author.Network, e);
                    break;
                }
                catch (Exception e)
                {
                    Debuger.LogException(Author.Network, e);
                    Close();
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
                    if (officer.stack.Count > 0)
                    {
                        lock (officer.stack)
                        {
                            socket.Send(NetworkConvert.ToBytes(officer.stack.Pop()));
                        }
                    }
                }
                catch (ThreadAbortException)
                {
                    break;
                }
                catch (Exception e)
                {
                    Debuger.LogException(Author.Network, e);
                    break;
                }
            }
        }

        public override void Send(string value)
        {
            if (string.IsNullOrEmpty(value)) return;

            if (!Connected)
            {
                Retry();
            }
            officer.stack.Push(value);
        }
    }
}