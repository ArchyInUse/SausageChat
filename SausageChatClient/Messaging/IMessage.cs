using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SausageChatClient.Messaging
{
    public interface IMessage
    {
        string Content { get; set; }
        string ToString();
    }
}
