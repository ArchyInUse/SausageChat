using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
