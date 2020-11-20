#define _WINSOCK_DEPRECATED_NO_WARNINGS
#include <iostream>
#include <winsock2.h>
#include <ws2tcpip.h>
#include <string>
#include <thread>
#include <stdexcept>
#include <sstream>
#include <iomanip>
#include <cstdint>
#pragma comment( lib, "wsock32.lib" )

#define PORT "5000"

std::string string_to_hex(const std::string& in) {
    std::stringstream ss;

    ss << std::hex << std::setfill('0');
    for (size_t i = 0; in.length() > i; ++i) {
        ss << std::setw(2) << static_cast<unsigned int>(static_cast<unsigned char>(in[i]));
    }

    return ss.str();
}

int main() {
    struct sockaddr_in addrport;
    struct sockaddr_in* server = NULL, * ptr = NULL, * result = NULL;
    SOCKET sockid = INVALID_SOCKET, NewSockid;
    std::string msg = "";
    int iResult = 0;

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

    // Attempt to connect to an address until one succeeds
    while (true) {
        iResult = connect(sockid, (struct sockaddr*)&addrport, sizeof(addrport));
        NewSockid = socket(PF_INET, SOCK_STREAM, 0);
        if (iResult == SOCKET_ERROR) {
            closesocket(NewSockid);
            NewSockid = INVALID_SOCKET;
            continue;
        }
        break;
    }

    if (NewSockid == INVALID_SOCKET) {
        std::cout << "Unable to connect to server!" << std::endl;
        WSACleanup();
        system("pause");
        return 1;
    }

    std::cout << "Successfully Connected" << std::endl;

    char* temp = new char[200];
    recv(NewSockid, temp, sizeof(temp), 0);
    std::cout << temp << std::endl;

    while (true)
    {
        std::cout << "Message: ";
        getline(std::cin, str);
        //str = string_to_hex(str);
        iResult = send(NewSockid, str.c_str(), sizeof(str.c_str()), 0);

        if (iResult <= 0)
        {
            std::cout << "send() failed: " << WSAGetLastError() << std::endl;
            break;
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

    closesocket(NewSockid);
    WSACleanup();
    system("pause");
    return 0;
}