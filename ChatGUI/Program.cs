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
        Thread my_thread;
        public volatile bool upload_flag = false;

        public void ShutDownAndClose(client_type client)
        {
            client.socket.Shutdown(SocketShutdown.Both);
            client.socket.Close();
        }

        public void Init(client_type client, string input, ref bool isConnected)
        {

            IPAddress ip = IPAddress.Parse(input);
            IPEndPoint end = new IPEndPoint(ip, 50000);
            client.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IAsyncResult result = client.socket.BeginConnect(ip, 50000, null, null);
            result.AsyncWaitHandle.WaitOne(3000, true);

            if (client.socket.Connected)
            {
                isConnected = true;
            }
            else
            {
                isConnected = false;
            }

        }

        public void Client_Thread(client_type new_client)
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
                ShutDownAndClose(new_client);
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

                Thread my_thread = new Thread(() => Client_Thread(client));
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

        public void Client_Private_Thread(client_type client)
        {
            while (true)
            {
                if (client.socket != null && upload_flag == false)
                {
                    byte[] messageReceived = new byte[4096];
                    int byteRecv = client.socket.Receive(messageReceived);
                    string str = Encoding.ASCII.GetString(messageReceived, 0, byteRecv);
                    if (upload_flag == true)
                    {
                        byte[] messageSent = Encoding.ASCII.GetBytes("-resend\0");
                        int byteSent = client.socket.Send(messageSent);
                        continue;
                    }
                    WorkingWindow.item = str;
                    WorkingWindow.start_flag = true;
                }
            }
        }

        public void Start_Client_Private_Chat(client_type client)
        {
            my_thread = new Thread(() => Client_Private_Thread(client));
            my_thread.Start();
        }

        public void End_Client_Private_Chat(client_type client)
        {
            client.socket = null;
            my_thread.Abort();
        }

        public bool Login(client_type client, string id, string password)
        {
            byte[] messageSent = Encoding.ASCII.GetBytes("-login\0");
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

            if (Array.Equals(Encoding.ASCII.GetString(messageReceived, 0, byteRecv), "ID or Password is incorrect\0") == true)
                return false;

            return true;
        }

        public bool Upload_File(client_type client, string str)
        {

            string fName = "";

            str = str.Replace("\\", "/");

            for (int i = str.Length - 1; i >= 0; i--) {
                if (str[i] == '/')
                {
                    fName = str.Substring(i + 1);
                    break;
                }
            }


            // Send file name and file size
            byte[] messageSent = Encoding.ASCII.GetBytes(fName);
            int byteSent = client.socket.Send(messageSent);

            byte[] messageReceived = new byte[4096];
            int byteRecv = client.socket.Receive(messageReceived);

            long size = new System.IO.FileInfo(str).Length;
            messageSent = Encoding.ASCII.GetBytes(size.ToString());
            byteSent = client.socket.Send(messageSent);

            Array.Clear(messageReceived, 0, messageReceived.Length);
            byteRecv = client.socket.Receive(messageReceived);


            BinaryReader br;
            try
            {
                br = new BinaryReader(new FileStream(str, FileMode.Open));      
            }
            catch
            {
                return false;
            }


            //Sending file processing
            long sizetemp = size;

            while (sizetemp > 0)
            {
                if (sizetemp < 512)
                {
                    int tempo = (int)sizetemp;
                    byte[] buffer = new byte[tempo];
                    br.Read(buffer, 0, tempo);
                    do
                    {
                        Array.Clear(messageReceived, 0, messageReceived.Length);

                        messageSent = Encoding.ASCII.GetBytes(sizetemp.ToString());

                        while (true)
                        {
                            try
                            {
                                byteSent = client.socket.Send(messageSent);
                            }
                            catch
                            {
                                continue;
                            }
                            break;
                        }

                        byteRecv = client.socket.Receive(messageReceived);
                        Array.Clear(messageReceived, 0, messageReceived.Length);

                        while (true)
                        {
                            try
                            {
                                byteSent = client.socket.Send(buffer);
                            }
                            catch
                            {
                                continue;
                            }
                            break;
                        }

                        byteRecv = client.socket.Receive(messageReceived);

                    } while (Array.Equals(Encoding.ASCII.GetString(messageReceived, 0, byteRecv), "NO\0") == true);
                    
                    Array.Clear(buffer, 0, buffer.Length);
                    sizetemp = 0;
                }
                else
                {
                    byte[] buffer = new byte[512];
                    br.Read(buffer, 0, 512);
                    do
                    {
                        Array.Clear(messageReceived, 0, messageReceived.Length);

                        messageSent = Encoding.ASCII.GetBytes("512\0");

                        while (true)
                        {
                            try
                            {
                                byteSent = client.socket.Send(messageSent);
                            }
                            catch
                            {
                                continue;
                            }
                            break;
                        }

                        byteRecv = client.socket.Receive(messageReceived);
                        Array.Clear(messageReceived, 0, messageReceived.Length);

                        while (true)
                        {
                            try
                            {
                                byteSent = client.socket.Send(buffer);
                            }
                            catch
                            {
                                continue;
                            }
                            break;
                        }

                        byteRecv = client.socket.Receive(messageReceived);

                    } while (Array.Equals(Encoding.ASCII.GetString(messageReceived, 0, byteRecv), "NO\0") == true);

                    Array.Clear(buffer, 0, buffer.Length);
                    sizetemp -= 512;
                }
            }

            br.Close();

            messageSent = Encoding.ASCII.GetBytes("-end\0");
            byteSent = client.socket.Send(messageSent);

            Array.Clear(messageReceived, 0, messageReceived.Length);
            byteRecv = client.socket.Receive(messageReceived);

            return true;
        }

        public bool Download_File(client_type client)
        {
            return true;
        }

        public void Check_Users_Online(client_type client, ref string[] client_array) 
        {
            byte[] messageSent = Encoding.ASCII.GetBytes("-check-users\0");
            int byteSent = client.socket.Send(messageSent);

            byte[] messageReceived = new byte[4096];
            int byteRecv = client.socket.Receive(messageReceived);

            string temp = System.Text.Encoding.UTF8.GetString(messageReceived, 0, messageReceived.Length);
            client_array = temp.Split('\n');
            
        }
    }
}
