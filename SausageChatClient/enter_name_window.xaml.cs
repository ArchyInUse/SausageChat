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

namespace SausageChatClient
{
  /// <summary>
  /// Interaction logic for Window1.xaml
  /// </summary>
  public partial class Window1
  {
    public Window1()
    {



      InitializeComponent();
    }


    string user_input_string; // will be used to store the name the user has set for himself.  global 



    private void Enter_name_input_box_TextChanged(object sender, TextChangedEventArgs e)
    {

    }





    private void Set_name_Button_Click(object sender, RoutedEventArgs e)
    {
      user_input_string = Enter_name_input_box.Text;


      this.Close();

    }
  }
}
