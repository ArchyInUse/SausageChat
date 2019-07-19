using System.Collections.ObjectModel;
using SausageChat.Messaging;
using SausageChat.Networking;

namespace SausageChat
{
    class ViewModel
    {
        public ObservableCollection<IMessage> Messages { get; set; } //a collection of messages
        // User names (for user list)
        public ObservableCollection<User> ConnectedUsers { get; set; } // a collection of connected users 
        public User SelectedUser { get; set; } = null;

        public ViewModel()
        {
            Messages = new ObservableCollection<IMessage>();
            Messages.Add(new ServerMessage("Hello!"));
            ConnectedUsers = new ObservableCollection<User>();
            ConnectedUsers.Add(new User("Sally"));
            ConnectedUsers.Add(new User("Bob"));
        }
    }
}
