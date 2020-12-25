#include "Header.h"

void Read_Account(std::vector<client_type>& User_List) {
    std::ifstream f("Data\\Account.csv");
    //SkipBOM(f);
    if (!f.is_open())
        return;
    string temp;
    while (!f.eof()) {
        client_type user;
        getline(f, user.Username, ',');
        getline(f, user.Password, ',');
        getline(f, user.Fullname, ',');
        getline(f, user.DOB, ',');
        getline(f, user.Email, '\n');
        if (user.Username.size() != 0)
            User_List.push_back(user);
    }
    f.close();
}

void Write_Account(vector<client_type>& users) {
    ofstream o("Data\\Account.csv");
    if (!o.is_open())
        return;
    for (int i = 0; i < users.size(); ++i) {
        o << users[i].Username << ',';
        o << users[i].Password << ',';
        o << users[i].Fullname << ',';
        o << users[i].DOB << ',';
        o << users[i].Email << std::endl;
    }
    o.close();
}

bool Register(SOCKET NewSockid, std::vector<client_type>& User_List) {
    client_type new_user;
    char temp[DEFAULT_MSG_LENGTH] = "";
    int iResult = recv(NewSockid, temp, DEFAULT_MSG_LENGTH, 0);
    if (iResult != SOCKET_ERROR) {
        for (std::vector<client_type>::iterator p = User_List.begin(); p != User_List.end(); p++) {
            if ((*p).Username.compare(std::string(temp)) == 0) {
                memset(&temp, NULL, sizeof(temp));
                strncpy(temp, "Username already taken", DEFAULT_MSG_LENGTH);
                send(NewSockid, temp, DEFAULT_MSG_LENGTH, 0);
                return false;
            }
        }
        send(NewSockid, "OK", 3, 0);
        new_user.Username = std::string(temp);
        memset(&temp, NULL, sizeof(temp));
        iResult = recv(NewSockid, temp, DEFAULT_MSG_LENGTH, 0);
        if (iResult != SOCKET_ERROR) {
            send(NewSockid, "OK", 3, 0);
            new_user.Password = std::string(temp);
            memset(&temp, NULL, sizeof(temp));
            iResult = recv(NewSockid, temp, DEFAULT_MSG_LENGTH, 0);
            if (iResult != SOCKET_ERROR) {
                send(NewSockid, "OK", 3, 0);
                new_user.Fullname = std::string(temp);
                memset(&temp, NULL, sizeof(temp));
                iResult = recv(NewSockid, temp, DEFAULT_MSG_LENGTH, 0);
                if (iResult != SOCKET_ERROR) {
                    send(NewSockid, "OK", 3, 0);
                    new_user.DOB = std::string(temp);
                    memset(&temp, NULL, sizeof(temp));
                    iResult = recv(NewSockid, temp, DEFAULT_MSG_LENGTH, 0);
                    if (iResult != SOCKET_ERROR) {
                        new_user.Email = std::string(temp);
                        User_List.push_back(new_user);
                        memset(&temp, NULL, sizeof(temp));
                        strncpy(temp, "Register successfully", DEFAULT_MSG_LENGTH);
                        send(NewSockid, temp, DEFAULT_MSG_LENGTH, 0);
                        return true;
                    }
                    return false;
                }
                return false;
            } 
            return false;
        }
        return false;   
    }
    return false;
}

