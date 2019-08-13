using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SausageChat.Core;
using SausageChat.Core.Networking;
using SausageChat.Core.Messaging;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using SausageChatClient.Networking;

namespace SausageChatClient
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class DmWindow : INotifyPropertyChanged
    {
        public static SynchronizationContext UiCtx { get; set; }
        public User Other { get; set; }
        public User ThisUser { get => SausageClient.ClientInfo; }
        public Channel Channel { get; set; }
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        private ObservableCollection<IMessage> _messages;
        public ObservableCollection<IMessage> Messages
        {
            get
            {
                return _messages;
            }
            set
            {
                _messages = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Messages)));
            }
        }

        public DmWindow(Channel channel)
        {
            Channel = channel;
            Other = channel.User;
            DataContext = this;
            Messages = new ObservableCollection<IMessage>();
        }

        public void Init() => InitializeComponent();

        public void Send(IMessage message) => UiCtx.Send(x => Messages.Add(message));

        private void SendMessageDMButtonWindow(object sender, RoutedEventArgs e)
        {

        }

        private void UserMessageInputBoxDMwindowTextChanged(object sender, RoutedEventArgs e)
        {
        }

        private void UserMessageInputBoxForDMWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
                Channel.Send(UserMessageInputBoxForDMWindow.Text);
        }
    }
}