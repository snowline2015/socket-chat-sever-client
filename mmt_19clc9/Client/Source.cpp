#define _WINSOCK_DEPRECATED_NO_WARNINGS
#include <iostream>
#include <string>
#include <winsock2.h>
#pragma comment( lib, "wsock32.lib" )
#include <winsock.h>
#pragma comment( lib, "wsock32.lib" )
#define PORT 1806 

int main() {
	std::string str;
	std::cout << "Input client ip address: ";
	getline(std::cin, str);
	const char* c = str.c_str();

	int sockid = socket(PF_INET, SOCK_STREAM, 0);
	struct sockaddr_in addrport;
	addrport.sin_family = AF_INET;
	addrport.sin_port = htons(PORT);
	addrport.sin_addr.s_addr = inet_addr(c); //htonl(INADDR_ANY);// 
	connect(sockid, (struct sockaddr*)&addrport, sizeof(addrport));
	while (true) {
		char* temp = new char[200];
		if (recv(sockid, temp, sizeof(temp), 0) == 0) {
			temp[strlen(temp) - 1] = '\0';
			std::cout << "Server: " << temp << std::endl;
		}
		/*else {
			std::cout << "Message: ";
			getline(std::cin, str);
			const char* c = str.c_str();
			send(sockid, c, sizeof(c), 0);*/
		//}
	}
	return 0;
}
