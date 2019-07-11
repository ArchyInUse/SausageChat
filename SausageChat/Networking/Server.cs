using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SausageChat
{
    static class Server
    {
        public static bool IsOpen = false;
        public static ViewModel Vm;
        public static MainWindow Mw;

        public async static Task Log(IMessage message)
        {
            Vm.Messages.Add(message);
            await Mw.AddAsync(message);
        }
    }
}
