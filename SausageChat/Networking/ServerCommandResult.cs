using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SausageChat.Networking
{
    /// <summary>
    /// result from a server command (kick, mute, ban etc) on a user
    /// </summary>
    enum ServerCommandResult
    {
        Success,
        UserNotFound,
        UserIsNull
    }
}
