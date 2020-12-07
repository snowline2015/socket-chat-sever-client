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
    char temp[DEFAULT_BUFFER_LENGTH] = "";
    int iResult = recv(NewSockid, temp, DEFAULT_BUFFER_LENGTH, 0);
    if (iResult != SOCKET_ERROR) {
        for (std::vector<client_type>::iterator p = User_List.begin(); p != User_List.end(); p++) {
            if ((*p).Username.compare(std::string(temp)) == 0) {
                memset(&temp, NULL, sizeof(temp));
                strncpy(temp, "Username already taken", DEFAULT_BUFFER_LENGTH);
                send(NewSockid, temp, DEFAULT_BUFFER_LENGTH, 0);
                return false;
            }
        }
        send(NewSockid, "OK", 3, 0);
        new_user.Username = std::string(temp);
        memset(&temp, NULL, sizeof(temp));
        iResult = recv(NewSockid, temp, DEFAULT_BUFFER_LENGTH, 0);
        if (iResult != SOCKET_ERROR) {
            send(NewSockid, "OK", 3, 0);
            new_user.Password = std::string(temp);
            memset(&temp, NULL, sizeof(temp));
            iResult = recv(NewSockid, temp, DEFAULT_BUFFER_LENGTH, 0);
            if (iResult != SOCKET_ERROR) {
                send(NewSockid, "OK", 3, 0);
                new_user.Fullname = std::string(temp);
                memset(&temp, NULL, sizeof(temp));
                iResult = recv(NewSockid, temp, DEFAULT_BUFFER_LENGTH, 0);
                if (iResult != SOCKET_ERROR) {
                    send(NewSockid, "OK", 3, 0);
                    new_user.DOB = std::string(temp);
                    memset(&temp, NULL, sizeof(temp));
                    iResult = recv(NewSockid, temp, DEFAULT_BUFFER_LENGTH, 0);
                    if (iResult != SOCKET_ERROR) {
                        new_user.Email = std::string(temp);
                        User_List.push_back(new_user);
                        memset(&temp, NULL, sizeof(temp));
                        strncpy(temp, "Register successfully", DEFAULT_BUFFER_LENGTH);
                        send(NewSockid, temp, DEFAULT_BUFFER_LENGTH, 0);
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
    char temp[DEFAULT_BUFFER_LENGTH] = "";
    int iResult = recv(NewSockid, temp, DEFAULT_BUFFER_LENGTH, 0);
    if (iResult != SOCKET_ERROR) {
        send(NewSockid, "OK", 3, 0);
        for (std::vector<client_type>::iterator p = User_List.begin(); p != User_List.end(); p++) {
            if ((*p).Username.compare(std::string(temp)) == 0) {
                username = (*p).Username;
                memset(&temp, NULL, sizeof(temp));
                iResult = recv(NewSockid, temp, DEFAULT_BUFFER_LENGTH, 0);
                if (iResult != SOCKET_ERROR) 
                    if ((*p).Password.compare(std::string(temp)) == 0) {
                        memset(&temp, NULL, sizeof(temp));
                        strncpy(temp, "Login successfully", DEFAULT_BUFFER_LENGTH);
                        send(NewSockid, temp, DEFAULT_BUFFER_LENGTH, 0);
                        return true;
                    }
                    else {
                        memset(&temp, NULL, sizeof(temp));
                        strncpy(temp, "ID or Password is incorrect", DEFAULT_BUFFER_LENGTH);
                        send(NewSockid, temp, DEFAULT_BUFFER_LENGTH, 0);
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
    char tempmsg[DEFAULT_BUFFER_LENGTH] = "";

    while (true) {
        memset(&tempmsg, NULL, sizeof(tempmsg));

        if (new_client.socket != 0) {
            int iResult = recv(new_client.socket, tempmsg, DEFAULT_BUFFER_LENGTH, 0);
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

                PlaySound(TEXT("Sound\\Summoner.wav"), NULL, SND_ASYNC);

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
    char tempmsg[DEFAULT_BUFFER_LENGTH] = "";
    int pos = 0;

    while (true) {
        memset(&tempmsg, NULL, sizeof(tempmsg));

        if (first_client.socket != 0) {
            int iResult = recv(first_client.socket, tempmsg, DEFAULT_BUFFER_LENGTH, 0);
            if (iResult != SOCKET_ERROR) {
                

                msg = first_client.Username + ": " + (tempmsg);

                for (int i = 0; i < MAX_CLIENTS; i++) {
                    if (client_array[i].socket != INVALID_SOCKET)
                        if (client_array[i].socket == SOCKET_ERROR) continue;
                    if (first_client.id != i && client_array[i].Username.compare(second_username) == 0) {
                        if (strcmp(tempmsg, "send file") == 0) {

                            //send "send file" command
                            send(client_array[i].socket, tempmsg, 10, 0); 
                            memset(&tempmsg, NULL, sizeof(tempmsg));

                            

                            //Receive file name and size from first client and send to second client
                            recv(first_client.socket, tempmsg, DEFAULT_BUFFER_LENGTH, 0);
                            send(client_array[i].socket, tempmsg, DEFAULT_BUFFER_LENGTH, 0);
                            memset(&tempmsg, NULL, sizeof(tempmsg));

                            recv(first_client.socket, tempmsg, DEFAULT_BUFFER_LENGTH, 0);
                            send(client_array[i].socket, tempmsg, DEFAULT_BUFFER_LENGTH, 0);
                            memset(&tempmsg, NULL, sizeof(tempmsg));


                            //Receive file buffer from first client and send to second client
                            while (true) {
                                /*memset(&tempmsg, NULL, sizeof(tempmsg));
                                iResult = recv(first_client.socket, tempmsg, DEFAULT_BUFFER_LENGTH, 0);

                                int buffersize = stoi(std::string(tempmsg));*/

                                char* buffer = new char[DEFAULT_RECEIVE_BUFFER_SIZE];
                                iResult = recv(first_client.socket, buffer, DEFAULT_RECEIVE_BUFFER_LENGTH, 0);
                                if (iResult == SOCKET_ERROR)
                                    break;
                                if (strcmp(buffer, "end") == 0) {
                                    send(client_array[i].socket, "end", 4, 0);
                                    break;
                                }

                                /*if (strlen(buffer) == buffersize)
                                    send(first_client.socket, "yes", 4, 0);
                                while (strlen(buffer) != buffersize) {
                                    memset(&buffer, NULL, sizeof(buffer));
                                    send(first_client.socket, "no", 3, 0);
                                    iResult = recv(first_client.socket, buffer, DEFAULT_RECEIVE_BUFFER_LENGTH, 0);
                                }*/
                                
                                iResult = send(client_array[i].socket, buffer, DEFAULT_SEND_BUFFER_LENGTH, 0);
                                delete[] buffer;
                            }
                        }
                        else {
                            pos = i;
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