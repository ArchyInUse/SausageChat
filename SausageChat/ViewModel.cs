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
        public ObservableCollection<IMessage> Messages { get; set; }

        public ViewModel()
        {
            Messages = new ObservableCollection<IMessage>();
            Messages.CollectionChanged += (_, e) =>
            {
                if(e.NewItems is ServerMessage)
                {

                }
                else if(e.NewItems is UserMessage)
                {

                }
            };
        }
    }
}
