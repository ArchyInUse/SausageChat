using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SausageChat.Messaging;

namespace SausageChat
{
    class ViewModel
    {
        public ObservableCollection<IMessage> Messages { get; set; }
        public ObservableCollection<string> Names { get; set; }

        public ViewModel()
        {
            Messages = new ObservableCollection<IMessage>();
            Names = new ObservableCollection<string>();
        }
    }
}
