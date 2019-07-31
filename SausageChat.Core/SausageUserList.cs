using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SausageChat.Core
{
    /// <summary>
    /// a special implementation of an observable list that acts like a dictionary (and notifies PropertyChanged)
    /// </summary>
    public class SausageUserList : INotifyCollectionChanged
    {
        public ObservableCollection<User> Users { get; set; }

        public User this[Guid guid]
        {
            get
            {
                return Users.First(x => x.Guid == guid);
            }
            set
            {
                var index = Users.IndexOf(Users.First(x => x.Guid == guid));

                Users[index] = value;
            }
        }

        public SausageUserList() => Users = new ObservableCollection<User>();
        public SausageUserList(User[] users) => Users = new ObservableCollection<User>(users);
        
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void NotifyCollectionChanged(NotifyCollectionChangedAction CCA, [CallerMemberName] string CollectionName = "")
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(CCA));
        }

        public void Remove(Guid guid)
        {
            Users.Remove(Users.FirstOrDefault(x => x.Guid == guid));
            NotifyCollectionChanged(NotifyCollectionChangedAction.Remove);
        }

        public void Remove(User user)
        {
            Users.Remove(user);
            NotifyCollectionChanged(NotifyCollectionChangedAction.Remove);
        }

        public void Add(User user)
        {
            Users.Add(user);
            NotifyCollectionChanged(NotifyCollectionChangedAction.Add);
        }

        public void Add(Guid guid)
        {
            Add(new User(guid: guid));
        }
    }
}
