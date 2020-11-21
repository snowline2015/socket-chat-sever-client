#include "Header.h"

void Initialize(std::vector<client_type>& client) {
    for (int i = 0; i < MAX_CLIENTS; i++) {
        client[i].socket = INVALID_SOCKET;
        client[i].id = -1;
    }
}