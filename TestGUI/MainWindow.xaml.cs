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
using System.Runtime.InteropServices;

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

        private void ClearUsername(object sender, MouseEventArgs e)
        {
            if (username.Text == "Username")
                username.Text = "";
        }
        private void FillUsername(object sender, MouseEventArgs e)
        {
            if (username.Text == "")
                username.Text = "Username";
        }
        private void ClearPassword(object sender, MouseEventArgs e)
        {
            if (password.Password == "Password")
                password.Password = "";
        }
        private void FillPassword(object sender, MouseEventArgs e)
        {
            if (password.Password == "")
                password.Password = "Password";
        }

        private void TakeUsername(object sender, RoutedEventArgs e) { name = username.Text; }
        private void TakeUsernameRegis(object sender, RoutedEventArgs e) { nameregis = username_regis.Text; }

        private void TakePassword(object sender, RoutedEventArgs e) { pass = password.Password; }
        private void TakePasswordRegis(object sender, RoutedEventArgs e) { passregis = password_regis.Text; }

        private void ClearUsernameRegis(object sender, MouseEventArgs e) {
            if (username_regis.Text == "Username")
                username_regis.Text = ""; 
        }
        private void FillUsernameRegis(object sender, MouseEventArgs e) {
            if (username_regis.Text == "")
            username_regis.Text = "Username";
        }
        private void ClearPasswordRegis(object sender, MouseEventArgs e)
        {
            if (password_regis.Text == "Password")
                password_regis.Text = "";
        }
        private void FillPasswordRegis(object sender, MouseEventArgs e)
        {
            if (password_regis.Text == "")
                password_regis.Text = "Password";
        }
        private void ClearFullnameRegis(object sender, MouseEventArgs e)
        {
            if (fullname_regis.Text == "Full name")
                fullname_regis.Text = "";
        }
        private void FillFullnameRegis(object sender, MouseEventArgs e)
        {
            if (fullname_regis.Text == "")
                fullname_regis.Text = "Full name";
        }
        //private void ClearDayofBirthRegis(object sender, MouseEventArgs e)
        //{
        //    if (dob_regis.Text == "Birthday")
        //        dob_regis.Text = "";
        //}
        //private void FillDayofBirthRegis(object sender, MouseEventArgs e)
        //{
        //    if (dob_regis.Text == "")
        //        dob_regis.Text = "Birthday";
        //}
        private void ClearEmailRegis(object sender, MouseEventArgs e)
        {
            if (email_regis.Text == "E-mail")
                email_regis.Text = "";
        }
        private void FillEmailRegis(object sender, MouseEventArgs e)
        {
            if (email_regis.Text == "")
                email_regis.Text = "E-mail";
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
