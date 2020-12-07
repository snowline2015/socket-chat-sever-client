#ifndef HEADER_H
#define HEADER_H
#define _WINSOCK_DEPRECATED_NO_WARNINGS
#define _CRT_SECURE_NO_WARNINGS
#include <iostream>
#include <winsock2.h>
#include <ws2tcpip.h>
#include <string>
#include <thread>
#include <stdexcept>
#include <sstream>
#include <iomanip>
#include <Windows.h>
#include <vector>
#include <fstream>
#include <tchar.h>
//#include <stdio.h>
//#include <stdlib.h>
//#include <cstdint>
#include <conio.h>
#pragma comment( lib, "wsock32.lib" )
#pragma comment( lib, "Ws2_32.lib")
#pragma comment( lib, "winmm.lib")
using namespace std;

#define PORT "5000"
#define DEFAULT_BUFFER_LENGTH 4096
#define DEFAULT_SENDER_BUFFER_SIZE 8193
#define DEFAULT_SENDER_BUFFER_LENGTH 8192
#define DEFAULT_RECEIVER_BUFFER_SIZE 8193
#define DEFAULT_RECEIVER_BUFFER_LENGTH 8192

struct client_type {
    SOCKET socket = INVALID_SOCKET;
    int id = -1;
    std::string IP, Username, Password, DOB, Email;
    char RecvMsg[DEFAULT_BUFFER_LENGTH];
};

void Init(client_type& client);
bool Login(client_type& client);
bool Register(client_type& client);
void Client_Thread(client_type& new_client);
void Client_Private_Thread(client_type& new_client);
void Client_Group_Chat(client_type& client);
void Client_Private_Chat(client_type& client);


#endif
