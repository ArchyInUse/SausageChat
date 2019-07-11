using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SausageChat
{
    class Server
    {
        public static ViewModel Vm;

        public async static Task Log(IMessage message)
        {
            Vm.Messaages.Add(message);
        }
    }
}
