#include "Header.h"

int main() {
    struct sockaddr_in addrport;
    struct sockaddr_in* server = NULL, * ptr = NULL, * result = NULL;
    SOCKET sockid = INVALID_SOCKET, NewSockid;
    std::string msg = "";
    int iResult = 0;

    std::string str;
    std::cout << "Input server ip address: ";
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


    // Attempt to connect to an address until one succeeds
    while (true) {
        sockid = socket(PF_INET, SOCK_STREAM, 0);
        iResult = connect(sockid, (struct sockaddr*)&addrport, sizeof(addrport));
        if (iResult == SOCKET_ERROR) {
            closesocket(sockid);
            sockid = INVALID_SOCKET;
            continue;
        }
        break;
    }

    if (sockid == INVALID_SOCKET) {
        std::cout << "Unable to connect to server!" << std::endl;
        WSACleanup();
        system("pause");
        return 1;
    }

    std::cout << "Successfully Connected" << std::endl;

    char temp[4096];
    
    while (true)
    {
        
        int BytesReceived = recv(sockid, temp, 4096, 0);
        
        if (BytesReceived == SOCKET_ERROR) continue;
        else if (BytesReceived == 0) break;
        else {
            std::cout << temp << std::endl;
            memset(&temp, NULL, sizeof(temp));
            std::cout << "Message: ";
            getline(std::cin, str);
            //str = string_to_hex(str);
            iResult = send(sockid, str.c_str(), sizeof(str.c_str()), 0);

            if (iResult <= 0)
            {
                std::cout << "send() failed: " << WSAGetLastError() << std::endl;
                break;
            }
        }
    }
    std::cout << "Shutting down socket..." << std::endl;
    /*iResult = shutdown(NewSockid, SD_SEND);
    if (iResult == SOCKET_ERROR) {
        std::cout << "shutdown() failed with error: " << WSAGetLastError() << std::endl;
        closesocket(NewSockid);
        WSACleanup();
        system("pause");
        return 1;
    }*/

    closesocket(sockid);
    WSACleanup();
    system("pause");
    return 0;
}