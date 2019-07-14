using System.Collections.Generic;
using SausageChat.Networking;

namespace SausageChat.Helpers
{
    class UserComparer : IComparer<User>
    {
        public int Compare(User x, User y)
        {
            if (x == null || y == null)
                return 0;

            return x.Name.CompareTo(y.Name);
        }
    }
}
