#define _WINSOCK_DEPRECATED_NO_WARNINGS
#include <iostream>
#include <winsock2.h>
#include <ws2tcpip.h>
#include <string>
#include "Header.h"

int main()
{
    struct sockaddr_in addrport;
    struct sockaddr_in* server = NULL;
    SOCKET sockid = INVALID_SOCKET;
    std::string msg = "";
    std::vector<client_type> client(MAX_CLIENTS);
    int num_clients = 0;
    int temp_id = -1;
    std::thread my_thread[MAX_CLIENTS];

    std::string str;
    std::cout << "Input client ip address: ";
    getline(std::cin, str);
    const char* c = str.c_str();

    std::cout << "Intializing Winsock..." << std::endl;

    WSADATA wsaData;
    int wsOK = WSAStartup(MAKEWORD(2, 2), &wsaData);
    if (wsOK != 0) {
        std::cout << "Can't initialize winsock. Application is now exiting..." << std::endl;
        return 0;
    }

    ZeroMemory(&addrport, sizeof(addrport));
    addrport.sin_family = AF_INET;
    addrport.sin_port = htons(5000);
    addrport.sin_addr.s_addr = inet_addr(c);

    /*std::cout << "Setting up server..." << std::endl;
    getaddrinfo(IP_ADDRESS, PORT, &hints, &server);*/

    sockid = socket(PF_INET, SOCK_STREAM, 0);

    ////Setup socket options
    //setsockopt(server_socket, SOL_SOCKET, SO_REUSEADDR, &OPTION_VALUE, sizeof(int)); //Make it possible to re-bind to a port that was used within the last 2 minutes
    //setsockopt(server_socket, IPPROTO_TCP, TCP_NODELAY, &OPTION_VALUE, sizeof(int)); //Used for interactive programs


    bind(sockid, (struct sockaddr*)&addrport, sizeof(addrport));
    listen(sockid, SOMAXCONN);

    //Initialize the client list
    for (int i = 0; i < MAX_CLIENTS; i++)
    {
        client[i] = { -1, INVALID_SOCKET };
    }

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
            }

            if (client[i].socket != INVALID_SOCKET)
                num_clients++;

            //std::cout << client[i].socket << std::endl;
        }

        if (temp_id != -1)
        {
            //Send the id to that client
            std::cout << "Client #" << client[temp_id].id << " Accepted" << std::endl;
            char temp[4096] = "Ok nha";
            
            while (true) {
                int BytesSent = send(NewSockid, temp, 4096, 0);
                if (BytesSent == SOCKET_ERROR)
                    send(NewSockid, temp, 4096, 0);
                else if (BytesSent == 0) continue;
                else {
                    ZeroMemory(&temp, sizeof(temp));
                    recv(NewSockid, temp, 4096, 0);
                    str = temp;

                    if (str != "bye") {
                        std::cout << str << "\nMessage: ";
                        getline(std::cin, str);
                        send(NewSockid, str.c_str(), strlen(str.c_str()), 0);
                    }
                    else break;
                }
            }

            //Create a thread process for that client
            //my_thread[temp_id] = std::thread(process_client, std::ref(client[temp_id]), std::ref(client), std::ref(my_thread[temp_id]));
        }
        else
        {
            msg = "Server is full";
            send(NewSockid, msg.c_str(), strlen(msg.c_str()), 0);
            //std::cout << msg << std::endl;
        }
    }


    closesocket(sockid);

    for (int i = 0; i < MAX_CLIENTS; i++)
    {
        my_thread[i].detach();
        closesocket(client[i].socket);
    }

    WSACleanup();
    std::cout << "Program has ended successfully" << std::endl;

    system("pause");
    return 0;
}