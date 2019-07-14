using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using SausageChat.Messaging;
using SausageChat.Networking;
using SausageChat.Helpers;
using System.Collections.ObjectModel;

namespace SausageChat.Networking
{
    static class Server
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
        public static List<IPEndPoint> Blacklisted { get; set; } = new List<IPEndPoint>();
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
                Mw.AddAsync(new ServerMessage("Closed all sockets"));
            }
        }

        public static ServerCommandResult Ban(User user)
        {
            if (user == null)
                return ServerCommandResult.UserIsNull;
            else if (ConnectedUsers.Any(x => x == user))
            {
                Blacklisted.Add(user.Ip);
                user.SendAsync($"<USER_BANNED>");
                user.Disconnect();
                return ServerCommandResult.Success;
            }
            else
            {
                return ServerCommandResult.UserNotFound;
            }
        }

        public static ServerCommandResult Kick(User user)
        {
            if (user == null)
                return ServerCommandResult.UserIsNull;
            else if (ConnectedUsers.Any(x => x == user))
            {
                user.SendAsync("<USER_KICKED>");
                user.Disconnect();
                return ServerCommandResult.Success;
            }
            else
            {
                return ServerCommandResult.UserNotFound;
            }
        }

        public static ServerCommandResult Mute(User user)
        {
            if (user == null)
                return ServerCommandResult.UserIsNull;
            else if (ConnectedUsers.Any(x => x == user))
            {
                user.SendAsync("<USER_MUTED>");
                user.IsMuted = true;
                return ServerCommandResult.Success;
            }
            else
                return ServerCommandResult.UserNotFound;
        }

        public static ServerCommandResult Unmute(User user)
        {
            if (user == null)
                return ServerCommandResult.UserIsNull;
            else if (user.IsMuted && ConnectedUsers.Any(x => x == user))
            {
                user.IsMuted = false;
                user.SendAsync("<USER_UNMUTED>");
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
            if (!Blacklisted.Any(x => x == user.Ip))
            {
                ConnectedUsers.Add(user);
                Vm.ConnectedUsers = SortUsersList();
                await Log(new ServerMessage($"{user} has joined."));
            }
            else
            {
                await user.SendAsync("<CLIENT_IS_BANNED>");
            }
            MainSocket.BeginAccept(OnUserConnect, null);
        }

        public async static Task Log(IMessage message)
        {
            Vm.Messages.Add(message);
            await Mw.AddAsync(message);

            foreach(var user in ConnectedUsers)
            {
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
