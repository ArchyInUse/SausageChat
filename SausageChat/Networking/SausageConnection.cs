using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SausageChat.Core;
using SausageChat.Core.Messaging;

namespace SausageChat.Networking
{
    class SausageConnection
    {
        public byte[] Data { get; set; } = new byte[1024];
        public IPEndPoint Ip { get; set; }
        public Socket Socket { get; set; }
        public User UserInfo { get; set; }

        public List<UserMessage> Messages { get; set; }

        public SausageConnection(Socket socket)
        {
            Socket = socket;
            Ip = Socket.RemoteEndPoint as IPEndPoint;

            // returns just the ip
            UserInfo.Name = Socket.RemoteEndPoint.ToString().Substring(0, Socket.RemoteEndPoint.ToString().Length - 6);

            Messages = new List<UserMessage>();

            SausageServer.Log(new ServerMessage($"{UserInfo.Name} has connected"));
            ListenAsync();
        }

        /// <summary>
        /// TEST CTOR DON'T USE
        /// </summary>
        /// <param name="n"></param>
        public SausageConnection(string n) => UserInfo = new User(n);

        public async Task SendAsync(string data) => await SendAsync(Encoding.ASCII.GetBytes(data));

        public async Task SendAsync(byte[] data)
        {
            try
            {
                Socket.BeginSend(data, 0, data.Length, SocketFlags.None, OnSendComplete, null);
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
                Socket.EndSend(ar);
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
                Socket.BeginReceive(Data, 0, Data.Length, SocketFlags.None, OnDataRecieve, null);
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
                var BytesRec = Socket.EndReceive(ar);
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
            string oldName = UserInfo.Name;
            UserInfo.Name = newName;
            SausageServer.Vm.ConnectedUsers.First(x => x == this).UserInfo.Name = newName;  
            SausageServer.Vm.ConnectedUsers = SausageServer.SortUsersList();
            SausageServer.Log(new ServerMessage($"{oldName} has changed their name to {UserInfo.Name}"));
            return oldName;
        }

        public async Task Disconnect()
        {
            SausageServer.ConnectedUsers.Remove(this);
            SausageServer.Vm.ConnectedUsers = SausageServer.SortUsersList();
            Socket.Close();
            await SausageServer.Log(new ServerMessage($"{this} disconnected"));
        }

        // needs reimplementation with JSON
        public async Task Parse(string message)
        {
            // Friend Request (For now, other party doesn't need to accept, needs implementation)
            if (message.Contains(MessageType.IpRequest))
            {
                string CommandCode = MessageType.IpRequest.ToStr();
                foreach (SausageConnection user in SausageServer.ConnectedUsers)
                {
                    if (user.UserInfo.Name == message.Substring(CommandCode.Length))
                    {
                        await SendAsync($"{CommandCode}{user.Ip.Address.ToString()},{user.UserInfo.Name}");
                    }
                }
            }
            // Rename
            else if (message.Contains(MessageType.NameChanged))
                Rename(message.Substring(MessageType.NameChanged.ToStr().Length));
            // on user join (need to send all the usernames for ViewModel)
            else if (message.Contains(MessageType.UserList))
            {
                // since user gets added in SausageServer OnUserConnect method, this will never be 0 on connect)
                if (SausageServer.ConnectedUsers.Count == 1)
                    await SendAsync($"{MessageType.UserList.ToStr()}NULL");
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

                    await SendAsync($"{MessageType.UserList.ToStr()}{ListUsersToSring}");
                    await SausageServer.Log(
                        new ServerMessage($"{MessageType.UserListAppend.ToStr()}{UserInfo.Name}"), 
                        this);
                }
            }
            else if (message.Contains(MessageType.UserDisconnect))
                await Disconnect();
            else if(message.Contains(MessageType.UserMessage))
            {
                await SausageServer.Log(new UserMessage(message, UserInfo));
            }
        }

        public override string ToString() => UserInfo.Name;

        public override bool Equals(object obj)
        {
            if(obj is SausageConnection)
            {
                SausageConnection u = obj as SausageConnection;
                return u.Ip == this.Ip;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            var hashCode = -333338465;
            hashCode = hashCode * -1521134295 + EqualityComparer<byte[]>.Default.GetHashCode(Data);
            hashCode = hashCode * -1521134295 + UserInfo.IsMuted.GetHashCode();
            hashCode = hashCode * -1521134295 + UserInfo.IsAdmin.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<IPEndPoint>.Default.GetHashCode(Ip);
            hashCode = hashCode * -1521134295 + EqualityComparer<Socket>.Default.GetHashCode(Socket);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(UserInfo.Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<UserMessage>>.Default.GetHashCode(Messages);
            return hashCode;
        }
    }
}
