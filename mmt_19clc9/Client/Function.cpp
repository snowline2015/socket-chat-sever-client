#include "Header.h"
std::atomic<bool> stop_flag(false);

void ShutDownAndClose(client_type& client) {
    shutdown(client.socket, SD_SEND);
    closesocket(client.socket);
    WSACleanup();
}

void Init(client_type& client) {
    int iResult = 0;
    struct sockaddr_in addrport;
    std::string str;
    std::cout << "Input server ip address: ";
    getline(std::cin, str);
    const char* c = str.c_str();

    WSADATA wsaData;
    int wsOK = WSAStartup(MAKEWORD(2, 2), &wsaData);
    if (wsOK != 0) {
        //std::cout << "Can't initialize winsock. Application is now exiting..." << std::endl;
        return;
    }

    ZeroMemory(&addrport, sizeof(addrport));
    addrport.sin_family = AF_INET;
    addrport.sin_port = htons(50000);
    addrport.sin_addr.s_addr = inet_addr(c);


    /*std::cout << "Setting up server..." << std::endl;
    getaddrinfo(IP_ADDRESS, PORT, &hints, &server);*/


    // Attempt to connect to an address until one succeeds
    while (true) {
        client.socket = socket(PF_INET, SOCK_STREAM, 0);
        iResult = connect(client.socket, (struct sockaddr*)&addrport, sizeof(addrport));
        if (iResult == SOCKET_ERROR) {
            closesocket(client.socket);
            client.socket = INVALID_SOCKET;
            continue;
        }
        break;
    }

    if (client.socket == INVALID_SOCKET) {
        ShutDownAndClose(client);
        system("pause");
        return;
    }
}

void Client_Thread(client_type& new_client) {
    while (true) {
        memset(new_client.RecvMsg, NULL, sizeof(new_client.RecvMsg));

        if (new_client.socket != 0) {
            int iResult = recv(new_client.socket, new_client.RecvMsg, DEFAULT_MSG_LENGTH, 0);

            if (iResult != SOCKET_ERROR)
                std::cout << new_client.RecvMsg << std::endl;

            else break;   
            
        }
    }

    if (WSAGetLastError() == WSAECONNRESET)
        std::cout << "The server has disconnected" << std::endl;
}

void Client_Group_Chat(client_type& client) {
    std::string str;
    recv(client.socket, client.RecvMsg, DEFAULT_MSG_LENGTH, 0);

    if (strcmp(client.RecvMsg, "Server is full") != 0)
    {

        std::thread my_thread(Client_Thread, std::ref(client));

        while (true)
        {
            getline(std::cin, str);

            //Encryt message before send
            //sent_message = string_to_hex(sent_message);



            if (str.compare("exit") == 0) break;    // De y dong nay, sau nay se chinh lai neu client muon out group chat

            int iResult = send(client.socket, str.c_str(), strlen(str.c_str()), 0);

            if (iResult <= 0) break;
        }

        client.socket = INVALID_SOCKET;

        my_thread.detach();
    }

    else
        std::cout << client.RecvMsg << std::endl;

    std::cout << "Shutting down socket..." << std::endl;

    ShutDownAndClose(client);
}

void Client_Private_Thread(client_type& new_client) {
    while (true) {
        if (stop_flag.load()) {
            sleep_for(milliseconds(500));
            continue;
        }
        else if (new_client.socket != 0) {
            memset(new_client.RecvMsg, NULL, sizeof(new_client.RecvMsg));
            int iResult = recv(new_client.socket, new_client.RecvMsg, DEFAULT_MSG_LENGTH, 0);
            if (iResult != SOCKET_ERROR) {
                sleep_for(milliseconds(150));
                if (stop_flag.load() == false)
                    std::cout << new_client.RecvMsg << std::endl;
                else
                    send(new_client.socket, "resend", 7, 0);
            }
            else break;
        }
        else break;
    }
}

void Client_Private_Chat(client_type& client) {
    std::string str;
    std::thread my_thread(Client_Private_Thread, std::ref(client));

    while (true)
    {
        getline(std::cin, str);

        //Encryt message before send
        //sent_message = string_to_hex(sent_message);

        if (str.compare("exit") == 0) break;    // De y dong nay, sau nay se chinh lai neu client muon out chat

        int iResult = send(client.socket, str.c_str(), strlen(str.c_str()), 0);

        if (iResult <= 0) break;

        if (str.compare("upload file") == 0) {

            //Stop thread
            stop_flag.store(true);

            Upload_File(client);

            // restart thread
            stop_flag.store(false);
        }
    }

    my_thread.detach();

    std::cout << "Shutting down socket..." << std::endl;

    ShutDownAndClose(client);
}

