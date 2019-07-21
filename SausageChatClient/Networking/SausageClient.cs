using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using SausageChat.Core.Messaging;
using SausageChat.Core;
using SausageChat.Core.Messaging;

namespace SausageChatClient.Networking
{
    static class SausageClient
    {
        /* TD -> add try{}catch(){} to Send/Recieve methods
         * TD -> Add ID (using GUID) system (will fix naming issues)
        */ 
        // not a prop to pass it as ref in StripData()
        public static byte[] Data = new byte[1024];
        public static Dictionary<string, IPEndPoint> IpPool { get; set; } = new Dictionary<string, IPEndPoint>()
        {
            ["Disco"] = new IPEndPoint(IPAddress.Parse("89.139.175.8"), 60000)
        };
        // TODO: Please fix this public static Dictionary<string, User> Friends
        //{ 
            //get
            //{
              //  return Vm.okFriends;
            //}
            //set
            //{
                //Vm.Friends = value;
            //}
        //}
        public static IPEndPoint ServerIp { get; set; }
        public static Socket Socket { get; set; }
        public static MainWindow Mw { get; set; }
        public static ViewModel Vm { get; set; }
        public static User ClientInfo { get; set; }
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

        public static void Start()
        {
            try
            {
                Users = new ObservableCollection<User>();
                ServerIp = IpPool["Disco"];
                Socket = new Socket(ServerIp.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                Socket.BeginConnect(ServerIp, OnConnect, null);
                ClientInfo = new User(new WebClient().DownloadString("http://ipinfo.io/ip").Trim());
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

        // Needs rename implement
        private static void Parse(string message)
        {
            // Needs implementation
            //if (message.Contains(MessageType.IpRequest))
            //{
            //    string ip;
            //    string name;
            //    message = message.Substring(MessageType.IpRequest.ToStr().Length);
            //    ip = message.Substring(0, message.IndexOf(','));
            //    name = message.Substring(message.IndexOf(',') + 1);
            //    Friends.Add(name, IPAddress.Parse(ip));
            //}
            if(message.Contains(MessageType.UserList))
            {
                message = message.Substring(MessageType.UserList.ToStr().Length);
                if (message == "NULL")
                    Vm.Users = new ObservableCollection<User>();
                else
                {
                    string[] users = message.Split(',');
                    foreach(string user in users)
                    {
                        Vm.Users.Add(new User(user));
                    }
                }
            }
            else if(message.Contains(MessageType.UserMuted))
            {
                message = message.Substring(MessageType.UserMuted.ToStr().Length);
                if(ClientInfo.Name == message)
                    Mw.User_Message_client_Copy.IsEnabled = false;
            }
            else if(message.Contains(MessageType.UserUnmuted))
            {
                message = message.Substring(MessageType.UserUnmuted.ToStr().Length);
                if (ClientInfo.Name == message)
                    Mw.User_Message_client_Copy.IsEnabled = true;
            }
            else if(message.Contains(MessageType.UserKicked))
            {
                message = message.Substring(MessageType.UserKicked.ToStr().Length);
                if (ClientInfo.Name == message)
                {
                    Disconnect(MessageType.UserKicked);
                }
            }
            else if(message.Contains(MessageType.UserBanned))
            {
                message = message.Substring(MessageType.UserBanned.ToStr().Length);
                if (ClientInfo.Name == message)
                {
                    Disconnect(MessageType.UserBanned);
                }
            }
            else
            {
                var Author = new User(message.Substring(4, message.IndexOf(":")));
                var msg = message.Substring(message.IndexOf(":") + 1);

                Log(new UserMessage(msg, Author));
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

        private static void OnSendComplete(IAsyncResult ar) => Socket.EndSend(ar);

        public static void Log(string msg) => Log(new UserMessage(msg));

        public static void Log(IMessage message) => Vm.Messages.Add(message);

        public static void Rename(string newName)
        {
            Send($"{MessageType.NameChanged.ToStr()}{newName}");
            ClientInfo.Name = newName;
        }

        public static void AddFriend(string Name)
        {
            Send($"{MessageType.IpRequest}{Name}");
        }

        public static void Disconnect(MessageType? Ct = null)
        {
            try
            {
                if (Socket.Connected) return;
                Socket.Close();

                if (Ct == null) return;
                if (Ct == MessageType.UserBanned)
                    Log(new ServerMessage("You were banned."));
                else if (Ct == MessageType.UserKicked)
                    Log(new ServerMessage("You were kicked."));
            }
            catch(SocketException)
            {
                throw new NotImplementedException();
            }
        }
    }
}
