using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SausageChat.Core;
using SausageChat.Core.Networking;
using SausageChat.Core.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SausageChatClient.Networking
{
    /// <summary>
    /// A wrapper for a DM between 2 people
    /// </summary>
    public class Channel
    {
        public ObservableCollection<IMessage> Messages { get => Window.Messages; set => Window.Messages = value; }
        public ObservableCollection<UserMessage> FirstMessages
        {
            get
            {
                ObservableCollection<UserMessage> userMessages = new ObservableCollection<UserMessage>(Messages.Where(x => x is UserMessage)
                    as ObservableCollection<UserMessage>);
                return new ObservableCollection<UserMessage>(userMessages.Where(x => x.Author.Guid == Client.Guid));
            }
        }
        public ObservableCollection<UserMessage> SecondMessages
        {
            get
            {
                ObservableCollection<UserMessage> userMessages = new ObservableCollection<UserMessage>(Messages.Where(x => x is UserMessage)
                    as ObservableCollection<UserMessage>);
                return new ObservableCollection<UserMessage>(userMessages.Where(x => x.Author.Guid == User.Guid));
            }
        }

        public static SynchronizationContext UiCtx { get => SausageClient.UiCtx; }
        // This client
        public User Client { get; set; }
        // Other client
        public User User { get; set; }
        public DmWindow Window { get; set; }

        public Channel(ref User first, ref User second)
        {
            Client = first;
            User = second;
            Messages = new ObservableCollection<IMessage>();
            Window = new DmWindow(this);
        }

        public void SendToUiContext(IMessage message) => UiCtx.Send(x => Window.Messages.Add(message));
        public void Send(string message)
        {
            PacketFormat packet = new PacketFormat(PacketOption.ClientMessage)
            {
                Content = message,
                Guid = User.Guid,
                Sender = Client.Guid
            };

            SausageClient.Send(packet);
            SendToUiContext(new UserMessage(message, Client));
        }
    }
}
