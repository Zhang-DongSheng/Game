using System;
using System.Net.Sockets;
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

                IAsyncResult async = socket.BeginConnect(IP, ConnectionCallback, null);

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

        private void ConnectionCallback(IAsyncResult async)
        {
            try
            {
                socket.EndConnect(async);

                Receive();
            }
            catch
            {
                Close();
            }
        }

        protected override void Receive()
        {
            try
            {
                socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void ReceiveCallback(IAsyncResult async)
        {
            try
            {
                int length = socket.EndReceive(async);

                if (length > 0)
                {
                    result = new byte[length];

                    Array.Copy(buffer, result, length);

                    onReceive?.Invoke(result);

                    Receive();
                }
                else
                {
                    Close();
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
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

            socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallback, null);
        }

        private void SendCallback(IAsyncResult async)
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