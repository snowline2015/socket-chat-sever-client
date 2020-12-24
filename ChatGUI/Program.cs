using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ChatGUI;

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

        public void Init(ref client_type client, string input, ref bool isConnected)
        {

            IPAddress ip = IPAddress.Parse(input);
            IPEndPoint end = new IPEndPoint(ip, 50000);
            client.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            isConnected = false;
            while (!isConnected)
            {
                try
                {
                    client.socket.Connect(end);
                    isConnected = true;
                    
                }
                catch
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
            while (true)
            {
                if (new_client.socket != null)
                {
                    byte[] messageReceived = new byte[4096];
                    int byteRecv = new_client.socket.Receive(messageReceived);
                    string str = Encoding.ASCII.GetString(messageReceived, 0, byteRecv);
                    WorkingWindow.AddListboxItems(str);
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

                if (str.Equals("exit\0")) break;    // De y dong nay, sau nay se chinh lai neu client muon out group chat

                byte[] messageSent = Encoding.ASCII.GetBytes(str);
                int byteSent = client.socket.Send(messageSent);
            }

            client.socket = null;

            my_thread.Abort();
        }

        public bool Login(client_type client, string id, string password)
        {
            byte[] messageSent = Encoding.ASCII.GetBytes("login\0");
            int byteSent = client.socket.Send(messageSent);

            byte[] messageReceived = new byte[4096];
            int byteRecv = client.socket.Receive(messageReceived);
            //string str = Encoding.ASCII.GetString(messageReceived, 0, byteRecv);


            if (Array.Equals(Encoding.ASCII.GetString(messageReceived, 0, byteRecv), "OK\0") == false)
                return false;


            messageSent = Encoding.ASCII.GetBytes(id);
            byteSent = client.socket.Send(messageSent);

            Array.Clear(messageReceived, 0, messageReceived.Length);
            byteRecv = client.socket.Receive(messageReceived);

            if (Array.Equals(Encoding.ASCII.GetString(messageReceived, 0, byteRecv), "OK\0") == false)
                return false;

            messageSent = Encoding.ASCII.GetBytes(password);
            byteSent = client.socket.Send(messageSent);

            while (true)
            {
                Array.Clear(messageReceived, 0, messageReceived.Length);
                byteRecv = client.socket.Receive(messageReceived);
                if (byteRecv <= 0) continue;
                else break;
            }

            if (Array.Equals(Encoding.ASCII.GetString(messageReceived, 0, byteRecv), "ID or Password is incorrect") == true)
                return false;

            return true;
        }

    }
}
