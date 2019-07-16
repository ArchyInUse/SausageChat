using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SausageChatClient.Messaging
{
    interface IMessage
    {
        string Content { get; set; }
        string ToString();
    }
}
