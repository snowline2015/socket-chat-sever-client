using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using System.Threading.Tasks;


namespace ConvertedCode
{
    public class client_type
    {
        public Socket socket = null;
        public string Username, Password, DOB, Email;
    }

    public class Client
    {
        public void ShutDownAndClose(ref client_type client)
        {
            client.socket.Shutdown(SocketShutdown.Both);
            client.socket.Close();
        }

        public void Init(ref client_type client)
        {
            Console.WriteLine("Input server ip address: ");
            string input = Console.ReadLine();

            IPAddress ip = IPAddress.Parse(input);
            IPEndPoint end = new IPEndPoint(ip, 50000);
            client.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            bool isConnected = false;
            while (!isConnected)
            {
                try
                {
                    client.socket.Connect(end);
                    isConnected = true;
                }
                catch (Exception e1)
                {
                    System.Threading.Thread.Sleep(2000);
                }
            }
        }

        public void Client_Thread(ref client_type new_client)
        {
            try
            {
                while (true)
                {
                    if (new_client.socket != null)
                    {
                        byte[] messageReceived = new byte[4096];
                        int byteRecv = new_client.socket.Receive(messageReceived);
                        Console.WriteLine(Encoding.ASCII.GetString(messageReceived, 0, byteRecv));
                    }
                }
            }
            catch
            {
                ShutDownAndClose(ref new_client);
            }
        }

        public void Client_Group_Chat(client_type client)
        {
            string str;
            byte[] messageReceived = new byte[4096];
            int byteRecv = client.socket.Receive(messageReceived);
            str = Encoding.ASCII.GetString(messageReceived, 0, byteRecv);

            if (!str.Equals("Server is full"))
            {

                Thread my_thread = new Thread(() => Client_Thread(ref client));
                my_thread.IsBackground = true;
                my_thread.Start();

                while (true)
                {
                    str = Console.ReadLine();

                    //Encryt message before send
                    //sent_message = string_to_hex(sent_message);

                    if (str.Equals("exit")) break;    // De y dong nay, sau nay se chinh lai neu client muon out group chat

                    byte[] messageSent = Encoding.ASCII.GetBytes(str);
                    int byteSent = client.socket.Send(messageSent);
                }

                client.socket = null;

                my_thread.Abort();
            }
        }

        public void Client_Private_Thread(client_type new_client)
        {
            //try { }
            while (true)
            {
                if (new_client.socket != null)
                {
                    byte[] messageReceived = new byte[4096];
                    int byteRecv = new_client.socket.Receive(messageReceived);
                    string str = Encoding.ASCII.GetString(messageReceived, 0, byteRecv);
                    Console.WriteLine(str);
                }
            }
        }

        public void Client_Private_Chat(client_type client)
        {
            string str;

            Thread my_thread = new Thread(() => Client_Private_Thread(client));
            my_thread.IsBackground = true;
            my_thread.Start();

            while (true)
            {
                str = Console.ReadLine();

                //Encryt message before send
                //sent_message = string_to_hex(sent_message);

                if (str.Equals("exit")) break;    // De y dong nay, sau nay se chinh lai neu client muon out group chat

                byte[] messageSent = Encoding.ASCII.GetBytes(str);
                int byteSent = client.socket.Send(messageSent);
            }

            client.socket = null;

            my_thread.Abort();
        }

        //void Send_File(client_type client)
        //{
        //    string str = "send file";
        //    byte[] messageSent = Encoding.ASCII.GetBytes(str);
        //    int byteSent = client.socket.Send(messageSent);

        //    // Do something to take path file

        //    // str = path file

        //    //Path file taken

        //    string fName = "";

        //    for (int i = 0; i < str.Length; i++)
        //    {
        //        if (str[i] == '/') str.Replace('/', '\\');
        //    }

        //    for (int i = str.Length - 1; i >= 0; i--) {
        //        if (str[i] == '\\')
        //        {
        //            fName += (str.Substring(i + 1));
        //            break;
        //        }
        //    }

        //    if (!File.Exists(fName))
        //    {
        //        // Warn cannot open file


        //        return;
        //    }


        //    // Send file name and file size

        //    long length = new System.IO.FileInfo(fName).Length;

        //    Array.Clear(messageSent, 0, messageSent.Length);
        //    messageSent = Encoding.ASCII.GetBytes(fName);
        //    byteSent = client.socket.Send(messageSent);

        //    Array.Clear(messageSent, 0, messageSent.Length);
        //    messageSent = BitConverter.GetBytes(length);
        //    byteSent = client.socket.Send(messageSent);


        //    BinaryReader br;
        //    try
        //    {
        //        br = new BinaryReader(new FileStream(str, FileMode.Open, FileAccess.Read));
        //        while (br.BaseStream.Position != br.BaseStream.Length) {
        //            byte[] fileSent = new byte[8196];
        //            fileSent = br.ReadBytes(8196);
        //            int fileByteSent = client.socket.Send(messageSent);
        //        }

        //        br.Close();
        //        string temp = "end";
        //        messageSent = Encoding.ASCII.GetBytes(temp);
        //        byteSent = client.socket.Send(messageSent); 
        //    }
        //    catch (IOException e)
        //    {
        //        Console.WriteLine(e.Message);
        //        return;
        //    }
        //}

    }
    class Program
    {
        static void Main()
        {
            //Co gi cu code het vao day xong khi nao xong cai ui se dua vao MainWindow.xaml.cs
            client_type cl = new client_type();
            Client client = new Client();
            client.Init(ref cl);
        }
    }
}
