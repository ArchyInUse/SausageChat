using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using SausageChatClient.Messaging;
using System.ComponentModel;

namespace SausageChatClient
{
    public class ViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<IMessage> Messages { get; set; }

        public ViewModel()
        {
            Messages = new ObservableCollection<IMessage>();
            Messages.Add(new UserMessage("Hello!"));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
