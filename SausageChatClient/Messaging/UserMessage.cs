using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SausageChat.Core;

namespace SausageChatClient.Messaging
{
    class UserMessage : IMessage
    {
        public string Content { get; set; }
        public string Author { get; set; }

        public UserMessage(string Content)
        {
            this.Content = Content;
            Author = "Me";
        }

        public UserMessage(string Content, string Author)
        {
            this.Content = Content;
            this.Author = Author;
        }

        public override string ToString() => $"{MessageType.UserMessage}<{Author}>:{Content}";
    }
}
