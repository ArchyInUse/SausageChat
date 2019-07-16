using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using SausageChatClient.Messaging;

namespace SausageChatClient
{
    public class ViewModel
    {
        public ObservableCollection<IMessage> Messages { get; set; }

        public ViewModel()
        {
            Messages = new ObservableCollection<IMessage>();
            Messages.Add(new UserMessage("Hello!"));
        }
    }
}
