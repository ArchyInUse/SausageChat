using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using SausageChatClient.Messaging;

namespace SausageChatClient.Networking
{
    static class SausageClient
    {
        // TD -> Save IPs for DM features
        // not a prop to pass it as ref in StripData()
        public static byte[] Data = new byte[1024];
        public static Dictionary<string, IPEndPoint> IpPool { get; set; } = new Dictionary<string, IPEndPoint>()
        {
            ["Disco"] = new IPEndPoint(IPAddress.Parse("89.139.180.73"), 60000)
        };
        public static Dictionary<string, IPEndPoint> Friends
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
        public static IPEndPoint Ip { get; set; }
        public static Socket Socket { get; set; }
        public static bool Connected { get; set; } = false;
        public static MainWindow Mw { get; set; }
        public static ViewModel Vm { get; set; }
        public static string Name { get; set; }

        public static void Start()
        {
            try
            {
                Ip = IpPool["Disco"];
                Socket = new Socket(Ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                Socket.BeginConnect(Ip, OnConnect, null);
                Name = new WebClient().DownloadString("http://ipinfo.io/ip").Trim();
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

        public static async void OnMessageRecieved(IAsyncResult ar)
        {
            Socket.EndReceive(ar);

            StripData();
            string message = Encoding.ASCII.GetString(Data);
            if (message.Contains("<SM>"))
                Log(new ServerMessage(message.Remove(message.IndexOf("<SM>"), 4)));
            else
            {
                var Author = message.Substring(0, message.IndexOf(":"));
                var msg = message.Substring(message.IndexOf(":") + 1);

                Log(new UserMessage(msg, Author));
            }

            Data = new byte[1024];
            Listen();
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

        public static async Task Send(IMessage message) => Send(message.ToString());

        private static async void OnSendComplete(IAsyncResult ar) => Socket.EndSend(ar);

        public static void Log(string msg) => Log(new UserMessage(msg));

        public static void Log(IMessage message)
        {
            Vm.Messages.Add(message);
        }

        public static async Task Rename(string newName)
        {
            Send($"<NC>{newName}");
            Name = newName;
        }

        public static async Task AddFriend(string Name)
        {

        }
    }
}
