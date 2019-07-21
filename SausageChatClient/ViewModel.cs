using System.Collections.Generic;
using System.Collections.ObjectModel;
using SausageChat.Core.Messaging;
using System.ComponentModel;
using SausageChat.Core;

namespace SausageChatClient
{
    public class ViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<IMessage> Messages { get; set; }
        public ObservableCollection<User> Users { get; set; }
        // Bind to: "OnlineFriends", "OfflineFriends"
        public Dictionary<string, ObservableCollection<User>> Friends { get; set; }

        public ViewModel()
        {
            Users = new ObservableCollection<User>();
            Friends = new Dictionary<string, ObservableCollection<User>>();
            Friends.Add("OnlineFriends", new ObservableCollection<User>()
            {
                new User("Sally"),
                new User("Bob"),
                new User("Francis")
            });
            Messages = new ObservableCollection<IMessage>();
            Messages.Add(new UserMessage("Hello!"));
            Users.Add(new User("User1"));
            Users.Add(new User("User2"));
            Users.Add(new User("User3"));
          

    }

    public event PropertyChangedEventHandler PropertyChanged;
        }
}
