#include "Header.h"

void Client_Multiple_Chatting(client_type& new_client, std::vector<client_type>& client_array, std::thread& thread) {
    std::string msg = "";
    char tempmsg[4096] = "";

    while (true) {
        memset(&tempmsg, NULL, sizeof(tempmsg));

        if (new_client.socket != INVALID_SOCKET) {
            int iResult = recv(new_client.socket, tempmsg, 4096, 0);
            //tempmsg[strlen(tempmsg)] = '\0';
            if (iResult != SOCKET_ERROR) {
                //if (strcmp("", tempmsg) == 0)
                    
                    
                    
                    msg = "Client #" + std::to_string(new_client.id) + ": " + (tempmsg);  // De y dong nay, client # la tam thoi
                    // Sau nay co username thi chinh sua lai






                std::cout << msg.c_str() << std::endl;

                for (int i = 0; i < MAX_CLIENTS; i++) {
                    if (client_array[i].socket != INVALID_SOCKET)
                        if (new_client.id != i)
                            iResult = send(client_array[i].socket, msg.c_str(), strlen(msg.c_str()), 0);
                }
            }
            else {
                msg = "Client #" + std::to_string(new_client.id) + " has disconnected";

                PlaySound(TEXT("Sound/Summoner.mp3"), NULL, SND_SYNC);

                std::cout << msg << std::endl;

                closesocket(new_client.socket);
                closesocket(client_array[new_client.id].socket);
                client_array[new_client.id].socket = INVALID_SOCKET;


                for (int i = 0; i < MAX_CLIENTS; i++) {
                    if (client_array[i].socket != INVALID_SOCKET)
                        iResult = send(client_array[i].socket, msg.c_str(), strlen(msg.c_str()), 0);
                }

                break;
            }
        }
    } 

    thread.detach();
}