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
#include <cstdint>
#include <Windows.h>
#include <vector>
#include <conio.h>
#include <fstream>
#pragma comment( lib, "wsock32.lib" )
#pragma comment( lib, "Ws2_32.lib")
#pragma comment( lib, "winmm.lib")

using namespace std;

#ifdef __cplusplus
    extern "C" {
#endif

#ifdef CLIENT_DLL
#define CLIENT_ _declspec(dllexport)
#else
#define CLIENT_ _declspec(dllimport)
#endif

#define PORT "5000"
#define DEFAULT_BUFFER_LENGTH 4096
#define DEFAULT_TRANSFER_LENGTH 1024
        struct client_type {
            SOCKET socket = INVALID_SOCKET;
            int id = -1;
            std::string IP, Username, Password, DOB, Email;
            char RecvMsg[DEFAULT_BUFFER_LENGTH];
        };

        void Init(client_type& client);
        bool Login(client_type& client, string id, string password);
        bool Register(client_type& client, string id, string password, string fullname, string birthday, string email);
        void Client_Thread(client_type& new_client);
        void Client_Group_Chat(client_type& client);
        void Client_Private_Chat(client_type& client);
        bool Client_Send_File(client_type& client, std::string& dir);
        bool Client_Receive_File(client_type& client);

#ifdef __cplusplus {
        }
#endif

#endif
