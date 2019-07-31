using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace SausageChat.Core
{
    /// <summary>
    /// a special implementation of an observable list that acts like a dictionary (and notifies PropertyChanged)
    /// </summary>
    class SausageUserList : INotifyPropertyChanged, INotifyCollectionChanged
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

        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void NotifyCollectionChanged(NotifyCollectionChangedAction CCA, [CallerMemberName] string propertyName = "")
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(CCA));
        }

        public void Remove(Guid guid)
        {
            Users.Remove(Users.FirstOrDefault(x => x.Guid == guid));
            NotifyPropertyChanged();
            NotifyCollectionChanged(NotifyCollectionChangedAction.Remove);
        }

        public void Remove(User user)
        {
            Users.Remove(user);
            NotifyPropertyChanged();
            NotifyCollectionChanged(NotifyCollectionChangedAction.Remove);
        }

        public void Add(User user)
        {
            Users.Add(user);
            NotifyPropertyChanged();
            NotifyCollectionChanged(NotifyCollectionChangedAction.Add);
        }
    }
}
