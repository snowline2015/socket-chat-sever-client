#include "Header.h"

void Login(User cur, vector<User> user,bool isSuccess) {
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
	int length = user.size();

	for (int i = 0; i < length; i++)
	{
		if (user.at(i).ID == id && user.at(i).Password == password)
		{
			cout << "Login successfully";

			cur = user.at(i);
			isSuccess = true;
			return;
		}
	}
	cout << "ID or password is not valid";
	isSuccess = false;
}