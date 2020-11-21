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
#pragma comment( lib, "wsock32.lib" )
#pragma comment( lib, "Ws2_32.lib")

#define PORT "5000"

const char OPTION_VALUE = 1;
const int MAX_CLIENTS = 10;

struct client_type
{
    int id;
    std::string IP, Username, 
        Password = "123";
    SOCKET socket;
};

void Initialize(std::vector<client_type>& client);


#endif
