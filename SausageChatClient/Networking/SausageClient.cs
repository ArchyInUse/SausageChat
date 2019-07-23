using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using SausageChat.Core.Messaging;
using SausageChat.Core;
using SausageChat.Core.Networking;
using Newtonsoft.Json;

namespace SausageChatClient.Networking
{
    static class SausageClient
    {
        /* TD -> add try{}catch(){} to Send/Recieve methods
        */ 
        // not a prop to pass it as ref in StripData()
        public static byte[] Data = new byte[1024];
        public static Dictionary<string, IPEndPoint> IpPool { get; set; } = new Dictionary<string, IPEndPoint>()
        {
            ["Disco"] = new IPEndPoint(IPAddress.Parse("89.139.175.8"), 60000)
        };
        public static Dictionary<string, ObservableCollection<User>> Friends
        {
            get
            {
                return Vm.Friends;
            }
            set
            {
                Vm.Friends = value;
            }
        }
        public static IPEndPoint ServerIp { get; set; }
        public static Socket Socket { get; set; }
        public static MainWindow Mw { get; set; }
        public static ViewModel Vm { get; set; }
        public static User ClientInfo { get; set; }
        // This will change soon and shouldn't be binded to.
        public static ObservableCollection<User> Users
        {
            get
            {
                return Vm.Users;
            }
            set
            {
                Vm.Users = value;
            }
        }
        public static Dictionary<Guid, User> UsersDictionary { get; set; }

        public static void Start()
        {
            try
            {
                Users = new ObservableCollection<User>();
                ServerIp = IpPool["Disco"];
                Socket = new Socket(ServerIp.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                Socket.BeginConnect(ServerIp, OnConnect, null);
                ClientInfo = new User();
            }
            catch (SocketException)
            {
                Socket.Close();
            }
        }

        public static void Stop()
        {
            if (!Socket.Connected) return;

            Socket.Close();
        }

        public static void OnConnect(IAsyncResult ar)
        {
            Socket.EndConnect(ar);
            Listen();
        }

        public static void Listen()
        {
            if (!Socket.Connected) return;
            
            Socket.BeginReceive(Data, 0, Data.Length, SocketFlags.None, OnMessageRecieved, null);
        }

        public static void OnMessageRecieved(IAsyncResult ar)
        {
            Socket.EndReceive(ar);

            StripData();
            string message = Encoding.ASCII.GetString(Data);
            Parse(message);

            Data = new byte[1024];
            Listen();
        }
        
        private static void Parse(string msg)
        {
            PacketFormat Message =  JsonConvert.DeserializeObject<PacketFormat>(msg);

            switch(Message.Option)
            {
                case PacketOption.ClientMessage:
                    Log(new UserMessage(Message.Content, UsersDictionary[Message.Guid]));
                    break;
                case PacketOption.IsServer:
                    Log(new ServerMessage(Message.Content));
                    break;
                case PacketOption.NameChange:
                    if (Message.Guid != ClientInfo.Guid)
                    {
                        User user = UsersDictionary[Message.Guid];
                        Log(new ServerMessage($"{user.Name} has changed their name to {Message.NewName}"));
                        user.Name = Message.NewName;
                        break;
                    }
                    Log(new ServerMessage($"You changed your name to {ClientInfo.Name}"));
                    break;
                case PacketOption.UserBanned:
                    if(ClientInfo.Guid == Message.Guid)
                    {
                        Disconnect();
                        Log(new ServerMessage("You've been banned:"));
                        Log(new ServerMessage(Message.Content));
                        break;
                    }
                    Log(new ServerMessage($"{UsersDictionary[Message.Guid]} has been banned:"));
                    Log(new ServerMessage(Message.Content));
                    UsersDictionary.Remove(Message.Guid);
                    break;
                case PacketOption.UserKicked:
                    if (ClientInfo.Guid == Message.Guid)
                    {
                        Disconnect();
                        Log(new ServerMessage("You've been kicked:"));
                        Log(new ServerMessage(Message.Content));
                        break;
                    }
                    Log(new ServerMessage($"{UsersDictionary[Message.Guid]} has been kicked:"));
                    Log(new ServerMessage(Message.Content));
                    UsersDictionary.Remove(Message.Guid);
                    break;
                case PacketOption.UserMuted:
                    if (ClientInfo.Guid == Message.Guid)
                        Log(new ServerMessage("You've been muted:"));
                    else
                        Log(new ServerMessage($"{UsersDictionary[Message.Guid]} has been muted:"));
                    Log(new ServerMessage(Message.Content));
                    UsersDictionary[Message.Guid].IsMuted = true;
                    break;
                case PacketOption.UserUnmuted:
                    Log(new ServerMessage($"{UsersDictionary[Message.Guid]} has been unmuted"));
                    UsersDictionary[Message.Guid].IsMuted = false;
                    break;
                case PacketOption.UserConnected:
                    Log(new ServerMessage($"{Message.Guid} has joined"));
                    UsersDictionary.Add(Message.Guid, new User(Message.NewName, Message.Guid));
                    break;
                case PacketOption.UserDisconnected:
                    Log(new ServerMessage($"{UsersDictionary[Message.Guid]} has disconnected"));
                    UsersDictionary.Remove(Message.Guid);
                    break;
                case PacketOption.UserList:
                    Vm.Users = new ObservableCollection<User>(Message.UsersList);
                    break;
            }
        }

        /// <summary>
        /// removes all null characters
        /// </summary>
        private static void StripData()
        {
            int newSize = Array.FindLastIndex(Data, Data.Length - 1, x => x != 0);

            Array.Resize(ref Data, newSize);
        }

        public static void Send(string message)
        {
            if (!Socket.Connected) return;

            byte[] bytesMessage = Encoding.ASCII.GetBytes(message);

            Socket.BeginSend(bytesMessage, 0, bytesMessage.Length, SocketFlags.None, OnSendComplete, null);
        }

        public static void Send(IMessage message) => Send(message.ToString());

        public static void Send(PacketFormat packet) => Send(JsonConvert.SerializeObject(packet));

        private static void OnSendComplete(IAsyncResult ar) => Socket.EndSend(ar);

        public static void Log(string msg) => Log(new UserMessage(msg));

        public static void Log(IMessage message) => Vm.Messages.Add(message);

        // the server will return the rename message thus no need for logging (in client 
        public static void Rename(string newName)
        {
            ClientInfo.Name = newName;
            PacketFormat packet = new PacketFormat(PacketOption.NameChange)
            {
                Guid = ClientInfo.Guid,
                NewName = newName
            };
            Send(packet);
        }

        // Needs implementation
        public static void AddFriend(Guid guid)
        {
        }

        public static void Disconnect(PacketOption? Po = null)
        {
            try
            {
                if (Socket.Connected) return;
                PacketFormat packet = new PacketFormat(PacketOption.UserDisconnected)
                {
                    Guid = ClientInfo.Guid,
                };
                byte[] inBytes = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(packet));
                Socket.BeginSend(inBytes, 0, inBytes.Length, SocketFlags.None, OnDisconnectDone, null);

                Log(new ServerMessage("Disconnected"));
            }
            catch(SocketException)
            {
                throw new NotImplementedException();
            }
        }

        private static void OnDisconnectDone(IAsyncResult ar)
        {
            Socket.EndSend(ar);
            Socket.Close();
        }
    }
}
