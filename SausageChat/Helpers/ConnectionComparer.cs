using System.Collections.Generic;
using SausageChat.Core;
using SausageChat.Networking;

namespace SausageChat.Helpers
{
    class ConnectionComparer : IComparer<SausageConnection>
    {
        public int Compare(SausageConnection x, SausageConnection y)
        {
            if (x == null || y == null)
                return 0;

            return x.UserInfo.Name.CompareTo(y.UserInfo.Name);
        }
    }
}
