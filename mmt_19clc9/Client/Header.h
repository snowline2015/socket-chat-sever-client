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
#include <direct.h>
#include <shlobj.h>
#include <conio.h>
#include <stdio.h>
#include <chrono>
#include <thread>
#pragma comment( lib, "wsock32.lib" )
#pragma comment( lib, "Ws2_32.lib")
#pragma comment( lib, "winmm.lib")
using namespace std;
using namespace std::this_thread; 
using namespace std::chrono; 

#define DEFAULT_MSG_LENGTH 4096
#define DEFAULT_BUFFER_SIZE 512

struct client_type {
    SOCKET socket = INVALID_SOCKET;
    std::string IP, Username, Password, DOB, Email;
    char RecvMsg[DEFAULT_MSG_LENGTH];
};

void Init(client_type& client);
bool Login(client_type& client);
bool Register(client_type& client);
void Client_Thread(client_type& new_client);
void Client_Private_Thread(client_type& new_client);
void Client_Group_Chat(client_type& client);
void Client_Private_Chat(client_type& client);

void Upload_File(client_type& client);
void Download_File(client_type& client);


#endif
