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
        public MainWindow()
        {
            ViewModel Vm = new ViewModel();
            SausageClient.Mw = this;
            SausageClient.Vm = Vm;
            this.DataContext = Vm;
    }



    public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      // Always test MultiValueConverter inputs for non-null
      // (to avoid crash bugs for views in the designer)
      if (values[0] is bool && values[1] is bool)
      {
        bool hasText = !(bool)values[0];
        bool hasFocus = (bool)values[1];

        if (hasFocus || hasText)
          return Visibility.Collapsed;
      }

      return Visibility.Visible;
    }


    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
    {
      throw new NotImplementedException();
    }





    // public void Log(IMessage message)
    // {
    //  Chat_Box_client.AppendText(message.ToString());
    //}





    //  Disco's IP  =  index 0
    // Monte's IP  = This is index 1

    public string Which_IP_is_selected(string option)
    {

      if(Select_IP_Combo_box.SelectedIndex == 0)
      {
        //user has selected Disco IP
         option = "disco's IP";
        

      }

      if(Select_IP_Combo_box.SelectedIndex == 1)
      {
        //user has selected  Montes IP
        option = "Montes IP";



      }

      

      return option;
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

        private void Select_IP_Combo_box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      Window1 win2 = new Window1();
      win2.ShowDialog();

    }

    private void Ban_Button_client_Click(object sender, RoutedEventArgs e)
    {

    }

    private void Mute_Button_Click(object sender, RoutedEventArgs e)
    {

    }

    private void Kick_Button_client_Click(object sender, RoutedEventArgs e)
    {

    }

    private void Info_button_client_Click(object sender, RoutedEventArgs e)
    {

    }

    private void Freinds_button_client_Click(object sender, RoutedEventArgs e)
    {

    }

    private void Connect_Button_Client_Click(object sender, RoutedEventArgs e)
    {

    }
  }

  
}
