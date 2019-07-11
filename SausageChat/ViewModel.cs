using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SausageChat
{
    class ViewModel
    {
        public ObservableCollection<IMessage> Messaages { get; set; }

        public ViewModel()
        {
            Messaages = new ObservableCollection<IMessage>();
            Messaages.CollectionChanged += (_, e) =>
            {
                // the new item is e so you can check if it's a UserMessage or a ServerMessage
            };
        }
    }
}
