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
using Microsoft.Win32;

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

        public void Client_Group_Thread(client_type client)
        {
            try
            {
                while (true)
                {
                    if (client.socket != null)
                    {
                        byte[] messageReceived = new byte[4096];
                        int byteRecv = client.socket.Receive(messageReceived);

                        string str = Encoding.ASCII.GetString(messageReceived, 0, byteRecv);
                        WorkingWindow.item = str;
                        WorkingWindow.start_flag = true;     //  Add listbox items
                    }
                }
            }
            catch
            {
                ShutDownAndClose(client);
            }
        }

        public void Start_Client_Group_Chat(client_type client)
        {
            WorkingWindow.logout_flag = false;
            my_thread = new Thread(() => Client_Group_Thread(client));
            my_thread.Start();
        }

        public void End_Client_Group_Chat(client_type client)
        {
            my_thread.Abort();
        }

        public void Client_Private_Thread(client_type client)
        {
            while (true)
            {
                if (client.socket != null && upload_flag == false)
                {
                    byte[] messageReceived = new byte[4096];
                    int byteRecv = client.socket.Receive(messageReceived);
                    
                    if (upload_flag == true)
                    {
                        byte[] messageSent = Encoding.ASCII.GetBytes("-resend\0");
                        int byteSent = client.socket.Send(messageSent);
                        continue;
                    }
                    else if (Array.Equals(Encoding.ASCII.GetString(messageReceived, 0, byteRecv), "-download-file\0") == true)
                    {
                        byte[] messageSent = Encoding.ASCII.GetBytes("OK\0");
                        int byteSent = client.socket.Send(messageSent);
                        byteSent = client.socket.Send(messageSent);
                        Download_File(client);
                    }
                    else if (Array.Equals(Encoding.ASCII.GetString(messageReceived, 0, byteRecv), "-disconnect\0") == true)
                    {
                        WorkingWindow.item = "-disconnect";
                        WorkingWindow.start_flag = true;
                        break;
                    }
                    else
                    {
                        string str = Encoding.ASCII.GetString(messageReceived, 0, byteRecv);
                        WorkingWindow.item = str;
                        WorkingWindow.start_flag = true;
                    }
                }
            }
        }

        public void Start_Client_Private_Chat(client_type client)
        {
            WorkingWindow.logout_flag = false;
            my_thread = new Thread(() => Client_Private_Thread(client));
            my_thread.Start();
        }

        public void End_Client_Private_Chat(client_type client)
        {
            my_thread.Abort();
        }

        public bool Login(client_type client, string id, string password)
        {
            byte[] messageSent = Encoding.ASCII.GetBytes("-login\0");
            int byteSent = client.socket.Send(messageSent);

            byte[] messageReceived = new byte[4096];
            int byteRecv = client.socket.Receive(messageReceived);

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

            Array.Clear(messageReceived, 0, messageReceived.Length);
            byteRecv = client.socket.Receive(messageReceived);

            if (Array.Equals(Encoding.ASCII.GetString(messageReceived, 0, byteRecv), "NO\0") == true)
                return false;

            return true;
        }

        public bool Register(client_type client, string id, string password, string fullname, string dob, string email)
        {
            byte[] messageSent = Encoding.ASCII.GetBytes("-register\0");
            int byteSent = client.socket.Send(messageSent);

            byte[] messageReceived = new byte[4096];
            int byteRecv = client.socket.Receive(messageReceived);

            if (Array.Equals(Encoding.ASCII.GetString(messageReceived, 0, byteRecv), "OK\0") == false)
                return false;

            messageSent = Encoding.ASCII.GetBytes(id);
            byteSent = client.socket.Send(messageSent);

            Array.Clear(messageReceived, 0, messageReceived.Length);
            byteRecv = client.socket.Receive(messageReceived);

            if (Array.Equals(Encoding.ASCII.GetString(messageReceived, 0, byteRecv), "NO\0") == true)
            {
                // Tao warning   = "Username already taken !"
                return false;
            }

            messageSent = Encoding.ASCII.GetBytes(password);
            byteSent = client.socket.Send(messageSent);

            Array.Clear(messageReceived, 0, messageReceived.Length);
            byteRecv = client.socket.Receive(messageReceived);

            if (Array.Equals(Encoding.ASCII.GetString(messageReceived, 0, byteRecv), "OK\0") == false)
                return false;

            messageSent = Encoding.ASCII.GetBytes(fullname);
            byteSent = client.socket.Send(messageSent);

            Array.Clear(messageReceived, 0, messageReceived.Length);
            byteRecv = client.socket.Receive(messageReceived);

            if (Array.Equals(Encoding.ASCII.GetString(messageReceived, 0, byteRecv), "OK\0") == false)
                return false;

            messageSent = Encoding.ASCII.GetBytes(dob);
            byteSent = client.socket.Send(messageSent);

            Array.Clear(messageReceived, 0, messageReceived.Length);
            byteRecv = client.socket.Receive(messageReceived);

            if (Array.Equals(Encoding.ASCII.GetString(messageReceived, 0, byteRecv), "OK\0") == false)
                return false;

            messageSent = Encoding.ASCII.GetBytes(email);
            byteSent = client.socket.Send(messageSent);

            Array.Clear(messageReceived, 0, messageReceived.Length);
            byteRecv = client.socket.Receive(messageReceived);

            if (Array.Equals(Encoding.ASCII.GetString(messageReceived, 0, byteRecv), "OK\0") == false)
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
            br.Dispose();

            messageSent = Encoding.ASCII.GetBytes("-end\0");
            byteSent = client.socket.Send(messageSent);

            Array.Clear(messageReceived, 0, messageReceived.Length);
            byteRecv = client.socket.Receive(messageReceived);

            return true;
        }

        public bool Download_File(client_type client)
        {
            // Receive file name and file size
            byte[] messageReceived = new byte[4096];
            int byteRecv = client.socket.Receive(messageReceived);
            string fName = Encoding.ASCII.GetString(messageReceived, 0, messageReceived.Length);

            byte[] messageSent = Encoding.ASCII.GetBytes("OK\0");
            int byteSent = client.socket.Send(messageSent);

            Array.Clear(messageReceived, 0, messageReceived.Length);
            byteRecv = client.socket.Receive(messageReceived);
            long fSize = long.Parse(Encoding.ASCII.GetString(messageReceived, 0, messageReceived.Length));

            byteSent = client.socket.Send(messageSent);

            // save file dialog here //
            
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Title = "Save as";
            saveFileDialog1.Filter = "All files (*.*)|*.*";
            saveFileDialog1.FileName = fName;
            saveFileDialog1.ShowDialog();

            BinaryWriter bw;
            try
            {
                bw = new BinaryWriter(File.Open(saveFileDialog1.FileName, FileMode.Create));       // Folder name to add file here
            }
            catch
            {
                return false;
            }

            while (true)
            {
                Array.Clear(messageReceived, 0, messageReceived.Length);
                byte[] buffer = new byte[512];

                byteRecv = client.socket.Receive(messageReceived);

                if (Array.Equals(Encoding.ASCII.GetString(messageReceived, 0, byteRecv), "-end\0") == true)
                {
                    bw.Close();
                    bw.Dispose();

                    Array.Clear(buffer, 0, buffer.Length);
                    messageSent = Encoding.ASCII.GetBytes("OK\0");
                    byteSent = client.socket.Send(messageSent);
                    break;
                }

                int buffersize = Int32.Parse(Encoding.ASCII.GetString(messageReceived, 0, byteRecv));
                messageSent = Encoding.ASCII.GetBytes("OK\0");
                byteSent = client.socket.Send(messageSent);

                try
                {
                    byteRecv = client.socket.Receive(buffer);
                }
                catch
                {
                    break;
                }

                if (byteRecv != buffersize)
                {
                    messageSent = Encoding.ASCII.GetBytes("NO\0");
                    byteSent = client.socket.Send(messageSent);
                }

                else if (byteRecv < 512)
                {
                    messageSent = Encoding.ASCII.GetBytes("OK\0");
                    byteSent = client.socket.Send(messageSent);
                    byte[] buffer2 = new byte[byteRecv];
                    Array.Copy(buffer, buffer2, byteRecv);
                    bw.Write(buffer2);
                    Array.Clear(buffer2, 0, buffer2.Length);
                }

                else
                {
                    messageSent = Encoding.ASCII.GetBytes("OK\0");
                    byteSent = client.socket.Send(messageSent);
                    bw.Write(buffer);
                }
                Array.Clear(buffer, 0, buffer.Length);
            }

            return true;
        }

        public void Check_Users_Online(client_type client, ref string[] client_array) 
        {
            byte[] messageSent = Encoding.ASCII.GetBytes("-check-users\0");
            int byteSent = client.socket.Send(messageSent);

            byte[] messageReceived = new byte[4096];
            int byteRecv = client.socket.Receive(messageReceived);

            string temp = Encoding.ASCII.GetString(messageReceived, 0, messageReceived.Length);
            client_array = temp.Split('\n');  
        }

        public void Check_User_Info(client_type client, string username, ref string[] user_info)
        {
            Array.Clear(user_info, 0, user_info.Length);
            byte[] messageSent = Encoding.ASCII.GetBytes("-check-user\0");
            int byteSent = client.socket.Send(messageSent);

            byte[] messageReceived = new byte[4096];
            int byteRecv = client.socket.Receive(messageReceived);

            messageSent = Encoding.ASCII.GetBytes(username);     
            byteSent = client.socket.Send(messageSent);

            Array.Clear(messageReceived, 0, messageReceived.Length);
            byteRecv = client.socket.Receive(messageReceived);

            if (Array.Equals(Encoding.ASCII.GetString(messageReceived, 0, byteRecv), "NO\0") == true)
            {
                // Lam gi do khi khong tim thay user
            }

            string temp = Encoding.ASCII.GetString(messageReceived, 0, messageReceived.Length);
            user_info = temp.Split('\n');                   // user_info[last_index] = online / offline
        }

        public void Change_Password(client_type client, string username, string new_password)
        {
            byte[] messageSent = Encoding.ASCII.GetBytes("-change-password\0");
            int byteSent = client.socket.Send(messageSent);

            byte[] messageReceived = new byte[4096];
            int byteRecv = client.socket.Receive(messageReceived);

            messageSent = Encoding.ASCII.GetBytes(username);
            byteSent = client.socket.Send(messageSent);

            Array.Clear(messageReceived, 0, messageReceived.Length);
            byteRecv = client.socket.Receive(messageReceived);

            messageSent = Encoding.ASCII.GetBytes(new_password);
            byteSent = client.socket.Send(messageSent);

            Array.Clear(messageReceived, 0, messageReceived.Length);
            byteRecv = client.socket.Receive(messageReceived);
        }

        public void Change_Info(client_type client, string username, string fullname, string dob, string email, string bio)
        {
            byte[] messageSent = Encoding.ASCII.GetBytes("-change-info\0");
            int byteSent = client.socket.Send(messageSent);

            byte[] messageReceived = new byte[4096];
            int byteRecv = client.socket.Receive(messageReceived);

            messageSent = Encoding.ASCII.GetBytes(username);
            byteSent = client.socket.Send(messageSent);

            Array.Clear(messageReceived, 0, messageReceived.Length);
            byteRecv = client.socket.Receive(messageReceived);

            messageSent = Encoding.ASCII.GetBytes(fullname);
            byteSent = client.socket.Send(messageSent);

            Array.Clear(messageReceived, 0, messageReceived.Length);
            byteRecv = client.socket.Receive(messageReceived);

            messageSent = Encoding.ASCII.GetBytes(dob);
            byteSent = client.socket.Send(messageSent);

            Array.Clear(messageReceived, 0, messageReceived.Length);
            byteRecv = client.socket.Receive(messageReceived);

            messageSent = Encoding.ASCII.GetBytes(email);
            byteSent = client.socket.Send(messageSent);

            Array.Clear(messageReceived, 0, messageReceived.Length);
            byteRecv = client.socket.Receive(messageReceived);

            messageSent = Encoding.ASCII.GetBytes(bio);
            byteSent = client.socket.Send(messageSent);

            Array.Clear(messageReceived, 0, messageReceived.Length);
            byteRecv = client.socket.Receive(messageReceived);
        }
    }
}
