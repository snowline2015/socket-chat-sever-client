#include "Header.h"

void Client_Multiple_Chatting(client_type& new_client) {
    std::string disconnected;
    while (true) {
        memset(new_client.RecvMsg, NULL, sizeof(new_client.RecvMsg));

        if (new_client.socket != 0) {
            int iResult = recv(new_client.socket, new_client.RecvMsg, 4096, 0);

            /*disconnected = new_client.RecvMsg;
            if (disconnected.substr(disconnected.length() - 16).compare("has disconnected") == 0)
                PlaySound(TEXT("Sound\\Summoner.wav"), NULL, SND_SYNC);*/

            if (iResult != SOCKET_ERROR)
                std::cout << new_client.RecvMsg << std::endl;

            else break;   
            
        }
    }

    if (WSAGetLastError() == WSAECONNRESET)
        std::cout << "The server has disconnected" << std::endl;
}