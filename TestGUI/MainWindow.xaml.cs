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
        private string name, pass, nameregis, passregis;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ClearUsername(object sender, MouseEventArgs e) { username.Text = ""; }

        private void TakeUsername(object sender, RoutedEventArgs e) { name = username.Text; }
        private void TakeUsernameRegis(object sender, RoutedEventArgs e) { nameregis = username_regis.Text; }

        private void ClearPassword(object sender, MouseEventArgs e) { password.Password = ""; }

        private void TakePassword(object sender, RoutedEventArgs e) { pass = password.Password; }
        private void TakePasswordRegis(object sender, RoutedEventArgs e) { passregis = password_regis.Text; }

        private void ClearUsernameRegis(object sender, MouseEventArgs e) { username_regis.Text = ""; }
        private void ClearPasswordRegis(object sender, MouseEventArgs e) { password_regis.Text = ""; }
        private void ClearFullnameRegis(object sender, MouseEventArgs e) { fullname_regis.Text = ""; }
        private void ClearEmailRegis(object sender, MouseEventArgs e) { email_regis.Text = ""; }
        private void ClearPhoneRegis(object sender, MouseEventArgs e) { phone_regis.Text = ""; }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
