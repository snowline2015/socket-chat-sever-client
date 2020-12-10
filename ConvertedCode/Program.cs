using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

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
        public string Username, Password, DOB, Email;
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
            client.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            try
            {
                client.socket.Connect(end);
            }
            catch
            {
                return;
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

        void Client_Group_Chat(client_type client)
        {
            string str;
            byte[] messageReceived = new byte[4096];
            int byteRecv = client.socket.Receive(messageReceived);
            string temp = Encoding.ASCII.GetString(messageReceived, 0, byteRecv);

            if ((String.Compare(temp, "Server is full", comparisonType: StringComparison.OrdinalIgnoreCase) != 0))
            {

                Thread my_thread = new Thread(()=> Client_Thread(ref client));
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

        void Client_Private_Thread(client_type new_client)
        {
            //try { }
            while (true)
            {
                if (new_client.socket != null)
                {
                    byte[] messageReceived = new byte[4096];
                    int byteRecv = new_client.socket.Receive(messageReceived);
                    string str = Encoding.ASCII.GetString(messageReceived, 0, byteRecv);

                    if (str.Equals("send file"))
                    {
                        string dest_temp = "C:\\Users\\snowl\\Desktop"; // OpenFileDialog

                        //Receive file name and size
                        messageReceived = new byte[4096];
                        byteRecv = new_client.socket.Receive(messageReceived);
                        str = Encoding.ASCII.GetString(messageReceived, 0, byteRecv);
                        dest_temp += ("\\"  + str);

                        messageReceived = new byte[4096];
                        byteRecv = new_client.socket.Receive(messageReceived);
                        str = Encoding.ASCII.GetString(messageReceived, 0, byteRecv);
                        ulong size = ulong.Parse(str);


                        /*cout << "Do you want download file? (Y/N): ";
                        char c = _getch();
                        if (c == 'y' || c == 'Y') {
                            fflush(stdin);
                        }
                        else return false;*/


                        //Receive buffer processing

                        BinaryWriter bw;
                        try
                        {
                            bw = new BinaryWriter(new FileStream(dest_temp, FileMode.OpenOrCreate, FileAccess.ReadWrite));

                            while (true)
                            {
                                byte[] FileReceived = new byte[8192];
                                byteRecv = new_client.socket.Receive(FileReceived);
                                str = Encoding.ASCII.GetString(FileReceived, 0, byteRecv);
                                if (str.Equals("end"))
                                {
                                    bw.Close();
                                    break;
                                }
                                bw.Write(FileReceived);
                            }
                        }
                        catch (IOException e)
                        {
                            Console.WriteLine(e.Message + "\n Cannot write to file.");
                            return;
                        }
                    }
                }
            }
        }

        void Client_Private_Chat(client_type client)
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

                if (str.compare("send file") == 0)
                {
                    std::cout << "File path: ";
                    std::getline(std::cin, str);

                    std::string fName = "";

                    for (int i = 0; i < str.length(); i++)
                    {
                        if (str[i] == '/') str.replace(i, 1, "\\");
                    }

                    for (int32_t i = str.std::string::length() - 1; i >= 0; i--) {
                        if (str[i] == '\\')
                        {
                            fName.std::string::append(str.std::string::substr(i + 1));
                            break;
                        }
                    }

                    /*FILE* fp = fopen(str.c_str(), "rb");
                    if (fp == NULL) {
                        std::cout << "Fail to open file" << std::endl;
                        continue;
                    }

                    fseek(fp, 0, SEEK_END);
                    long long int size = ftell(fp);
                    fseek(fp, -size, SEEK_END);*/


                    std::ifstream fp(str, ios::in | ios::binary);

                    if (fp.fail())
                    {
                        continue;
                    }

                    fp.seekg(0, ios::end);
                    long long int size = fp.tellg();
                    fp.seekg(0, ios::beg);



                    // Send file name and file size
                    send(client.socket, fName.c_str(), strlen(fName.c_str()), 0);
                    send(client.socket, std::to_string(size).c_str(), strlen(std::to_string(size).c_str()), 0);


                    //Sending file processing
                    while (!fp.eof())
                    {
                        char* buffer = new char[DEFAULT_SENDER_BUFFER_SIZE];
                        fp.read(buffer, DEFAULT_SENDER_BUFFER_LENGTH);
                        send(client.socket, buffer, DEFAULT_SENDER_BUFFER_LENGTH, 0);
                        delete[] buffer;
                    }
                    fp.close();
                    send(client.socket, "end", 4, 0);
                }
            }

            client.socket = INVALID_SOCKET;

            my_thread.detach();

            std::cout << "Shutting down socket..." << std::endl;

            ShutDownAndClose(client);

        }

    class Program
    {
        static void Main()
        {
            //Co gi cu code het vao day xong khi nao xong cai ui se dua vao MainWindow.xaml.cs
        }
    }
}
