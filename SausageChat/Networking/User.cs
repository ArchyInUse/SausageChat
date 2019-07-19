using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SausageChat.Messaging;
using SausageChat.Core;

namespace SausageChat.Networking
{
    class User
    {
        public byte[] Data { get; set; } = new byte[1024];
        // to be implemented in Listen functions
        public bool IsMuted { get; set; } = false;
        public bool IsAdmin { get; set; } = false;
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

            SausageServer.Log(new ServerMessage($"{Name} has connected"));
            ListenAsync();
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
                Task.Run(() => Parse(Encoding.ASCII.GetString(Data)));
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

        // need to change the VM names lists, maybe change it to a dictionary
        public string Rename(string newName)
        {
            string oldName = Name;
            Name = newName;
            SausageServer.Vm.ConnectedUsers.First(x => x == this).Name = newName;
            SausageServer.Vm.ConnectedUsers = SausageServer.SortUsersList();
            SausageServer.Log(new ServerMessage($"{oldName} has changed their name to {Name}"));
            return oldName;
        }

        public async Task Disconnect()
        {
            SausageServer.ConnectedUsers.Remove(this);
            SausageServer.Vm.ConnectedUsers = SausageServer.SortUsersList();
            _Socket.Close();
            await SausageServer.Log(new ServerMessage($"{this} disconnected"));
        }

        public async Task Parse(string message)
        {
            // Friend Request (For now, other party doesn't need to accept, needs implementation)
            if (message.Contains(MessageType.IpRequest))
            {
                string CommandCode = MessageType.IpRequest.ToStr();
                foreach (User user in SausageServer.ConnectedUsers)
                {
                    if (user.Name == message.Substring(CommandCode.Length))
                    {
                        await SendAsync($"{CommandCode}{user.Ip.Address.ToString()},{user.Name}");
                    }
                }
            }
            // Rename
            else if (message.Contains(MessageType.NameChanged))
                Rename(message.Substring(MessageType.NameChanged.ToStr().Length));
            // on user join (need to send all the usernames for ViewModel)
            else if (message.Contains(MessageType.OnJoinUserList))
            {
                // since user gets added in SausageServer OnUserConnect method, this will never be 0 on connect)
                if (SausageServer.ConnectedUsers.Count == 1)
                    await SendAsync($"{MessageType.OnJoinUserList.ToStr()}NULL");
                else
                {
                    string ListUsersToSring = "";
                    for (int i = 0; i < SausageServer.ConnectedUsers.Count; i++)
                    {
                        if (i == SausageServer.ConnectedUsers.Count - 1)
                            ListUsersToSring += SausageServer.ConnectedUsers[i].ToString();
                        else
                            ListUsersToSring += SausageServer.ConnectedUsers[i].ToString() + ",";
                    }

                    await SendAsync($"{MessageType.OnJoinUserList.ToStr()}{ListUsersToSring}");
                    await SausageServer.Log(
                        new ServerMessage($"{MessageType.UserListAppend.ToStr()}{Name}"), 
                        this);
                }
            }
            else if (message.Contains(MessageType.UserDisconnect))
                await Disconnect();
            else if(message.Contains(MessageType.UserMessage))
            {
                await SausageServer.Log(new UserMessage(this, message));
            }
        }

        public override string ToString() => Name;

        public override bool Equals(object obj)
        {
            if(obj is User)
            {
                User u = obj as User;
                return u.Ip == this.Ip;
            }
            else
            {
                return false;
            }
        }
    }
}
