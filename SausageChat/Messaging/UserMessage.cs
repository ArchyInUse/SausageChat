using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SausageChat.Networking;

namespace SausageChat.Messaging
{
    class UserMessage : IMessage
    {
        public string Content { get; set; }
        public User Author { get; set; }

        public UserMessage(User author, string content)
        {
            Content = content;
            Author = author;
        }

        public string FormatMessage()
        {
            return ToString();
        }

        public override string ToString()
        {
            return $"<{Author}>:{Content}";
        }
    }
}
