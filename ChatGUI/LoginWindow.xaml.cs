using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ConvertedCode;

namespace ChatGUI
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        public static ConvertedCode.Client CPP = new ConvertedCode.Client();
        public static ConvertedCode.client_type client
        {
            set;
            get;
        }
        
        bool success;

        public static string _friend;

        private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();

        private void RemoveText(object sender, RoutedEventArgs e)
        {
            if (Username.Text != "")
                Username.Text = "";
        }

        public void RemoveTextPassword(object sender, RoutedEventArgs e)
        {
            if (Password.Password == "Password")
                Password.Password = "";
        }

        public void ReturnTextUsr_Regis(object sender, RoutedEventArgs e)
        {
            if (UsrName_reg.Text == "")
                UsrName_reg.Text = "Name";
        }

        public void ReturnTextPass_Regis(object sender, RoutedEventArgs e)
        {
            if (Pass_reg.Text == "")
                Pass_reg.Text = "Password";
        }

        public void ReturnTextName_Regis(object sender, RoutedEventArgs e)
        {
            if (Name.Text == "")
                Name.Text = "Name";
        }

        public void ReturnTextEmail_Regis(object sender, RoutedEventArgs e)
        {
            if (Mail_reg.Text == "")
                Mail_reg.Text = "E-mail";
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (Username.Text.Length == 0 || Password.Password.Length == 0)
            {
                MessageBox.Show("Empty Username or Password!!!");
                return;
            }

            if(CPP.Login(client, Username.Text, Password.Password) == true)
            {
                _friend = Username.Text;
                if (warning.Text.Length == 0)
                    warning.Text = "";
                WorkingWindow work = new WorkingWindow();
                work.Show();
                this.Close();
            }
            else
            {
                warning.Text = "ID or Password is incorrect";
                Password.Password = "";
                Username.Focus();
            }
        }

        private void EnterRegister(object sender, RoutedEventArgs e)
        {
            if (LoginGrid.Visibility == Visibility.Visible)
                LoginGrid.Visibility = Visibility.Collapsed;
            RegisterGrid.Visibility = Visibility.Visible;
        }

        private void ReturnLogin(object sender, RoutedEventArgs e)
        {
            if (RegisterGrid.Visibility == Visibility.Visible)
                RegisterGrid.Visibility = Visibility.Collapsed;
            LoginGrid.Visibility = Visibility.Visible;
        }

        private void RemoveText_(object sender, RoutedEventArgs e)
        {
            if((sender as TextBox).Text != "")
            {
                (sender as TextBox).Text = "";
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            if (Name.Text.Length == 0 || UsrName_reg.Text.Length == 0 || Pass_reg.Text.Length == 0 || Mail_reg.Text.Length == 0)
            {
                MessageBox.Show("Please fill all information");
                return;
            }

            if (CPP.Register(client, UsrName_reg.Text, Pass_reg.Text, Name.Text, Birthday.Text, Mail_reg.Text) == true)
            {
                MessageBox.Show("Register successfully");

                UsrName_reg.Text = "";
                Pass_reg.Text = "";
                Name.Text = "";
                Birthday.Text = "";
                Mail_reg.Text = "";
            }
            else
            {
                MessageBox.Show("Username already taken");

                UsrName_reg.Text = "";
                Pass_reg.Text = "";
            }
        }

        private void ConnectServer_Click(object sender, RoutedEventArgs e)
        {
            if (warning.Text != "")
            {
                warning.Text = "";
            }
            else if (ServerIP.Text.Length == 0)
            {
                warning.Text = "Connection Error";
            }
            else
            {
                ConvertedCode.client_type temp = new ConvertedCode.client_type();
                CPP.Init(temp, ServerIP.Text, ref success);

                if (success == true)
                {
                    client = temp;
                    ServerIP.Visibility = Visibility.Collapsed;
                    ConnectServer.Visibility = Visibility.Collapsed;
                    label_ip.Visibility = Visibility.Collapsed;

                    Username.Visibility = Visibility.Visible;
                    Password.Visibility = Visibility.Visible;
                    LoginButton.Visibility = Visibility.Visible;
                    label_username.Visibility = Visibility.Visible;
                    label_pass.Visibility = Visibility.Visible;
                    some_label.Visibility = Visibility.Visible;
                    reg.Visibility = Visibility.Visible;
                }
                else
                    warning.Text = "Connection Error";
            }

        }

        #region Press Enter
        public void EnterOk(object sender, System.Windows.Input.KeyEventArgs k)
        {

            if (Keyboard.Modifiers == ModifierKeys.Control && k.Key == Key.Return)
            {
                ConnectServer.Focus();
                ConnectServer.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, ConnectServer));
            }
        }

        public void Login_Enter(object sender, KeyEventArgs k)
        {
            if(Keyboard.Modifiers == ModifierKeys.Control && k.Key == Key.Enter)
            {
                LoginButton.Focus();
                LoginButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, LoginButton));
            }
        }
        #endregion
    }
}
