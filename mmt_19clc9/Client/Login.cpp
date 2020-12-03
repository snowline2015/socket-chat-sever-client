#define CLIENT_DLL
#include "Header.h"

bool Login(client_type& client, string id, string password) {
	char temp[DEFAULT_BUFFER_LENGTH] = "login";
	send(client.socket, temp, DEFAULT_BUFFER_LENGTH, 0);

	int iResult = recv(client.socket, client.RecvMsg, DEFAULT_BUFFER_LENGTH, 0);
	if (iResult != SOCKET_ERROR) {
		if (strcmp(client.RecvMsg, "OK") != 0)
			return false;
	}

	send(client.socket, id.c_str(), strlen(id.c_str()), 0);

	iResult = recv(client.socket, client.RecvMsg, DEFAULT_BUFFER_LENGTH, 0);
	if (iResult != SOCKET_ERROR) {
		if (strcmp(client.RecvMsg, "OK") != 0)
			return false;
	}

	send(client.socket, password.c_str(), strlen(password.c_str()), 0);

	while (true) {
		iResult = recv(client.socket, client.RecvMsg, DEFAULT_BUFFER_LENGTH, 0);
		if (iResult <= 0) continue;
		else break;
	}

	if (strcmp(client.RecvMsg, "ID or Password is incorrect") == 0) {
		std::cout << client.RecvMsg << std::endl;
		return false;
	}

	std::cout << "Login successfully" << std::endl;
	return true;
}

bool Register(client_type& client, string username, string password, string fullname, string birthday, string email) {
	char temp[DEFAULT_BUFFER_LENGTH] = "register";
	send(client.socket, temp, DEFAULT_BUFFER_LENGTH, 0);
	int iResult = recv(client.socket, client.RecvMsg, DEFAULT_BUFFER_LENGTH, 0);
	if (iResult != SOCKET_ERROR) {
		if (strcmp(client.RecvMsg, "OK") != 0)
			return false;
	}

	send(client.socket, username.c_str(), strlen(username.c_str()), 0);
	iResult = recv(client.socket, client.RecvMsg, DEFAULT_BUFFER_LENGTH, 0);
	if (iResult != SOCKET_ERROR) {
		if (strcmp(client.RecvMsg, "Username already taken") == 0) {
			std::cout << client.RecvMsg << std::endl;
			return false;
		}
	}
	else return false;

	send(client.socket, password.c_str(), strlen(password.c_str()), 0);
	iResult = recv(client.socket, client.RecvMsg, DEFAULT_BUFFER_LENGTH, 0);
	if (iResult != SOCKET_ERROR) {
		if (strcmp(client.RecvMsg, "OK") != 0)
			return false;
	}
	else return false;

	send(client.socket, fullname.c_str(), strlen(fullname.c_str()), 0);
	iResult = recv(client.socket, client.RecvMsg, DEFAULT_BUFFER_LENGTH, 0);
	if (iResult != SOCKET_ERROR) {
		if (strcmp(client.RecvMsg, "OK") != 0)
			return false;
	}
	else return false;

	send(client.socket, birthday.c_str(), strlen(birthday.c_str()), 0);
	iResult = recv(client.socket, client.RecvMsg, DEFAULT_BUFFER_LENGTH, 0);
	if (iResult != SOCKET_ERROR) {
		if (strcmp(client.RecvMsg, "OK") != 0)
			return false;
	}
	else return false;

	send(client.socket, email.c_str(), strlen(email.c_str()), 0);
	iResult = recv(client.socket, client.RecvMsg, DEFAULT_BUFFER_LENGTH, 0);
	if (strcmp(client.RecvMsg, "Register successfully") == 0)
		std::cout << client.RecvMsg << std::endl;
	return true;
}

	