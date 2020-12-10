using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;

//#define DEFAULT_BUFFER_LENGTH 4096
//#define DEFAULT_SENDER_BUFFER_SIZE 8193
//#define DEFAULT_SENDER_BUFFER_LENGTH 8192
//#define DEFAULT_RECEIVER_BUFFER_SIZE 8193
//#define DEFAULT_RECEIVER_BUFFER_LENGTH 8192

namespace ConvertedCode
{
    class client_type
    {
        public Socket socket = null;
        public int id = -1;
        public string IP, Username, Password, DOB, Email;
    }

    class Client
    {
        void ShutDownAndClose(ref client_type client)
        {
            client.socket.Shutdown(SocketShutdown.Both);
            client.socket.Close();
        }

        void Init(ref client_type client)
        {
            Console.WriteLine("Input server ip address: ");
            string input = Console.ReadLine();
            IPAddress ip = IPAddress.Parse(input);
            IPEndPoint end = new IPEndPoint(ip, 5000);
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            sock.Connect(end);
        }

        public void Client_Thread(ref client_type new_client)
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

        void Client_Group_Chat(ref client_type client)
        {
            string str;
            byte[] messageReceived = new byte[4096];
            int byteRecv = client.socket.Receive(messageReceived);
            string temp = Encoding.ASCII.GetString(messageReceived, 0, byteRecv);

            if (temp.Equals("Server is full"))
            {

                Thread my_thread = new Thread(new ThreadStart(Client_Thread(ref client)));
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


    }

    class Program
    {
        static void Main()
        {
            //Co gi cu code het vao day xong khi nao xong cai ui se dua vao MainWindow.xaml.cs
        }
    }
}
