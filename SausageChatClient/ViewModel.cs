using System;
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

        private Dictionary<Guid, User> usersDictionary;
        public Dictionary<Guid, User> UsersDictionary
        {
            get
            {
                return usersDictionary;
            }
            set
            {
                usersDictionary = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<User> Users { get; set; }
        public User SelectedUser { get; set; } = null;
        public User SelectedFriend { get; set; } = null;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Bind to: "OnlineFriends", "OfflineFriends"
        public Dictionary<string, ObservableCollection<User>> Friends { get; set; }

        public ViewModel()
        {
            UsersDictionary = new Dictionary<Guid, User>();
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
            Messages.Add(new UserMessage("This is a user message!"));
            Messages.Add(new ServerMessage("This is a server message"));
        }

    public event PropertyChangedEventHandler PropertyChanged;
        }
}
