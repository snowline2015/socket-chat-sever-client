#include "Header.h"

int main()
{
    struct sockaddr_in addrport;
    SOCKET sockid = INVALID_SOCKET;
    std::string msg = "", username, tempo;
    std::vector<client_type> client(MAX_CLIENTS), client_List;
    int num_clients = 0;
    int temp_id = -1;
    std::thread my_thread[MAX_CLIENTS], temp_thread[MAX_CLIENTS];

    Read_Account(client_List);

    std::cout << "Intializing Winsock..." << std::endl;

    WSADATA wsaData;
    int wsOK = WSAStartup(MAKEWORD(2, 2), &wsaData);
    if (wsOK != 0) {
        std::cout << "Can't initialize winsock. Application is now exiting..." << std::endl;
        return 0;
    }
    std::cout << "Server started..." << std::endl;

    ZeroMemory(&addrport, sizeof(addrport));
    addrport.sin_family = AF_INET;
    addrport.sin_port = htons(50000);
    addrport.sin_addr.s_addr = htonl(INADDR_ANY);// inet_addr(c);

    sockid = socket(PF_INET, SOCK_STREAM, 0);
    bind(sockid, (struct sockaddr*)&addrport, sizeof(addrport));
    listen(sockid, SOMAXCONN);

    while (true)
    {
        SOCKET NewSockid = INVALID_SOCKET;
        NewSockid = accept(sockid, NULL, NULL);

        if (NewSockid == INVALID_SOCKET) continue;

        //Reset the number of clients
        num_clients = -1;

        //Create a temporary id for the next client
        temp_id = -1;
        for (int i = 0; i < MAX_CLIENTS; i++)
        {
            if (client[i].socket == INVALID_SOCKET && temp_id == -1)
            {
                client[i].socket = NewSockid;
                client[i].id = i;
                temp_id = i;
                char clientIP[16];
                int client_len = sizeof(addrport);
                getpeername(NewSockid, (struct sockaddr*)&addrport, &client_len);
                client[i].IP = inet_ntop(AF_INET, &addrport.sin_addr, clientIP, sizeof(clientIP));
                memset(&clientIP, NULL, sizeof(clientIP));
            }

            if (client[i].socket != INVALID_SOCKET)
                num_clients++;
        }

        if (temp_id != -1)
        {
            temp_thread[temp_id] = std::thread(Client_Thread, NewSockid, std::ref(client_List), std::ref(client), std::ref(my_thread), temp_id, std::ref(temp_thread[temp_id]));
        }
        else
        {
            msg = "Server is full";
            send(NewSockid, msg.c_str(), strlen(msg.c_str()), 0);
        }
    }

    closesocket(sockid);

    for (int i = 0; i < MAX_CLIENTS; i++)
    {
        temp_thread[i].detach();
        my_thread[i].detach();
        closesocket(client[i].socket);
    }

    WSACleanup();
    std::cout << "Server has ended successfully" << std::endl;

    system("pause");
    return 0;
}