bool Login(SOCKET NewSockid, std::vector<client_type>& User_List, std::string& username) {
    char temp[DEFAULT_MSG_LENGTH] = "";
    int iResult = recv(NewSockid, temp, DEFAULT_MSG_LENGTH, 0);
    if (iResult != SOCKET_ERROR) {
        send(NewSockid, "OK", 3, 0);
        for (std::vector<client_type>::iterator p = User_List.begin(); p != User_List.end(); p++) {
            if ((*p).Username.compare(std::string(temp)) == 0) {
                username = (*p).Username;
                memset(&temp, NULL, sizeof(temp));
                iResult = recv(NewSockid, temp, DEFAULT_MSG_LENGTH, 0);
                if (iResult != SOCKET_ERROR) 
                    if ((*p).Password.compare(std::string(temp)) == 0) {
                        memset(&temp, NULL, sizeof(temp));
                        strncpy(temp, "Login successfully", DEFAULT_MSG_LENGTH);
                        send(NewSockid, temp, DEFAULT_MSG_LENGTH, 0);
                        return true;
                    }
                    else {
                        memset(&temp, NULL, sizeof(temp));
                        strncpy(temp, "ID or Password is incorrect", DEFAULT_MSG_LENGTH);
                        send(NewSockid, temp, DEFAULT_MSG_LENGTH, 0);
                        return false;
                    }
                return false;
            }
        }
        return false;
    }
    return false;
}

void Client_Multiple_Chatting(client_type& new_client, std::vector<client_type>& client_array, std::thread& thread) {
    std::string msg = "";
    char tempmsg[DEFAULT_MSG_LENGTH] = "";

    while (true) {
        memset(&tempmsg, NULL, sizeof(tempmsg));

        if (new_client.socket != 0) {
            int iResult = recv(new_client.socket, tempmsg, DEFAULT_MSG_LENGTH, 0);
            //tempmsg[strlen(tempmsg)] = '\0';
            if (iResult != SOCKET_ERROR) {
                //if (strcmp("", tempmsg) == 0)

                msg = new_client.Username + ": " + (tempmsg);

                //std::cout << msg.c_str() << std::endl;

                for (int i = 0; i < MAX_CLIENTS; i++) {
                    if (client_array[i].socket != INVALID_SOCKET)
                        if (client_array[i].socket == SOCKET_ERROR) continue;
                    if (new_client.id != i)
                        iResult = send(client_array[i].socket, msg.c_str(), strlen(msg.c_str()), 0);
                }
            }
            else {
                msg = new_client.Username + " has disconnected";

                std::cout << msg << std::endl;

                closesocket(new_client.socket);
                closesocket(client_array[new_client.id].socket);
                client_array[new_client.id].socket = INVALID_SOCKET;


                for (int i = 0; i < MAX_CLIENTS; i++) {
                    if (client_array[i].socket != INVALID_SOCKET)
                        iResult = send(client_array[i].socket, msg.c_str(), strlen(msg.c_str()), 0);
                }

                break;
            }
        }
    }

    thread.detach();
}

void Client_Single_Chatting(client_type& first_client, std::vector<client_type>& client_array, std::string second_username, std::thread& thread) {
    std::string msg = "";
    char tempmsg[DEFAULT_MSG_LENGTH] = "";
    int pos = 0;

    while (true) {
        memset(&tempmsg, NULL, sizeof(tempmsg));
        if (first_client.socket != INVALID_SOCKET) {
            int iResult = recv(first_client.socket, tempmsg, DEFAULT_MSG_LENGTH, 0);
            if (iResult != SOCKET_ERROR) {

                for (int i = 0; i < MAX_CLIENTS; i++) {
                    if (client_array[i].socket == INVALID_SOCKET) continue;
                    else if (first_client.id != i && client_array[i].Username.compare(second_username) == 0) {

                        if (strcmp(tempmsg, "upload file") == 0) {
                            Upload_File(first_client);
                            break;
                        }

                        else if (strcmp(tempmsg, "download file") == 0) {
                            Download_File(first_client);
                            break;
                        }

                        else {
                            pos = i;
                            msg = first_client.Username + ": " + (tempmsg);
                            iResult = send(client_array[i].socket, msg.c_str(), strlen(msg.c_str()), 0);
                            break;
                        }
                    }
                }
            }
            else {

                closesocket(first_client.socket);
                first_client.socket = INVALID_SOCKET;

                closesocket(client_array[first_client.id].socket);
                client_array[first_client.id].socket = INVALID_SOCKET;

                closesocket(client_array[pos].socket);
                client_array[pos].socket = INVALID_SOCKET;

                break;
            }
        }
    }

    thread.detach();
}

