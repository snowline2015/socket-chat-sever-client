using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using Microsoft.Win32;

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

        //readonly LoginWindow login = new LoginWindow();
        string path = "";

        public void OnOpenDialog(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "All files (*.*)|*.*";
            open.FilterIndex = 1;
            open.ShowDialog();
            path = open.FileName;
        }

        public void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            //login.Show();
        }

        public void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
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
