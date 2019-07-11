using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SausageChat
{
    // Async server methods implementations are because I don't want to block the GUI thread at all
    class User
    {
        public byte[] Data { get; set; } = new byte[1024];
        public IPEndPoint Ip { get; set; }
        // not following name convention to avoid name colision
        public Socket _Socket { get; set; }
        public string Name { get; set; }
        public List<UserMessage> Messages { get; set; }

        public User(Socket socket)
        {
            _Socket = socket;
            Ip = _Socket.RemoteEndPoint as IPEndPoint;

            // returns just the ip
            Name = _Socket.RemoteEndPoint.ToString().Substring(0, _Socket.RemoteEndPoint.ToString().Length - 6);

            Messages = new List<UserMessage>();
        }

        public async Task SendAsync(string data) => await SendAsync(Encoding.ASCII.GetBytes(data));

        public async Task SendAsync(byte[] data)
        {
            try
            {
                _Socket.BeginSend(data, 0, data.Length, SocketFlags.None, OnSendComplete, null);
            }
            catch (SocketException)
            {
                await Disconnect();
            }
            catch(ObjectDisposedException)
            {
                await Disconnect();
            }
        }

        private void OnSendComplete(IAsyncResult ar)
        {
            try
            {
                _Socket.EndSend(ar);
            }
            catch(SocketException)
            {
                Disconnect();
            }
        }

        public async Task ListenAsync()
        {
            try
            {
                _Socket.BeginReceive(Data, 0, Data.Length, SocketFlags.None, OnDataRecieve, null);
            }
            catch(SocketException)
            {
                await Disconnect();
            }
            catch(ObjectDisposedException)
            {
                await Disconnect();
            }
        }

        private void OnDataRecieve(IAsyncResult ar)
        {
            try
            {
                var BytesRec = _Socket.EndReceive(ar);
                Task.Run(() => ParseMessage(Encoding.ASCII.GetString(Data)));
            }
            catch(SocketException)
            {
                Disconnect();
            }
            catch(ObjectDisposedException)
            {
                Disconnect();
            }
        }

        // to be implemented
        public async Task Disconnect()
        {
            throw new Exception();
        }

        public async Task ParseMessage(string message)
        {
            // I'll change this later to check if the input is a message or a command, not sure on the syntax yet
            if(true)
            {
                await Server.Log(new UserMessage(this, message));
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
