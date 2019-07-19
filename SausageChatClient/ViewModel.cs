using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using SausageChatClient.Messaging;
using System.ComponentModel;
using System.Net;

namespace SausageChatClient
{
    public class ViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<IMessage> Messages { get; set; }
        public ObservableCollection<string> Users { get; set; }
        public Dictionary<string, IPAddress> Friends { get; set; }

        public ViewModel()
        {
            Users = new ObservableCollection<string>();
            Friends = new Dictionary<string, IPAddress>();
            Messages = new ObservableCollection<IMessage>();
            Messages.Add(new UserMessage("Hello!"));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
