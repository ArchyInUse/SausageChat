using System.Collections.Generic;
using System.Collections.ObjectModel;
using SausageChat.Core.Messaging;
using System.ComponentModel;
using SausageChat.Core;
using System.Runtime.CompilerServices;

namespace SausageChatClient
{
    public class ViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<IMessage> messages;
        public ObservableCollection<IMessage> Messages
        {
            get
            {
                return messages;
            }
            set
            {
                messages = value;
                NotifyPropertyChanged();
            }
        }
        public ObservableCollection<User> Users { get; set; }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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




      Friends.Add("Offlineusers", new ObservableCollection<User>()
            {
                new User("jay"),
                new User("john"),
                new User("sasha")
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
