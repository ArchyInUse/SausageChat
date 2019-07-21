using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using SausageChat.Core.Messaging;
using SausageChat.Helpers;
using SausageChat.Core;
using System.Collections.ObjectModel;

namespace SausageChat.Networking
{
    // TD -> when renaming, make sure there isn't another user called the same name (would mess up Mute/Kick/Ban)
    static class SausageServer
    {
        public static bool IsOpen { get; set; } = false;
        public static ViewModel Vm { get; set; }
        public static MainWindow Mw { get; set; }
        public static Socket MainSocket { get; set; }
        public const int PORT = 60000;
        public static ObservableCollection<SausageConnection> ConnectedUsers
        {
            get
            {
                return Vm.ConnectedUsers;
            }
            set
            {
                Vm.ConnectedUsers = value;
            }
    }
        public static List<IPAddress> Blacklisted { get; set; } = new List<IPAddress>();
        public static IPEndPoint LocalIp { get; set; } = new IPEndPoint(IPAddress.Any, PORT);
        
        public static void Open()
        {
            if(!IsOpen)
            {
                MainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                ConnectedUsers = new ObservableCollection<SausageConnection>();
                MainSocket.Listen(10);
                MainSocket.Bind(LocalIp);
                MainSocket.BeginAccept(OnUserConnect, null);
            }
        }

        public static void Close()
        {
            if(IsOpen)
            {
                MainSocket.Close();
                foreach(SausageConnection u in ConnectedUsers)
                {
                    u.Disconnect();
                }
                Vm.Messages.Add(new ServerMessage("Closed all sockets"));
            }
        }

        public static async Task<ServerCommandResult> Ban(SausageConnection user)
        {
            if (user == null)
                return ServerCommandResult.UserIsNull;
            else if (ConnectedUsers.Any(x => x == user))
            {
                Blacklisted.Add(user.Ip.Address);
                await Log(new ServerMessage($"{MessageType.UserBanned.ToStr()}{user.UserInfo.Name}"));
                await user.Disconnect();
                return ServerCommandResult.Success;
            }
            else
            {
                return ServerCommandResult.UserNotFound;
            }
        }

        public static async Task<ServerCommandResult> Kick(SausageConnection user)
        {
            if (user == null)
                return ServerCommandResult.UserIsNull;
            else if (ConnectedUsers.Any(x => x == user))
            {
                await Log(new ServerMessage($"{SausageHelper.CommandToString(MessageType.UserKicked)}{user.UserInfo.Name}"));
                await user.Disconnect();
                return ServerCommandResult.Success;
            }
            else
            {
                return ServerCommandResult.UserNotFound;
            }
        }

        public static async Task<ServerCommandResult> Mute(SausageConnection user)
        {
            if (user == null)
                return ServerCommandResult.UserIsNull;
            else if (ConnectedUsers.Any(x => x == user))
            {
                await user.SendAsync($"{SausageHelper.CommandToString(MessageType.UserMuted)}{user.UserInfo.Name}");
                user.UserInfo.IsMuted = true;
                return ServerCommandResult.Success;
            }
            else
                return ServerCommandResult.UserNotFound;
        }

        public static async Task<ServerCommandResult> Unmute(SausageConnection user)
        {
            if (user == null)
                return ServerCommandResult.UserIsNull;
            else if (user.UserInfo.IsMuted && ConnectedUsers.Any(x => x == user))
            {
                user.UserInfo.IsMuted = false;
                await user.SendAsync($"{SausageHelper.CommandToString(MessageType.UserUnmuted)}{user.UserInfo.Name}");
                await Log(new ServerMessage($"{SausageHelper.CommandToString(MessageType.UserUnmuted)}{user.UserInfo.Name}"), user);
                return ServerCommandResult.Success;
            }
            else
            {
                return ServerCommandResult.UserNotFound;
            }
        }

        public static async void OnUserConnect(IAsyncResult ar)
        {
            var user = new SausageConnection(MainSocket.EndAccept(ar));
            if (!Blacklisted.Any(x => x == user.Ip.Address))
            {
                ConnectedUsers.Add(user);
                Vm.ConnectedUsers = SortUsersList();
                await Log(new ServerMessage($"{SausageHelper.CommandToString(MessageType.UserListAppend)}{user.UserInfo.Name}"));
            }
            else
            {
                await user.SendAsync($"{SausageHelper.CommandToString(MessageType.UserBlackListed)}");
                await user.Disconnect();
            }
            MainSocket.BeginAccept(OnUserConnect, null);
        }

        public async static Task Log(IMessage message, SausageConnection ignore = null)
        {
            Vm.Messages.Add(message);

            foreach(var user in ConnectedUsers)
            {
                if(user != ignore)
                    user.SendAsync(message.ToString());
            }
        }

        public static ObservableCollection<SausageConnection> SortUsersList()
        {
            List<SausageConnection> names = new List<SausageConnection>(Vm.ConnectedUsers);
            names.Sort(new ConnectionComparer());

            return new ObservableCollection<SausageConnection>(names);
        }
    }
}
