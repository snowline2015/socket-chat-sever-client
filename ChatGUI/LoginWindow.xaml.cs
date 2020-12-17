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
using System.Data.SqlClient;
using System.Data;

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
            if(Username.Text.Length == 0 || Password.Password.Length == 0)
            {
                MessageBox.Show("Empty Username or Password!!!");
                return;
            }

            try
            {
                SqlConnection myConnection = default(SqlConnection);
                myConnection = new SqlConnection(db);
                SqlCommand myCommand = default(SqlCommand);
                myCommand = new SqlCommand("SELECT Username,Password FROM Users WHERE Username = @Username AND Password = @Password", myConnection);
                SqlParameter usrName = new SqlParameter("@Username", SqlDbType.VarChar);
                SqlParameter usrPass = new SqlParameter("@Password", SqlDbType.VarChar);

                usrName.Value = Username.Text;
                usrPass.Value = Password.Password;

                myCommand.Parameters.Add(usrName);
                myCommand.Parameters.Add(usrPass);

                myCommand.Connection.Open();
                SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

                if(myReader.Read() == true)
                {
                    this.Close();
                    work.Show();
                }
                else
                {
                    MessageBox.Show("Incorrect Username or Password!!!");
                    Username.Focus();
                }

                if(myConnection.State == ConnectionState.Open)
                {
                    myConnection.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error occurred", MessageBoxButton.OK);
            }
        }
    }
}
