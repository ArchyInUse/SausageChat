using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SausageChat
{
    public interface IMessage
    {
        string Content { get; set; }
        string FormatMessage();
    }
}
