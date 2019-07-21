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

    public ObservableCollection<User> offlineUsers { get; set; }

    // Bind to: "OnlineFriends", "OfflineFriends"
    //public Dictionary<string, User> Friends { get; set; }

    public Dictionary<string, ObservableCollection<User>> Friends { get; set; }

    public ViewModel()
    {
      Users = new ObservableCollection<User>();

      Friends = new Dictionary<string, ObservableCollection<User>>();

    
      

      Messages = new ObservableCollection<IMessage>();
      Messages.Add(new UserMessage("Hello!"));
      Users.Add(new User("User1"));
      Users.Add(new User("User2"));
    


     // offlineUsers.Add(new User("User3"));
     // offlineUsers.Add(new User("User4"));


      Friends.Add("online_Freinds", Users);

     // Friends.Add("online_Freinds", offlineUsers);




    }

    public event PropertyChangedEventHandler PropertyChanged;
  }
}
