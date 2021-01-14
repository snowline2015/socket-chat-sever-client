using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using ConvertedCode;
using AESEncryption;
using System.Text;
using System.Threading;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;

namespace ChatGUI
{
    /// <summary>
    /// Interaction logic for WorkingWindow.xaml
    /// </summary>
    public partial class WorkingWindow : Window
    {
        public WorkingWindow()
        {
            InitializeComponent();
            DataContext = new CurrentTimeViewModel();
        }

        string[] user_list = new string[10];

        public static volatile bool start_flag = false;
        public static volatile bool logout_flag = false;
        public static string item;

        public static Thread my_thread;

        private void TakeUserList()
        {
            Array.Clear(user_list, 0, user_list.Length);
            LoginWindow.CPP.Check_Users_Online(LoginWindow.client, ref user_list);
            foreach (string str in user_list)
            {
                if (str.Equals(LoginWindow._friend) == false)
                    list_friend_pr.Items.Add(str);
            }
        }

        private void SelectUser()
        {
            while (true)
            {
                if (list_friend_pr.SelectedIndex != -1)
                {
                    string _Friend = "";
                    object selected = this.list_friend_pr.SelectedItem;

                    _Friend = selected.ToString();
                    byte[] messageSent = Encoding.ASCII.GetBytes(_Friend);
                    int byteSent = LoginWindow.client.socket.Send(messageSent);
                    break;
                }

                Thread.Sleep(1500);
                TakeUserList();
            }  
        }

        public void OnOpenDialog(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Multiselect = true;
            open.Filter = "All files (*.*)|*.*";
            open.FilterIndex = 1;
            open.ShowDialog();
            
            for (int i = 0; i < open.FileNames.Length; i++)
            {
                if (open.FileNames[i] != "")
                {
                    LoginWindow.CPP.upload_flag = true;

                    byte[] messageSent = Encoding.ASCII.GetBytes("-upload-file\0");
                    int byteSent = LoginWindow.client.socket.Send(messageSent);
                    LoginWindow.CPP.Upload_File(LoginWindow.client, open.FileNames[i]);

                    byte[] messageReceived = new byte[4096];
                    int byteRecv = LoginWindow.client.socket.Receive(messageReceived);

                    LoginWindow.CPP.upload_flag = false;
                }
            }
        }

        #region Some Button
        public void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            byte[] messageSent = Encoding.ASCII.GetBytes("-logout\0");
            int byteSent = LoginWindow.client.socket.Send(messageSent);
            login.Show();
            this.Close();
        }

        private void EnterPrivateChat(object sender, RoutedEventArgs e)
        {
            byte[] messageSent = Encoding.ASCII.GetBytes("-private-chat\0");
            int byteSent = LoginWindow.client.socket.Send(messageSent);

            if (PreChatPanel.Visibility == Visibility.Visible)
                PreChatPanel.Visibility = Visibility.Collapsed;
            PrivateChatPanel.Visibility = Visibility.Visible;
            PrivateChatInfoGrid.Visibility = Visibility.Visible;

            this.TakeUserList();
        }

        private void EnterGroupChat(object sender, RoutedEventArgs e)
        {
            byte[] messageSent = Encoding.ASCII.GetBytes("-public-chat\0");
            int byteSent = LoginWindow.client.socket.Send(messageSent);

            if (PreChatPanel.Visibility == Visibility.Visible)
                PreChatPanel.Visibility = Visibility.Collapsed;
            GroupChatPanel.Visibility = Visibility.Visible;
            GroupChatInfoGrid.Visibility = Visibility.Visible;
        }

