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

        public ConvertedCode.Client CPP = new ConvertedCode.Client();
        public static ConvertedCode.client_type client
        {
            set;
            get;
        }
        bool success;
       
        //Doi vi tri cuar AttachDbFilename thanh cho luu folder ChatGUI
        String db = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\LENOVO\source\repos\ChatGUI\LoginData.mdf;Integrated Security=True";
        private readonly WorkingWindow work = new WorkingWindow();

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

            //try
            //{
            //    SqlConnection myConnection = default(SqlConnection);
            //    myConnection = new SqlConnection(db);
            //    SqlCommand myCommand = default(SqlCommand);
            //    myCommand = new SqlCommand("SELECT Username,Password FROM Users WHERE Username = @Username AND Password = @Password", myConnection);
            //    SqlParameter usrName = new SqlParameter("@Username", SqlDbType.VarChar);
            //    SqlParameter usrPass = new SqlParameter("@Password", SqlDbType.VarChar);

            //    usrName.Value = Username.Text;
            //    usrPass.Value = Password.Password;

            //    myCommand.Parameters.Add(usrName);
            //    myCommand.Parameters.Add(usrPass);

            //    myCommand.Connection.Open();
            //    SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            //    if (myReader.Read() == true)
            //    {
            //        if (warning.Text.Length != 0)
            //            warning.Text = "";
            //        work.Show();
            //        this.Close();
            //    }
            //    else
            //    {
            //        warning.Text = "Incorrect username or password";
            //        Username.Focus();
            //    }

            //    if (myConnection.State == ConnectionState.Open)
            //    {
            //        myConnection.Dispose();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Error occurred", MessageBoxButton.OK);
            //}

            if(CPP.Login(client, Username.Text, Password.Password) == true)
            {
                if (warning.Text.Length == 0)
                    warning.Text = "";
                work.Show();
                this.Close();
            }
            else
            {
                warning.Text = "ID or Password is incorrect";
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

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            if (Name.Text.Length == 0 || UsrName_reg.Text.Length == 0 || Pass_reg.Text.Length == 0 || Mail_reg.Text.Length == 0)
            {
                MessageBox.Show("Please fill all information");
                return;
            }
            try
            {
                SqlConnection connection = default(SqlConnection);
                connection = new SqlConnection(db);
                string cmd = "Insert into Users (Username,Password,Name,Email,Birthday) VALUES (@Username,@Password,@Name,@Email,@Birthday)";

                SqlCommand command = default(SqlCommand);
                command = new SqlCommand(cmd, connection);

                connection.Open();
                command.Parameters.AddWithValue("@Username", UsrName_reg.Text);
                command.Parameters.AddWithValue("@Password", Pass_reg.Text);
                command.Parameters.AddWithValue("@Name", Name.Text);
                command.Parameters.AddWithValue("@Email", Mail_reg.Text);
                command.Parameters.AddWithValue("@Birthday", Birthday.SelectedDate.Value.Date.ToShortDateString());

                command.ExecuteNonQuery();
                MessageBox.Show("Success");
                connection.Close();

                if (connection.State == ConnectionState.Open)
                {
                    connection.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error occurred", MessageBoxButton.OK);
            }
        }



        private void ConnectServer_Click(object sender, RoutedEventArgs e)
        {
            if (ServerIP.Text.Length == 0)
            {
                warning.Text = "Connection Error";
            }
            else
            {
                ConvertedCode.client_type temp = new ConvertedCode.client_type();
                CPP.Init(ref temp, ServerIP.Text, ref success);

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
