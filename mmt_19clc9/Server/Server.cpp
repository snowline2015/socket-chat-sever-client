#include "Header.h"

int main()
{
    struct sockaddr_in addrport;
    SOCKET sockid = INVALID_SOCKET;
    std::string msg = "";
    std::vector<client_type> client(MAX_CLIENTS), client_List;
    int num_clients = 0;
    int temp_id = -1;
    std::thread my_thread[MAX_CLIENTS];

    std::string str;

    std::cout << "Intializing Winsock..." << std::endl;

    WSADATA wsaData;
    int wsOK = WSAStartup(MAKEWORD(2, 2), &wsaData);
    if (wsOK != 0) {
        std::cout << "Can't initialize winsock. Application is now exiting..." << std::endl;
        return 0;
    }

    Read_Account(client_List);

    ZeroMemory(&addrport, sizeof(addrport));
    addrport.sin_family = AF_INET;
    addrport.sin_port = htons(5000);
    addrport.sin_addr.s_addr = htonl(INADDR_ANY);// inet_addr(c);

    /*std::cout << "Setting up server..." << std::endl;
    getaddrinfo(IP_ADDRESS, PORT, &hints, &server);*/

    sockid = socket(PF_INET, SOCK_STREAM, 0);

    ////Setup socket options
    //setsockopt(server_socket, SOL_SOCKET, SO_REUSEADDR, &OPTION_VALUE, sizeof(int)); //Make it possible to re-bind to a port that was used within the last 2 minutes
    //setsockopt(server_socket, IPPROTO_TCP, TCP_NODELAY, &OPTION_VALUE, sizeof(int)); //Used for interactive programs


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
            ////Send the id to that client
            //std::cout << "Client #" << client[temp_id].id << " Accepted" << std::endl;
            char temp[DEFAULT_BUFFER_LENGTH];
            //send(NewSockid, temp, DEFAULT_BUFFER_LENGTH, 0);

            blahblah:


            int iResult = recv(NewSockid, temp, DEFAULT_BUFFER_LENGTH, 0);
            

            if (strcmp(temp, "register") == 0) {        
                send(NewSockid, "OK", 3, 0);
                if (Register(NewSockid, client_List) == true) { // De y dong nay, dong nay khi lam register xong thi lam gi phu thuoc vao UI
                    Write_Account(client_List);
                    goto blahblah;
                }
                else {

                }
            }

            if (strcmp(temp, "login") == 0) {
                send(NewSockid, "OK", 3, 0);
                if (Login(NewSockid, client_List) == true) {
                    
                    //my_thread[temp_id] = std::thread(Client_Multiple_Chatting, std::ref(client[temp_id]), std::ref(client), std::ref(my_thread[temp_id]));

                }
                else {

                }
            }
            
            msg = "OK";
            send(NewSockid, msg.c_str(), strlen(msg.c_str()), 0);
            my_thread[temp_id] = std::thread(Client_Multiple_Chatting, std::ref(client[temp_id]), std::ref(client), std::ref(my_thread[temp_id]));
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
        my_thread[i].detach();
        closesocket(client[i].socket);
    }

    WSACleanup();
    std::cout << "Program has ended successfully" << std::endl;

    system("pause");
    return 0;
}