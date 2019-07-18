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
        public Dictionary<string, IPEndPoint> Friends { get; set; }

        public ViewModel()
        {
            Friends = new Dictionary<string, IPEndPoint>();
            Messages = new ObservableCollection<IMessage>();
            Messages.Add(new UserMessage("Hello!"));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