        private void CreateRoomClick(object sender, RoutedEventArgs e)
        {
            gr_warning.Text = "";
            //this.EnterPublicChatCommand(ref temp, "-create-room\0");
            byte[] messageSent = Encoding.ASCII.GetBytes("-create-room\0");
            int byteSent = LoginWindow.client.socket.Send(messageSent);

            byte[] messageReceived = new byte[4096];
            int byteRecv = LoginWindow.client.socket.Receive(messageReceived);

            messageSent = Encoding.ASCII.GetBytes(IPAddress_server_gr.Text);
            byteSent = LoginWindow.client.socket.Send(messageSent);

            Array.Clear(messageReceived, 0, messageReceived.Length);
            byteRecv = LoginWindow.client.socket.Receive(messageReceived);

            if (Encoding.ASCII.GetString(messageReceived, 0, byteRecv).Equals("NO\0"))    //Them vai thu
            {
                gr_warning.Text = "Room already existed";
            }

            else
            {
                if (GroupChatInfoGrid.Visibility == Visibility.Visible)
                    GroupChatInfoGrid.Visibility = Visibility.Collapsed;
                GroupChatMain.Visibility = Visibility.Visible;

                LoginWindow.CPP.Start_Client_Group_Chat(LoginWindow.client);
                logout_flag = false;
                my_thread = new Thread(new ThreadStart(AddListboxItemsGroup));
                my_thread.Start();
            }
        }

        private void JoinRoomClick(object sender, RoutedEventArgs e)
        {
            gr_warning.Text = "";

            byte[] messageSent = Encoding.ASCII.GetBytes("-join-room\0");
            int byteSent = LoginWindow.client.socket.Send(messageSent);

            byte[] messageReceived = new byte[4096];
            int byteRecv = LoginWindow.client.socket.Receive(messageReceived);

            messageSent = Encoding.ASCII.GetBytes(IPAddress_server_gr.Text);
            byteSent = LoginWindow.client.socket.Send(messageSent);

            Array.Clear(messageReceived, 0, messageReceived.Length);
            byteRecv = LoginWindow.client.socket.Receive(messageReceived);

            if (Encoding.ASCII.GetString(messageReceived, 0, byteRecv).Equals("NO\0"))    //Them vai thu
            {
                gr_warning.Text = "Room does not exist";
            }

            else
            {
                if (GroupChatInfoGrid.Visibility == Visibility.Visible)
                    GroupChatInfoGrid.Visibility = Visibility.Collapsed;
                GroupChatMain.Visibility = Visibility.Visible;

                LoginWindow.CPP.Start_Client_Group_Chat(LoginWindow.client);

                my_thread = new Thread(new ThreadStart(AddListboxItemsGroup));
                my_thread.Start();
            }
        }

        private void EnterMoreOpts(object sender, RoutedEventArgs e)
        {
            byte[] messageSent = Encoding.ASCII.GetBytes("-other-option\0");
            int byteSent = LoginWindow.client.socket.Send(messageSent);

            if (PreChatPanel.Visibility == Visibility.Visible)
                PreChatPanel.Visibility = Visibility.Collapsed;
            MoreOptPanel.Visibility = Visibility.Visible;
        }

