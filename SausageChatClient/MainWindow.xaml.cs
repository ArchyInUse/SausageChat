using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using SausageChatClient.Networking;
using SausageChatClient.Messaging;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.Collections.ObjectModel;

namespace SausageChatClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public ObservableCollection<IMessage> MyProperty { get; set; }

        public MainWindow()
        {
            SausageClient.Mw = this;
            MyProperty.Add(new UserMessage("Hello!"));
            MyProperty.Add(new ServerMessage("Server Announcment!!!"));
        }

        public void Log(IMessage message)
        {
            Chat_Box_client.AppendText(message.ToString());
        }

   
        private void Mute_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Kick_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Ban_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Info_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Start_server_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Chat_Box_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Send_message_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void User_Message_client_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {

        }
    }
}
