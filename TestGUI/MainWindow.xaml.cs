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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string name, pass;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void txt_Changed(object sender, TextChangedEventArgs e)
        {
            if (username.Text.Length > 0)
                username_mask.Visibility = Visibility.Collapsed;
            else
                username_mask.Visibility = Visibility.Visible;
        }

        private void pass_Changed(object sender, RoutedEventArgs e)
        {
            if (password.Password.Length > 0)
                password_mask.Visibility = Visibility.Collapsed;
            else
                password_mask.Visibility = Visibility.Visible;
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
