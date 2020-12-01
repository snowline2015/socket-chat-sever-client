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

        private void TakeUsername(object sender, RoutedEventArgs e)
        {
            name = username.Text;
        }

        private void TakePassword(object sender, RoutedEventArgs e)
        {
            pass = password.Password;
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
