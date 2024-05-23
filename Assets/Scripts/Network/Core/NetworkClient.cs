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
                    Name = "Unity Receive",
                    IsBackground = true,
                };
                officer.receiver.Start();

                officer.sender = new Thread(Send)
                {
                    Name = "Unity Send",
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

                    result = new byte[length];

                    Array.Copy(buffer, result, length);

                    onReceive?.Invoke(result);
                }
                catch (ThreadAbortException e)
                {
                    Debug.LogException(e);
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
                        lock (officer.stack)
                        {
                            socket.Send(Data.Convert.StringToBytes(officer.stack.Pop()));
                        }
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