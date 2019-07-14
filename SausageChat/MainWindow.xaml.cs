using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using SausageChat.Messaging;
using SausageChat.Networking;

namespace SausageChat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            var vm = new ViewModel();
            DataContext = vm;
            Server.Vm = vm;
            Server.Mw = this;
        }

        /// <summary>
        /// Adds a message to the chat box
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task AddAsync(IMessage message)
        {
            if (message is UserMessage)
            {
                Chat_Box.AppendText(message.FormatMessage());
            }
            else if (message is ServerMessage)
            {
                Chat_Box.AppendText(message.FormatMessage());
            }
        }

        private void Chat_Box_TextChanged(object sender, TextChangedEventArgs e) //event hander for the Chat box. 
        {

        }

        // User list
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) //event for user list box
        {
            Server.Vm.SelectedUser = (User)e.AddedItems[0];
        }

        private void Mute_Button_Click(object sender, RoutedEventArgs e)
        {
            Server.Mute(Server.Vm.SelectedUser);
        }

        private void Kick_Button_Click(object sender, RoutedEventArgs e)
        {
            Server.Kick(Server.Vm.SelectedUser);
        }

        private void Ban_Button_Click(object sender, RoutedEventArgs e)
        {
            Server.Ban(Server.Vm.SelectedUser);
        }

        private void Info_Button_Click(object sender, RoutedEventArgs e) 
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e) ///event for info2 button click
        {

        }

        private void Stop_Server_Click(object sender, RoutedEventArgs e)
        {
            Server.Close();
        }

        private void Start_server_Click(object sender, RoutedEventArgs e)
        {
            Server.Open();
        }

        private void Server_message_button_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            
        }

        private void Send_message_Button_Click(object sender, RoutedEventArgs e)
        {
            Server.Log(new ServerMessage(Server_message_button.Text));
        }
    }
}
