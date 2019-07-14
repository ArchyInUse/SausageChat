using System.Collections.ObjectModel;
using SausageChat.Messaging;
using SausageChat.Networking;

namespace SausageChat
{
    class ViewModel
    {
        public ObservableCollection<IMessage> Messages { get; set; }
        // User names (for user list)
        public ObservableCollection<User> ConnectedUsers { get; set; }
        public User SelectedUser { get; set; } = null;

        public ViewModel()
        {
            Messages = new ObservableCollection<IMessage>();
        }
    }
}
