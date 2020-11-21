#ifndef HEADER_H
#define HEADER_H
#define _WINSOCK_DEPRECATED_NO_WARNINGS
#include <iostream>
#include <winsock2.h>
#include <ws2tcpip.h>
#include <string>
#include <thread>
#include <vector>
#include <stdexcept>
#include <sstream>
#include <iomanip>
#include <cstdint>
#include <Windows.h>
#pragma comment( lib, "wsock32.lib" )
#pragma comment( lib, "Ws2_32.lib")
#pragma comment( lib, "winmm.lib")

#define PORT "5000"

const char OPTION_VALUE = 1;
const int MAX_CLIENTS = 10;

struct client_type
{
    int id = -1;
    std::string IP, Username, 
        Password = "123";
    SOCKET socket = INVALID_SOCKET;
};

void Client_Multiple_Chatting(client_type& new_client, std::vector<client_type>& client_array, std::thread& thread);


#endif
