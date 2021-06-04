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
                Debug.LogException(e);
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

                    string message = Convert.ToString(buffer, 0, length);

                    onReceive?.Invoke(message);
                }
                catch (ThreadAbortException)
                {
                    break;
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
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
                        socket.Send(Convert.ToBytes(officer.stack.Pop()));
                    }
                }
                catch (ThreadAbortException)
                {
                    break;
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
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