using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SausageChat
{
    class ServerMessage : IMessage
    {
        public string Content { get; set; }

        public ServerMessage(string content)
        {
            Content = content;
        }

        public string FormatMessage()
        {
            return Content;
        }
    }
}
