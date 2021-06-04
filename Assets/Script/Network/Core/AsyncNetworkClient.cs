using System;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace Game.Network
{
    public sealed class AsyncNetworkClient : Client
    {
        public AsyncNetworkClient(string ipString, int port) : base(ipString, port)
        {

        }

        protected override void Connection()
        {
            try
            {
                socket = Create();

                IAsyncResult async = socket.BeginConnect(IP, ConnectionComplete, null);

                bool success = async.AsyncWaitHandle.WaitOne(1000 * 60, true);

                if (!success)
                {
                    Debug.LogError("Á¬½Ó³¬Ê±");
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void ConnectionComplete(IAsyncResult async)
        {
            try
            {
                socket.EndConnect(async);

                officer.Reset();

                officer.receiver = new Thread(Receive)
                {
                    IsBackground = true,
                };
                officer.receiver.Start();
            }
            catch
            {
                Close();
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

        public override void Send(string value)
        {
            if (string.IsNullOrEmpty(value)) return;

            if (!Connected)
            {
                Retry();
            }
            byte[] buffer = Convert.ToBytes(value);

            socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendComplete, null);
        }

        private void SendComplete(IAsyncResult async)
        {
            try
            {
                socket.EndSend(async);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}