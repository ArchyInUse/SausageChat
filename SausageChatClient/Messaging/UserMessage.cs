using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SausageChatClient.Messaging
{
    class UserMessage : IMessage
    {
        public string Content { get; set; }
        public string Author { get; set; }

        public UserMessage(string Content)
        {

        }

        public override string ToString()
        {
            return $"<{Author}>:{Content}";
        }
    }
}
