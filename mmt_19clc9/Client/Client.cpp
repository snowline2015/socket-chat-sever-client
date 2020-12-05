#include "Header.h"

int main() {
	client_type client;
	Init(client);

	//Register(client);
	
	//Login(client);

	//Client_Group_Chat(client);

	string temp = "D:\\ASUS\\Videos\\19CLC9.mp4"; // test thu thi de ten file vao day
	Client_Send_File(client, temp);

	return 0;
}