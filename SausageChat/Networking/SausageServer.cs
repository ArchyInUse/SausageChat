using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using SausageChat.Messaging;
using SausageChat.Helpers;
using SausageChat.Core;
using System.Collections.ObjectModel;

namespace SausageChat.Networking
{
    static class SausageServer
    {
        public static bool IsOpen { get; set; } = false;
        public static ViewModel Vm { get; set; }
        public static MainWindow Mw { get; set; }
        public static Socket MainSocket { get; set; }
        public const int PORT = 60000;
        public static ObservableCollection<User> ConnectedUsers
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
                ConnectedUsers = new ObservableCollection<User>();
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
                foreach(User u in ConnectedUsers)
                {
                    u.Disconnect();
                }
                Vm.Messages.Add(new ServerMessage("Closed all sockets"));
            }
        }

        public static async Task<ServerCommandResult> Ban(User user)
        {
            if (user == null)
                return ServerCommandResult.UserIsNull;
            else if (ConnectedUsers.Any(x => x == user))
            {
                Blacklisted.Add(user.Ip.Address);
                await Log(new ServerMessage($"{CommandParser.ToString(CommandType.UserBanned)}{user.Name}"));
                await user.Disconnect();
                return ServerCommandResult.Success;
            }
            else
            {
                return ServerCommandResult.UserNotFound;
            }
        }

        public static async Task<ServerCommandResult> Kick(User user)
        {
            if (user == null)
                return ServerCommandResult.UserIsNull;
            else if (ConnectedUsers.Any(x => x == user))
            {
                await Log(new ServerMessage($"{CommandParser.ToString(CommandType.UserKicked)}{user.Name}"));
                await user.Disconnect();
                return ServerCommandResult.Success;
            }
            else
            {
                return ServerCommandResult.UserNotFound;
            }
        }

        public static async Task<ServerCommandResult> Mute(User user)
        {
            if (user == null)
                return ServerCommandResult.UserIsNull;
            else if (ConnectedUsers.Any(x => x == user))
            {
                await user.SendAsync($"{CommandParser.ToString(CommandType.UserMuted)}{user.Name}");
                user.IsMuted = true;
                return ServerCommandResult.Success;
            }
            else
                return ServerCommandResult.UserNotFound;
        }

        public static async Task<ServerCommandResult> Unmute(User user)
        {
            if (user == null)
                return ServerCommandResult.UserIsNull;
            else if (user.IsMuted && ConnectedUsers.Any(x => x == user))
            {
                user.IsMuted = false;
                await user.SendAsync($"{CommandParser.ToString(CommandType.UserUnmuted)}{user.Name}");
                await Log(new ServerMessage($"{CommandParser.ToString(CommandType.UserUnmuted)}{user.Name}"), user);
                return ServerCommandResult.Success;
            }
            else
            {
                return ServerCommandResult.UserNotFound;
            }
        }

        public static async void OnUserConnect(IAsyncResult ar)
        {
            var user = new User(MainSocket.EndAccept(ar));
            if (!Blacklisted.Any(x => x == user.Ip.Address))
            {
                ConnectedUsers.Add(user);
                Vm.ConnectedUsers = SortUsersList();
                await Log(new ServerMessage($"{CommandParser.ToString(CommandType.UserListAppend)}{user.Name}"));
            }
            else
            {
                await user.SendAsync($"{CommandParser.ToString(CommandType.UserBlackListed)}");
                await user.Disconnect();
            }
            MainSocket.BeginAccept(OnUserConnect, null);
        }

        public async static Task Log(IMessage message, User ignore = null)
        {
            Vm.Messages.Add(message);

            foreach(var user in ConnectedUsers)
            {
                if(user != ignore)
                    user.SendAsync(message.ToString());
            }
        }

        public static ObservableCollection<User> SortUsersList()
        {
            List<User> names = new List<User>(Vm.ConnectedUsers);
            names.Sort(new UserComparer());

            return new ObservableCollection<User>(names);
        }
    }
}
