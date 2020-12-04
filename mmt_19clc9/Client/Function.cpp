#define CLIENT_DLL
#include "Header.h"

void ShutDownAndClose(client_type& client) {
    shutdown(client.socket, SD_SEND);
    closesocket(client.socket);
    WSACleanup();
}

void Init(client_type& client) {
    struct sockaddr_in addrport;
    int iResult = 0;

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
    addrport.sin_port = htons(5000);
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
            int iResult = recv(new_client.socket, new_client.RecvMsg, DEFAULT_BUFFER_LENGTH, 0);

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
    recv(client.socket, client.RecvMsg, DEFAULT_BUFFER_LENGTH, 0);

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

void Client_Private_Chat(client_type& client) {
    std::string str;

    std::thread my_thread(Client_Thread, std::ref(client));

    while (true)
    {
        getline(std::cin, str);

        //Encryt message before send
        //sent_message = string_to_hex(sent_message);

        if (str.compare("exit") == 0) break;    // De y dong nay, sau nay se chinh lai neu client muon out chat

        int iResult = send(client.socket, str.c_str(), strlen(str.c_str()), 0);

        if (iResult <= 0) break;
    }

    client.socket = INVALID_SOCKET;

    my_thread.detach();

    std::cout << "Shutting down socket..." << std::endl;

    ShutDownAndClose(client);
}

bool Client_Send_File(client_type& client, std::string& dir) {
    std::string fName = "";
    std::string temp;
    for (int i = 0; i < dir.length(); i++) {
        if (dir[i] == '/') dir.replace(i, 1, "\\");
    }

    for (int32_t i = dir.std::string::length() - 1; i >= 0; i--) {
        if (dir[i] == '\\') {
            fName.std::string::append(dir.std::string::substr(i));
            break;
        }
    }

    std::ifstream file(dir, std::ios::in, std::ios::binary);
    if (file.fail()) {
        std::cout << "Fail to open file" << std::endl;
        return;
    }
    file.seekg(0, std::ios::end);
    int size = file.tellg();
    file.seekg(0, std::ios::beg);
    temp = std::to_string(size);

    // Send file name and file size
    send(client.socket, fName.c_str(), strlen(fName.c_str()), 0);
    int iResult = recv(client.socket, client.RecvMsg, DEFAULT_BUFFER_LENGTH, 0);
    if (iResult != SOCKET_ERROR) {
        if (strcmp(client.RecvMsg, "OK") != 0)
            return false;
    }
    else return false;
    send(client.socket, temp.c_str(), strlen(temp.c_str()), 0);
    iResult = recv(client.socket, client.RecvMsg, DEFAULT_BUFFER_LENGTH, 0);
    if (iResult != SOCKET_ERROR) {
        if (strcmp(client.RecvMsg, "OK") != 0)
            return false;
    }
    else return false;

    //Sending file processing
    while (file.tellg() != size) {
        char* buffer = new char[1025];
        file.read(buffer, DEFAULT_TRANSFER_LENGTH);
        send(client.socket, buffer, DEFAULT_TRANSFER_LENGTH, 0);
        iResult = recv(client.socket, client.RecvMsg, DEFAULT_BUFFER_LENGTH, 0);
        if (iResult != SOCKET_ERROR) {
            if (strcmp(client.RecvMsg, "OK") != 0)
                return false;
        }
        else return false;
        delete[] buffer;
    }

    return true;
}

bool Client_Receive_File(client_type& client) {

}



