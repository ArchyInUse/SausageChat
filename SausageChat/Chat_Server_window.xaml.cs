using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
//using SausageChat.Messaging;
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
      SausageServer.Vm = vm;
      SausageServer.Mw = this;
    }

    /// <summary>
    /// Adds a message to the chat box
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>

    // User list
    private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) //event for user list box
    {
      SausageServer.Vm.SelectedUser = (SausageConnection)e.AddedItems[0];
    }

    private async void Mute_Button_Click(object sender, RoutedEventArgs e)
    {
      await SausageServer.Mute(SausageServer.Vm.SelectedUser);
    }

    private async void Kick_Button_Click(object sender, RoutedEventArgs e)
    {
      await SausageServer.Kick(SausageServer.Vm.SelectedUser);
    }

    private async void Ban_Button_Click(object sender, RoutedEventArgs e)
    {
      await SausageServer.Ban(SausageServer.Vm.SelectedUser);
    }

    private void Info_Button_Click(object sender, RoutedEventArgs e)
    {

    }

    private void Button_Click(object sender, RoutedEventArgs e) ///event for info2 button click
    {

    }

    private void Stop_Server_Click(object sender, RoutedEventArgs e)
    {
      SausageServer.Close();
    }

    private void Start_server_Click(object sender, RoutedEventArgs e)
    {
      SausageServer.Open();
    }

    private void Server_message_button_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
    {

    }

    private void Send_message_Button_Click(object sender, RoutedEventArgs e)
    {
      //Server.Log(new ServerMessage(Server_message_button.Text));
    }
  }
}
