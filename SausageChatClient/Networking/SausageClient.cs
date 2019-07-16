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
        // TI (To implement) - Name, Send, Recieve
        public static byte[] Data { get; set; } = new byte[1024];
        public static Dictionary<string, IPEndPoint> IpPool { get; set; } = new Dictionary<string, IPEndPoint>()
        {
            ["Disco"] = new IPEndPoint(IPAddress.Parse("89.139.180.73"), 60000)
        };
        public static IPEndPoint Ip { get; set; }
        public static Socket Socket { get; set; }
        public static bool Connected { get; set; } = false;
        public static MainWindow Mw { get; set; }

        public static void Start()
        {
            try
            {
                Ip = IpPool["Disco"];
                Socket = new Socket(Ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                Socket.BeginConnect(Ip, OnConnect, null);
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
            int BytesRec = Socket.EndReceive(ar);

            /*
             USE DATA HERE
            */
            Data = new byte[1024];
        }

        public static void Send()
        {
            
        }

        public static void Log(string msg) => Log(new UserMessage(msg));

        public static void Log(IMessage message)
        {
            Mw.Log(message);
        }
    }
}
