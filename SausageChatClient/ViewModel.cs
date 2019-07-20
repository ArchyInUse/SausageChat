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
        public ObservableCollection<string> Users { get; set; } // isnt the < > suppose to be users?  
        public Dictionary<string, IPAddress> Friends { get; set; }




    public ViewModel()
        {
            Users = new ObservableCollection<string>();
            Friends = new Dictionary<string, IPAddress>();
            Messages = new ObservableCollection<IMessage>();
            Messages.Add(new UserMessage("Hello!"));
      Users.Add("User1");

    //  char examplevalue23 = 'a';

     // Users.Add(new string(value23)); does not work. 
      //   Users.Add(new string("test")); Does not accept strings, only chars. 
      // ConnectedUsers.Add(new User("Sally"));
    }

    public event PropertyChangedEventHandler PropertyChanged;
    }
}