void Upload_File(client_type& client) {
    std::string str;
    int iResult;
    std::cout << "File path: ";
    std::getline(std::cin, str);

    std::string fName = "";

    for (int i = 0; i < str.length(); i++) {
        if (str[i] == '/') str.replace(i, 1, "\\");
    }

    for (int32_t i = str.std::string::length() - 1; i >= 0; i--) {
        if (str[i] == '\\') {
            fName.std::string::append(str.std::string::substr(i + 1));
            break;
        }
    }

    std::ifstream fp;
    fp.open(str, ios::binary);

    if (fp.fail()) {
        return;
    }

    // Send file name and file size
    fp.seekg(0, ios::end);
    long long int size = fp.tellg();
    fp.seekg(0, ios::beg);

    send(client.socket, fName.c_str(), strlen(fName.c_str()), 0);
    recv(client.socket, client.RecvMsg, DEFAULT_MSG_LENGTH, 0);
    send(client.socket, std::to_string(size).c_str(), strlen(std::to_string(size).c_str()), 0);
    recv(client.socket, client.RecvMsg, DEFAULT_MSG_LENGTH, 0);


    //Sending file processing
    long long int sizetemp = size;

    while (sizetemp > 0) {
        if (sizetemp < DEFAULT_BUFFER_SIZE) {
            char* buffer = new char[sizetemp];
            fp.read(buffer, sizetemp);
            do {
                memset(&client.RecvMsg, NULL, sizeof(client.RecvMsg));
                iResult = send(client.socket, std::to_string(sizetemp).c_str(), strlen(std::to_string(sizetemp).c_str()), 0);
                while (iResult == SOCKET_ERROR) {
                    sleep_for(milliseconds(5));
                    iResult = send(client.socket, std::to_string(sizetemp).c_str(), strlen(std::to_string(sizetemp).c_str()), 0);
                }
                recv(client.socket, client.RecvMsg, DEFAULT_MSG_LENGTH, 0);
                memset(&client.RecvMsg, NULL, sizeof(client.RecvMsg));
                iResult = send(client.socket, buffer, sizetemp, 0);
                while (iResult == SOCKET_ERROR) {
                    sleep_for(milliseconds(5));
                    iResult = send(client.socket, buffer, sizetemp, 0);
                }
                recv(client.socket, client.RecvMsg, DEFAULT_MSG_LENGTH, 0);
            } while (strcmp(client.RecvMsg, "no") == 0);
            delete[] buffer;
            sizetemp = 0;
        }
        else {
            char* buffer = new char[DEFAULT_BUFFER_SIZE];
            fp.read(buffer, DEFAULT_BUFFER_SIZE);
            do {
                memset(&client.RecvMsg, NULL, sizeof(client.RecvMsg));
                iResult = send(client.socket, std::to_string(DEFAULT_BUFFER_SIZE).c_str(), strlen(std::to_string(DEFAULT_BUFFER_SIZE).c_str()), 0);
                while (iResult == SOCKET_ERROR) {
                    sleep_for(milliseconds(5));
                    iResult = send(client.socket, std::to_string(DEFAULT_BUFFER_SIZE).c_str(), strlen(std::to_string(DEFAULT_BUFFER_SIZE).c_str()), 0);
                }
                recv(client.socket, client.RecvMsg, DEFAULT_MSG_LENGTH, 0);
                memset(&client.RecvMsg, NULL, sizeof(client.RecvMsg));
                iResult = send(client.socket, buffer, DEFAULT_BUFFER_SIZE, 0);
                while (iResult == SOCKET_ERROR) {
                    sleep_for(milliseconds(5));
                    iResult = send(client.socket, buffer, DEFAULT_BUFFER_SIZE, 0);
                }
                recv(client.socket, client.RecvMsg, DEFAULT_MSG_LENGTH, 0);
            } while (strcmp(client.RecvMsg, "no") == 0);
            delete[] buffer;
            sizetemp -= DEFAULT_BUFFER_SIZE;
        }
    }
    fp.close();
    std::cout << "Upload completed" << std::endl;
    iResult = send(client.socket, "end", 4, 0);
    recv(client.socket, client.RecvMsg, DEFAULT_MSG_LENGTH, 0);
}