void Upload_File(client_type& first_client) {
    char tempmsg[DEFAULT_MSG_LENGTH] = "";

    //Receive file name and size from first client 
    memset(&tempmsg, NULL, sizeof(tempmsg));
    recv(first_client.socket, tempmsg, DEFAULT_MSG_LENGTH, 0);
    send(first_client.socket, "OK", 3, 0);
    string fName = string(tempmsg);

    // Use for stopping client thread
    memset(&tempmsg, NULL, sizeof(tempmsg));
    recv(first_client.socket, tempmsg, DEFAULT_MSG_LENGTH, 0);
    if (strcmp(tempmsg, "resend") == 0) {
        send(first_client.socket, "OK", 3, 0);
    }
    //

    memset(&tempmsg, NULL, sizeof(tempmsg));
    recv(first_client.socket, tempmsg, DEFAULT_MSG_LENGTH, 0);
    send(first_client.socket, "OK", 3, 0);
    long long int fSize = stoll(string(tempmsg));


    //Receive file buffer from first client and save to temp folder
    ofstream fs;
    fs.open("Temp\\" + fName, ios::binary | ios::trunc);

    //bool check = false;

    while (true) {
        memset(&tempmsg, NULL, sizeof(tempmsg));
        char* buffer = new char[DEFAULT_TRANSFER_BUFFER_SIZE];

        recv(first_client.socket, tempmsg, DEFAULT_MSG_LENGTH, 0);

        if (strcmp(tempmsg, "end") == 0) {
            fs.close();
            delete[] buffer;
            send(first_client.socket, "OK", 3, 0);
            break;
        }

        int buffersize = stoi(string(tempmsg));
        send(first_client.socket, "OK", 3, 0);

        int iResult = recv(first_client.socket, buffer, DEFAULT_TRANSFER_BUFFER_SIZE, 0);
        if (iResult == SOCKET_ERROR)
            break;

        else if (iResult != buffersize)
            send(first_client.socket, "no", 3, 0);

        else if (iResult < DEFAULT_TRANSFER_BUFFER_SIZE) {
            send(first_client.socket, "OK", 3, 0);
            char* buffer2 = new char[iResult];
            memcpy(buffer2, buffer, iResult);
            fs.write(buffer2, iResult);
            delete[] buffer2;
        }

        else {
            send(first_client.socket, "OK", 3, 0);
            fs.write(buffer, DEFAULT_TRANSFER_BUFFER_SIZE);
        }
        delete[] buffer;
    }
}

void Download_File(client_type& client) {

}

void Client_Thread(SOCKET NewSockid, std::vector<client_type>& client_List, std::vector<client_type>& client, std::thread my_thread[], int temp_id) {
    blahblah:
    char temp[DEFAULT_MSG_LENGTH];
    int iResult = recv(NewSockid, temp, DEFAULT_MSG_LENGTH, 0);

    if (strcmp(temp, "register") == 0) {
        send(NewSockid, "OK", 3, 0);
        if (Register(NewSockid, client_List) == true) { 
            Write_Account(client_List);
            goto blahblah;
        }

        if (strcmp(temp, "login") == 0) {
            std::string username;
            send(NewSockid, "OK", 3, 0);
            if (Login(NewSockid, client_List, username) == true) {
                client[temp_id].Username = username;
                memset(&temp, NULL, sizeof(temp));
                recv(NewSockid, temp, DEFAULT_MSG_LENGTH, 0);
                if (strcmp(temp, "private chat") == 0) {
                    recv(NewSockid, temp, DEFAULT_MSG_LENGTH, 0);
                    string tempo = std::string(temp);
                    my_thread[temp_id] = std::thread(Client_Single_Chatting, std::ref(client[temp_id]), std::ref(client), std::ref(tempo), std::ref(my_thread[temp_id]));
                }
                else {
                    my_thread[temp_id] = std::thread(Client_Multiple_Chatting, std::ref(client[temp_id]), std::ref(client), std::ref(my_thread[temp_id]));
                }
            }
        }
    }
}