#include "Header.h"

int main() {
	client_type client;
	struct sockaddr_in addrport;
	Init(client);

	//Register(client);
	
	Login(client);

	Client_Group_Chat(client);

    

	return 0;
}