#ifndef HEADER_H
#define HEADER_H
#include <iostream>
#include <thread>
#include <vector>
#include <stdexcept>
#include <sstream>
#include <iomanip>
#include <cstdint>
#pragma comment( lib, "wsock32.lib" )

#define PORT "5000"

const char OPTION_VALUE = 1;
const int MAX_CLIENTS = 5;

struct client_type
{
    int id;
    SOCKET socket;
};

#endif
