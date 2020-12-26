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

        public void Start_Client_Private_Chat(client_type client)
        {
            my_thread = new Thread(() => Client_Private_Thread(client));
            my_thread.IsBackground = true;
            my_thread.Start();
        }

        public void End_Client_Private_Chat(client_type client)
        {
            client.socket = null;
            my_thread.Abort();
        }
    

        public void Client_Send(client_type client, string mess)
        {
            byte[] messageSent = Encoding.ASCII.GetBytes(mess);
            int byteSent = client.socket.Send(messageSent);
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

        public bool Upload_File(client_type client)
        {

            // take file path
            string str = "";        // file path here


            string fName = "";

            str = str.Replace("\\", "/");

            for (int i = str.Length - 1; i >= 0; i--) {
                if (str[i] == '/')
                {
                    fName = str.Substring(i + 1);
                    break;
                }
            }

            BinaryReader br;

            try
            {
                br = new BinaryReader(new FileStream(str, FileMode.Open));      // open ifstream here
            }
            catch
            {
                return false;
            }


            // Send file name and file size

            byte[] messageSent = Encoding.ASCII.GetBytes(fName);
            int byteSent = client.socket.Send(messageSent);

            byte[] messageReceived = new byte[4096];
            int byteRecv = client.socket.Receive(messageReceived);

            long size = new System.IO.FileInfo(str).Length;
            byteSent = client.socket.Send(BitConverter.GetBytes(size));

            Array.Clear(messageReceived, 0, messageReceived.Length);
            byteRecv = client.socket.Receive(messageReceived);


            //Sending file processing
            long sizetemp = size;

            while (sizetemp > 0)
            {
                if (sizetemp < 512)
                {
                    int tempo = unchecked((int)sizetemp);
                    byte[] buffer = new byte[tempo];
                    buffer = br.ReadBytes(tempo);
                    do
                    {
                        Array.Clear(messageReceived, 0, messageReceived.Length);
                        byteRecv = client.socket.Receive(messageReceived);

                        byteSent = client.socket.Send(BitConverter.GetBytes(sizetemp));
                        while (byteSent == -1)
                        {
                            Thread.Sleep(5);
                            byteSent = client.socket.Send(BitConverter.GetBytes(sizetemp));
                        }

                        Array.Clear(messageReceived, 0, messageReceived.Length);
                        byteRecv = client.socket.Receive(messageReceived);

                        byteSent = client.socket.Send(buffer);

                        while (byteSent == -1)
                        {
                            Thread.Sleep(5);
                            byteSent = client.socket.Send(buffer);
                        }

                        Array.Clear(messageReceived, 0, messageReceived.Length);
                        byteRecv = client.socket.Receive(messageReceived);

                    } while (Array.Equals(Encoding.ASCII.GetString(messageReceived, 0, byteRecv), "no\0") == true);

                    Array.Clear(buffer, 0, buffer.Length);
                    sizetemp = 0;
                }
                else
                {
                    byte[] buffer = new byte[512];
                    buffer = br.ReadBytes(512);
                    do
                    {
                        Array.Clear(messageReceived, 0, messageReceived.Length);
                        byteRecv = client.socket.Receive(messageReceived);

                        byteSent = client.socket.Send(BitConverter.GetBytes(512));
                        while (byteSent == -1)
                        {
                            Thread.Sleep(5);
                            byteSent = client.socket.Send(BitConverter.GetBytes(512));
                        }

                        Array.Clear(messageReceived, 0, messageReceived.Length);
                        byteRecv = client.socket.Receive(messageReceived);

                        byteSent = client.socket.Send(buffer);

                        while (byteSent == -1)
                        {
                            Thread.Sleep(5);
                            byteSent = client.socket.Send(buffer);
                        }

                        Array.Clear(messageReceived, 0, messageReceived.Length);
                        byteRecv = client.socket.Receive(messageReceived);

                    } while (Array.Equals(Encoding.ASCII.GetString(messageReceived, 0, byteRecv), "no\0") == true);

                    Array.Clear(buffer, 0, buffer.Length);
                    sizetemp -= 512;
                }
            }
            br.Close();

            messageSent = Encoding.ASCII.GetBytes("end\0");
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

            //for (int i = 0; i < client_array.Length; i++)
            //{
            //    if (client_array[i][0] == '\0')
            //        client_array[i] = "";
            //}
            
        }
    }
}
