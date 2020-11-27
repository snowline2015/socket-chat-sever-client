#include "Header.h"

bool Login(client_type& client) {
	string id, password;
	cout << "ID: ";
	getline(cin, id);
	cout << "Password: ";
	char c;
	while (true)
	{
		c = _getch();

		if (c == 14) break;

		if (c == 8)
		{
			if (password.size() == 0);
			else cout << "\b \b";
			if (password.size() > 0)
				password.pop_back();
		}

		else
		{
			password.push_back(c);
			cout << "*";
		}
	}

	char temp[DEFAULT_BUFFER_LENGTH] = "login";
	send(client.socket, temp, DEFAULT_BUFFER_LENGTH, 0);
	send(client.socket, id.c_str(), strlen(id.c_str()), 0);
	send(client.socket, password.c_str(), strlen(password.c_str()), 0);

	while (true) {
		int iResult = recv(client.socket, client.RecvMsg, DEFAULT_BUFFER_LENGTH, 0);
		if (iResult <= 0) continue;
		else break;
	}

	if (strcmp(client.RecvMsg, "Access granted") != 0) {
		std::cout << "ID or Password is incorrect" << std::endl;
		return false;
	}

	return true;
}

void Register(client_type& client) {
	blabla:
	string id;
	cout << "New ID: ";
	getline(cin, id);

	char temp[DEFAULT_BUFFER_LENGTH] = "register";
	send(client.socket, temp, DEFAULT_BUFFER_LENGTH, 0);
	send(client.socket, id.c_str(), strlen(id.c_str()), 0);

	while (true) {
		int iResult = recv(client.socket, client.RecvMsg, DEFAULT_BUFFER_LENGTH, 0);
		if (iResult <= 0) continue;
		else break;
	}

	if (strcmp(client.RecvMsg, "New account registered") == 0) 
		std::cout << "Your default password is 123. You can change it later." << std::endl;
	
	if (strcmp(client.RecvMsg, "username already taken") == 0)
		goto blabla;
}

	