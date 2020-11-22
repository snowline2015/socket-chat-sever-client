#ifndef HEADER_H
#define HEADER_H
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
#include <Windows.h>
#pragma comment( lib, "wsock32.lib" )
#pragma comment( lib, "Ws2_32.lib")
#pragma comment( lib, "winmm.lib")
using namespace std;

#define PORT "5000"

struct client_type {
    SOCKET socket = INVALID_SOCKET;
    int id = -1;
    std::string IP, Username, Password;
    char RecvMsg[4096];
};

void Client_Multiple_Chatting(client_type& new_client);

#endif