        public void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            byte[] messageSent = Encoding.ASCII.GetBytes("-logout\0");
            int byteSent = LoginWindow.client.socket.Send(messageSent);
            this.Close();
        }

        private void ConnectPrivateChat(object sender, RoutedEventArgs e)
        {
            this.SelectUser();
            if (PrivateChatInfoGrid.Visibility == Visibility.Visible)
                PrivateChatInfoGrid.Visibility = Visibility.Collapsed;

            PrivateChatMain.Visibility = Visibility.Visible;
            LoginWindow.CPP.Start_Client_Private_Chat(LoginWindow.client);
            logout_flag = false;
            my_thread = new Thread(() => AddListboxItems(my_thread));
            my_thread.Start();
        }

        private void ReturnOptions(object sender, RoutedEventArgs e)
        {
            if (PrivateChatInfoGrid.Visibility == Visibility.Visible)
                PrivateChatInfoGrid.Visibility = Visibility.Collapsed;
            PrivateChatPanel.Visibility = Visibility.Collapsed;
            PreChatPanel.Visibility = Visibility.Visible;
        }

        private void _ReturnOptions(object sender, RoutedEventArgs e)
        {
            if (GroupChatInfoGrid.Visibility == Visibility.Visible)
                GroupChatInfoGrid.Visibility = Visibility.Collapsed;
            GroupChatPanel.Visibility = Visibility.Collapsed;
            PreChatPanel.Visibility = Visibility.Visible;
        }

        private void ReturnMainOptions(object sender, RoutedEventArgs e)
        {
            byte[] messageSent = Encoding.ASCII.GetBytes("-back\0");
            int byteSent = LoginWindow.client.socket.Send(messageSent);

            if (MoreOptPanel.Visibility == Visibility.Visible)
                MoreOptPanel.Visibility = Visibility.Collapsed;
            PreChatPanel.Visibility = Visibility.Visible;
        }
        #endregion

        private void Send_Enter(object sender, KeyEventArgs k)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && k.Key == Key.Enter)
            {
                Send.Focus();
                Send.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, Send));
            }
        }

        private void Send_Enter_(object sender, KeyEventArgs k)
        {
            if(Keyboard.Modifiers == ModifierKeys.Control && k.Key == Key.Enter)
            {
                Send1.Focus();
                Send1.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, Send1));
            }
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            if (ChatBox_pr.Text.Length != 0)
            {
                PrivateChat.Items.Add("Me at " + DateTime.Now.ToString("HH:mm") +" :\n" + ChatBox_pr.Text + "\n");
                PrivateChat.SelectedIndex = PrivateChat.Items.Count - 1;

                byte[] messageSent = Encoding.ASCII.GetBytes(ChatBox_pr.Text);
                int byteSent = LoginWindow.client.socket.Send(messageSent);

                ChatBox_pr.Text = "";
            }
        }

        private void EndChat_Click(object sender, RoutedEventArgs e)
        {
            PrivateChatMain.Visibility = Visibility.Collapsed;
            logout_flag = true;

            LoginWindow.CPP.stop_flag = true;
            byte[] messageSent = Encoding.ASCII.GetBytes("-back\0");
            int byteSent = LoginWindow.client.socket.Send(messageSent);

            LoginWindow.CPP.End_Client_Private_Chat(LoginWindow.client);
            my_thread.Abort();
            start_flag = false;

            PrivateChatPanel.Visibility = Visibility.Collapsed;
            PreChatPanel.Visibility = Visibility.Visible;
        }

        public void AddListboxItems(Thread my_thread)
        {
            string[] item_array;
            char[] sep = { ':' };
            while (!logout_flag)
            {
                if (start_flag)
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        item_array = item.Split(sep, 2);

                        if (item_array[0] == "-disconnect")
                        {
                            PrivateChat.Items.Add("User disconnected\nEnding private chat.....");
                            Thread.Sleep(2000);

                            PrivateChatMain.Visibility = Visibility.Collapsed;
                            logout_flag = true;
                            start_flag = false;
                            LoginWindow.CPP.End_Client_Private_Chat(LoginWindow.client);
                           
                            byte[] messageSent = Encoding.ASCII.GetBytes("-back\0");
                            int byteSent = LoginWindow.client.socket.Send(messageSent);

                            PrivateChatPanel.Visibility = Visibility.Collapsed;
                            PreChatPanel.Visibility = Visibility.Visible;

                            //exit_pr_chat.Focus();
                            //exit_pr_chat.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, exit_pr_chat));
                        }
                        else
                        {
                            PrivateChat.Items.Add(item_array[0] + " at " + DateTime.Now.ToString("HH:mm") + " :\n" + item_array[1] + "\n");
                            PrivateChat.SelectedIndex = PrivateChat.Items.Count - 1;
                            start_flag = false;
                        }
                    }));
                }
            }
            my_thread.Abort();
        }

        private void Send_Click_Group(object sender, RoutedEventArgs e)
        {
            if (ChatBox_gr.Text.Length != 0)
            {
                GroupChat.Items.Add("Me at " + DateTime.Now.ToString("HH:mm") + " :\n" + ChatBox_gr.Text + "\n");
                GroupChat.SelectedIndex = GroupChat.Items.Count - 1;

                byte[] messageSent = Encoding.ASCII.GetBytes(ChatBox_gr.Text);
                int byteSent = LoginWindow.client.socket.Send(messageSent);

                ChatBox_gr.Text = "";
            }
        }

        public void AddListboxItemsGroup()
        {
            string[] item_array;
            char[] sep = { ':' };
            while (!logout_flag)
            {
                if (start_flag)
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        item_array = item.Split(sep, 2);
                        GroupChat.Items.Add(item_array[0] + " at " + DateTime.Now.ToString("HH:mm") + " :\n" + item_array[1] + "\n");
                        GroupChat.SelectedIndex = GroupChat.Items.Count - 1;
                        start_flag = false;
                    }));
                }
            }
        }

        private void EndGroupChat_Click(object sender, RoutedEventArgs e)
        {
            GroupChatMain.Visibility = Visibility.Collapsed;
            logout_flag = true;

            LoginWindow.CPP.stop_flag = true;
            byte[] messageSent = Encoding.ASCII.GetBytes("-back\0");
            int byteSent = LoginWindow.client.socket.Send(messageSent);

            LoginWindow.CPP.End_Client_Group_Chat(LoginWindow.client);
            my_thread.Join();
            start_flag = false;

            GroupChatPanel.Visibility = Visibility.Collapsed;
            PreChatPanel.Visibility = Visibility.Visible;
        }

        private void OnlyNumber(object sender, TextCompositionEventArgs e)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void MoveMainWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void EnterChangePass(object sender, RoutedEventArgs e)
        {
            if (MoreOptGrid.Visibility == Visibility.Visible)
                MoreOptGrid.Visibility = Visibility.Collapsed;
            ChangePassGrid.Visibility = Visibility.Visible;
        }

        private void EnterCheckUser(object sender, RoutedEventArgs e)
        {
            if (MoreOptGrid.Visibility == Visibility.Visible)
                MoreOptGrid.Visibility = Visibility.Collapsed;
            CheckUsrGrid.Visibility = Visibility.Visible;
        }

        private void EnterChangeInfo(object sender, RoutedEventArgs e)
        {
            if (MoreOptGrid.Visibility == Visibility.Visible)
                MoreOptGrid.Visibility = Visibility.Collapsed;
            ChangeInfoGrid.Visibility = Visibility.Visible;

            string[] usr_info = new string[5];
            bool chk = LoginWindow.CPP.Check_User_Info(LoginWindow.client, LoginWindow._friend + "\0", ref usr_info);
            fullname_info.Text = usr_info[0];
            birthday_info.Text = usr_info[1];
            email_info.Text = usr_info[2];
            bio_info.Text = usr_info[3];
        }

        private void ChangePass_Click(object sender, RoutedEventArgs e)
        {
            ChangePass_warning.Foreground = new SolidColorBrush(Colors.Red);
            if (NewPass_again.Password != NewPass.Password)
                ChangePass_warning.Text = "New password do not match with current password";
            else
            {
                LoginWindow.CPP.Change_Password(LoginWindow.client, LoginWindow._friend + "\0", NewPass.Password + "\0");
                ChangePass_warning.Foreground = new SolidColorBrush(Colors.Green);
                ChangePass_warning.Text = "Password update successfully";
            }
        }

        private void CheckUer_Click(object sender, RoutedEventArgs e)
        {
            if (check_usr.Text == "")
                CheckUsr_warning.Text = "Please input a username";

            string[] usr_info = new string[5];
            bool chk = LoginWindow.CPP.Check_User_Info(LoginWindow.client, check_usr.Text + "\0", ref usr_info);
            if(chk == false)
                CheckUsr_warning.Text = "Cannot find input user";
            else
            {
                CheckUsr_warning.Text = "";

                User_search.Text = check_usr.Text;
                Fullname_search.Text = usr_info[0];
                Birthday_search.Text = usr_info[1];
                Email_search.Text = usr_info[2];
                Bio_search.Text = usr_info[3];
                if(usr_info[4] == "Online")
                    status_search1.Foreground = new SolidColorBrush(Colors.Green);
                else
                    status_search1.Foreground = new SolidColorBrush(Colors.Red);
                status_search1.Text = usr_info[4];

                Search_result.Visibility = Visibility.Visible;
            }
        }

        private void CheckUser_Enter(object sender, KeyEventArgs k)
        {
            if(Keyboard.Modifiers == ModifierKeys.Control && k.Key == Key.Enter)
            {
                search_usr.Focus();
                search_usr.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, search_usr));
            }
        }

        private void ChangeInfo(object sender, RoutedEventArgs e)
        {
            ChangeInfo_warning.Foreground = new SolidColorBrush(Colors.Red);
            if (fullname_info.Text == "" || birthday_info.Text == "" || email_info.Text == "")
                ChangeInfo_warning.Text = "Pleare fill all information";
            else
            {
                LoginWindow.CPP.Change_Info(LoginWindow.client, LoginWindow._friend + "\0", fullname_info.Text + "\0",
                    birthday_info.Text + "\0", email_info.Text + "\0", bio_info.Text + "\0");
                ChangeInfo_warning.Foreground = new SolidColorBrush(Colors.Green);
                ChangeInfo_warning.Text = "Information update successfully";
            }
        }

        private void CancelChangeInfo(object sender, RoutedEventArgs e)
        {
            byte[] messageSent = Encoding.ASCII.GetBytes("-cancel\0");
            int byteSent = LoginWindow.client.socket.Send(messageSent);

            ChangeInfoGrid.Visibility = Visibility.Collapsed;
            MoreOptGrid.Visibility = Visibility.Visible;
        }

        private void CancelChangePassword(object sender, RoutedEventArgs e)
        {
            byte[] messageSent = Encoding.ASCII.GetBytes("-cancel\0");
            int byteSent = LoginWindow.client.socket.Send(messageSent);

            ChangePassGrid.Visibility = Visibility.Collapsed;
            MoreOptGrid.Visibility = Visibility.Visible;
        }

        private void CancelCheckUser(object sender, RoutedEventArgs e)
        {
            byte[] messageSent = Encoding.ASCII.GetBytes("-cancel\0");
            int byteSent = LoginWindow.client.socket.Send(messageSent);

            check_usr.Text = "";
            User_search.Text = "";
            Fullname_search.Text = "";
            Birthday_search.Text = "";
            Email_search.Text = "";
            Bio_search.Text = "";
            status_search1.Text = "";
            Search_result.Visibility = Visibility.Collapsed;
            CheckUsrGrid.Visibility = Visibility.Collapsed;
            MoreOptGrid.Visibility = Visibility.Visible;
        }

        public void NoSpecialChar(object sender, TextCompositionEventArgs e)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("[^0-9a-zA-Z]");
            e.Handled = regex.IsMatch(e.Text);
        }
    }

    public class CurrentTimeViewModel : INotifyPropertyChanged
    {
        private string currTime;
        public CurrentTimeViewModel()
        {
            UpdateTime();
            UpdateName();
        }

        private void UpdateTime()
        {
            CurrentTime = "Login time: ";
            CurrentTime += DateTime.Now.ToString("g");
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string CurrentTime
        {
            get { return currTime; }
            set { currTime = value; OnPropertyChanged(); }
        }

        private string name;
        private void UpdateName()
        {
            CurrentName = LoginWindow._friend;
        }

        public string CurrentName
        {
            get { return name; }
            set { name = value; OnPropertyChanged(); }
        }
    }

    
}
