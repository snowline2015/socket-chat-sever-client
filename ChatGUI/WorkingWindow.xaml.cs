using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

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

        //private readonly LoginWindow login = new LoginWindow();
        string[] pathArr = new string[10];

        public void OnOpenDialog(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Multiselect = true;
            open.Filter = "All files (*.*)|*.*";
            open.FilterIndex = 1;
            open.ShowDialog();
            int index = 0;
            foreach (string str in open.FileNames)
            {
                pathArr[index] = str;
            }
        }

        #region Some Button
        public void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }

        private void EnterPrivateChat(object sender, RoutedEventArgs e)
        {
            if (PreChatPanel.Visibility == Visibility.Visible)
                PreChatPanel.Visibility = Visibility.Collapsed;
            PrivateChatPanel.Visibility = Visibility.Visible;
            PrivateChatInfoGrid.Visibility = Visibility.Visible;
        }

        private void EnterGroupChat(object sender, RoutedEventArgs e)
        {
            if (PreChatPanel.Visibility == Visibility.Visible)
                PreChatPanel.Visibility = Visibility.Collapsed;
            GroupChatPanel.Visibility = Visibility.Visible;
            GroupChatInfoGrid.Visibility = Visibility.Visible;
        }

        private void EnterMoreOpts(object sender, RoutedEventArgs e)
        {
            if (PreChatPanel.Visibility == Visibility.Visible)
                PreChatPanel.Visibility = Visibility.Collapsed;
            MoreOptPanel.Visibility = Visibility.Visible;
        }

        public void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ConnectPrivateChat(object sender, RoutedEventArgs e)
        {
            if (PrivateChatInfoGrid.Visibility == Visibility.Visible)
                PrivateChatInfoGrid.Visibility = Visibility.Collapsed;
            PrivateChatMain.Visibility = Visibility.Visible;
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

        private void ConnectGroupChat(object sender, RoutedEventArgs e)
        {
            if (GroupChatInfoGrid.Visibility == Visibility.Visible)
                GroupChatInfoGrid.Visibility = Visibility.Collapsed;
            GroupChatMain.Visibility = Visibility.Visible;
        }
        #endregion


    }

    public class CurrentTimeViewModel : INotifyPropertyChanged
    {
        private string currTime;
        public CurrentTimeViewModel()
        {
            UpdateTime();
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
    }
}
