#include "Header.h"

int main() {
	client_type client;
	Init(client);

	//Register(client);
	
	Login(client);

	//Client_Group_Chat(client);


	send(client.socket, "private chat", 13, 0);
	std::string user;
	std::cout << "Nhap ten user: ";
	std::getline(std::cin, user);
	send(client.socket, user.c_str(), strlen(user.c_str()), 0);

	Client_Private_Chat(client);

	return 0;
}