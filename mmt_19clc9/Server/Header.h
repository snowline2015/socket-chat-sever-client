#ifndef HEADER_H
#define HEADER_H
#define _WINSOCK_DEPRECATED_NO_WARNINGS
#define _CRT_SECURE_NO_WARNINGS
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
#include <fstream>
#include <Windows.h>
#pragma comment( lib, "wsock32.lib" )
#pragma comment( lib, "Ws2_32.lib")
#pragma comment( lib, "Winmm.lib")
using namespace std;

#define PORT "5000"
#define DEFAULT_BUFFER_LENGTH 4096

const char OPTION_VALUE = 1;
const int MAX_CLIENTS = 10;

struct client_type
{
    int id = -1;
    std::string IP, Username, Password;
    SOCKET socket = INVALID_SOCKET;
};

void Client_Multiple_Chatting(client_type& new_client, std::vector<client_type>& client_array, std::thread& thread);
void Read_Account(std::vector<client_type>& User_List);
bool Register(SOCKET NewSockid, std::vector<client_type>& User_List);
bool Login(SOCKET NewSockid, std::vector<client_type>& User_List);


#endif
