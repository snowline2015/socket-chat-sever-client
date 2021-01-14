#include "Header.h"
std::atomic<bool> stop_receiving_for_downloading_flag(false);

void Read_Account(std::vector<client_type>& User_List) {
    User_List.clear();
    std::ifstream f("Data\\Account.csv");
    if (!f.is_open())
        return;
    while (!f.eof()) {
        string temp;
        client_type user;
        getline(f, user.Username, ',');
        getline(f, user.Password, ',');
        getline(f, user.Fullname, ',');
        getline(f, user.DOB, ',');
        getline(f, user.Email, ',');
        getline(f, user.Bio, ';');
        getline(f, temp, '\n');
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
        o << users[i].Email << ',';
        o << users[i].Bio << ';' << std::endl;
    }
    o.close();
}

void CloseSocket(SOCKET Sockid) {
    closesocket(Sockid);
    Sockid = INVALID_SOCKET;
}

bool Register(SOCKET NewSockid, std::vector<client_type>& User_List) {
    client_type new_user;
    char temp[DEFAULT_MSG_LENGTH] = "";
    int iResult = recv(NewSockid, temp, DEFAULT_MSG_LENGTH, 0);
    if (iResult != SOCKET_ERROR) {
        for (std::vector<client_type>::iterator p = User_List.begin(); p != User_List.end(); p++) {
            if ((*p).Username.compare(std::string(temp)) == 0) {
                send(NewSockid, "NO", 3, 0);
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
                        send(NewSockid, "OK", 3, 0);
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
                if (iResult != SOCKET_ERROR) {
                    if ((*p).Password.compare(std::string(temp)) == 0) {
                        send(NewSockid, "OK", 3, 0);
                        return true;
                    }
                    else {
                        send(NewSockid, "NO", 3, 0);
                        return false;
                    }
                }
                else return false;
            }
        }
        send(NewSockid, "NO", 3, 0);
        return false;
    }
    return false;
}

void Client_Multiple_Chatting(client_type& new_client, std::vector<client_type>& client_array, std::thread& thread) {
    std::string msg = "";
    char tempmsg[DEFAULT_MSG_LENGTH] = "";

    while (true) {
        memset(&tempmsg, NULL, sizeof(tempmsg));
        if (new_client.socket != INVALID_SOCKET) {
            int iResult = recv(new_client.socket, tempmsg, DEFAULT_MSG_LENGTH, 0);
            if (iResult > 0) {
                if (strcmp(tempmsg, "-back") == 0) {
                    msg = "Server:" + new_client.Username + " has left the chat";
                    for (int i = 0; i < MAX_CLIENTS; i++) {
                        if (client_array[i].socket != INVALID_SOCKET && new_client.RoomID.compare(client_array[i].RoomID) == 0) {
                            iResult = send(client_array[i].socket, msg.c_str(), strlen(msg.c_str()), 0);
                            while (iResult <= 0) {
                                sleep_for(milliseconds(10));
                                iResult = send(client_array[i].socket, msg.c_str(), strlen(msg.c_str()), 0);
                                if (iResult <= 0) break;
                            }
                        }
                    }
                    break;
                }

                msg = new_client.Username + ":" + (tempmsg);

                for (int i = 0; i < MAX_CLIENTS; i++) {
                    if (client_array[i].socket != INVALID_SOCKET) {
                        if (new_client.id != i && new_client.RoomID.compare(client_array[i].RoomID) == 0) {
                            iResult = send(client_array[i].socket, msg.c_str(), strlen(msg.c_str()), 0);
                            while (iResult <= 0) {
                                sleep_for(milliseconds(10));
                                iResult = send(client_array[i].socket, msg.c_str(), strlen(msg.c_str()), 0);
                                if (iResult <= 0) break;
                            }
                        }
                    }
                }
            }
            else {
                msg = "Server:" + new_client.Username + " has disconnected";
                for (int i = 0; i < MAX_CLIENTS; i++) {
                    if (client_array[i].socket != INVALID_SOCKET && new_client.RoomID.compare(client_array[i].RoomID) == 0) {
                        iResult = send(client_array[i].socket, msg.c_str(), strlen(msg.c_str()), 0);
                        while (iResult <= 0) {
                            sleep_for(milliseconds(10));
                            iResult = send(client_array[i].socket, msg.c_str(), strlen(msg.c_str()), 0);
                            if (iResult <= 0) break;
                        }
                    }
                }
                break;
            }
        }
        else break;
    }
}

void Client_Single_Chatting(client_type& first_client, std::vector<client_type>& client_array, std::string second_username, std::thread& thread) {
    std::string msg = "";
    char tempmsg[DEFAULT_MSG_LENGTH] = "";

    while (true) {
        memset(&tempmsg, NULL, sizeof(tempmsg));
        if (first_client.socket != INVALID_SOCKET && stop_receiving_for_downloading_flag.load() == false) {
            int iResult = recv(first_client.socket, tempmsg, DEFAULT_MSG_LENGTH, 0);
            if (iResult != SOCKET_ERROR) {
                for (int i = 0; i < MAX_CLIENTS; i++) {
                    if (client_array[i].socket == INVALID_SOCKET) continue;
                    else if (first_client.id != i && client_array[i].Username.compare(second_username) == 0) {
                        if (strcmp(tempmsg, "-upload-file") == 0) {
                            string fileName;
                            if (Upload_File(first_client, fileName) == true) {

                                stop_receiving_for_downloading_flag.store(true);

                                iResult = send(client_array[i].socket, "-download-file", 15, 0);
                                while (iResult <= 0) {
                                    sleep_for(milliseconds(10));
                                    iResult = send(client_array[i].socket, "-download-file", 15, 0);
                                    if (iResult <= 0) break;
                                }
                                memset(&tempmsg, NULL, sizeof(tempmsg));
                                recv(client_array[i].socket, tempmsg, DEFAULT_MSG_LENGTH, 0);
                                Download_File(client_array[i], fileName);

                                send(first_client.socket, "OK", 3, 0);        // Response to first client for another upload

                                stop_receiving_for_downloading_flag.store(false);
                                break;
                            }
                        }

                        else if (strcmp(tempmsg, "-back") == 0) {
                            send(first_client.socket, "OK", 3, 0);
                            iResult = send(client_array[i].socket, "-disconnect", 12, 0);
                            while (iResult <= 0) {
                                sleep_for(milliseconds(10));
                                iResult = send(client_array[i].socket, "-disconnect", 12, 0);
                                if (iResult <= 0) break;
                            }
                            goto blahblah;
                        }

                        else if (stop_receiving_for_downloading_flag.load() == false) {
                            msg = first_client.Username + ":" + (tempmsg);
                            iResult = send(client_array[i].socket, msg.c_str(), strlen(msg.c_str()), 0);
                            while (iResult <= 0) {
                                sleep_for(milliseconds(10));
                                iResult = send(client_array[i].socket, msg.c_str(), strlen(msg.c_str()), 0);
                                if (iResult <= 0) break;
                            }
                            break;
                        }
                    }    
                }       
            }
            else { 
                for (int i = 0; i < MAX_CLIENTS; i++) {
                    if (client_array[i].socket == INVALID_SOCKET) continue;
                    else if (first_client.id != i && client_array[i].Username.compare(second_username) == 0) {
                        iResult = send(client_array[i].socket, "-disconnect", 12, 0);
                        while (iResult <= 0) {
                            sleep_for(milliseconds(10));
                            iResult = send(client_array[i].socket, "-disconnect", 12, 0);
                            if (iResult <= 0) break;
                        }
                        break;
                    }
                }
                break;
            }
        }
    }
    blahblah: {}   
}

bool Upload_File(client_type& first_client, std::string& fileName) {
    char tempmsg[DEFAULT_MSG_LENGTH] = "";

    //Receive file name and size from first client 
    int iResult = recv(first_client.socket, tempmsg, DEFAULT_MSG_LENGTH, 0);
    if (iResult <= 0)
        return false;
    send(first_client.socket, "OK", 3, 0);
    string fName = string(tempmsg);
    fileName = fName;

    memset(&tempmsg, NULL, sizeof(tempmsg));
    iResult = recv(first_client.socket, tempmsg, DEFAULT_MSG_LENGTH, 0);
    if (iResult <= 0)
        return false;
    if (strcmp(tempmsg, "-resend") == 0)
        send(first_client.socket, "OK", 3, 0);

    memset(&tempmsg, NULL, sizeof(tempmsg));
    iResult = recv(first_client.socket, tempmsg, DEFAULT_MSG_LENGTH, 0);
    if (iResult <= 0)
        return false;
    send(first_client.socket, "OK", 3, 0);
    long long int fSize = stoll(string(tempmsg));

    //Receive file buffer from first client and save to temp folder
    ofstream fs;
    fs.open("Temp\\" + fName, ios::binary | ios::trunc);

    while (true) {
        memset(&tempmsg, NULL, sizeof(tempmsg));
        char* buffer = new char[DEFAULT_TRANSFER_BUFFER_SIZE];

        iResult = recv(first_client.socket, tempmsg, DEFAULT_MSG_LENGTH, 0);
        if (iResult <= 0) {
            fs.close();
            delete[] buffer;
            remove(("Temp\\" + fName).c_str());
            return false;
        }

        if (strcmp(tempmsg, "-end") == 0) {
            fs.close();
            delete[] buffer;
            send(first_client.socket, "OK", 3, 0);
            break;
        }

        int buffersize = stoi(string(tempmsg));
        send(first_client.socket, "OK", 3, 0);

        iResult = recv(first_client.socket, buffer, DEFAULT_TRANSFER_BUFFER_SIZE, 0);
        if (iResult <= 0) {
            fs.close();
            delete[] buffer;
            return false;
        }

        else if (iResult != buffersize) 
            send(first_client.socket, "NO", 3, 0);

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
    return true;
}

bool Download_File(client_type& client, std::string& fileName) {
    char tempmsg[DEFAULT_MSG_LENGTH] = "";

    std::ifstream fp;
    fp.open("Temp\\" + fileName, ios::binary);

    if (fp.fail()) {
        return false;
    }

    // Send file name and file size
    fp.seekg(0, ios::end);
    long long int size = fp.tellg();
    fp.seekg(0, ios::beg);

    int iResult = send(client.socket, fileName.c_str(), strlen(fileName.c_str()), 0);
    while (iResult <= 0) {
        sleep_for(milliseconds(10));
        iResult = send(client.socket, fileName.c_str(), strlen(fileName.c_str()), 0);
        if (iResult <= 0) break;
    }
    iResult = recv(client.socket, tempmsg, DEFAULT_MSG_LENGTH, 0);
    if (iResult <= 0)
        return false;

    iResult = send(client.socket, std::to_string(size).c_str(), strlen(std::to_string(size).c_str()), 0);
    while (iResult <= 0) {
        sleep_for(milliseconds(10));
        iResult = send(client.socket, std::to_string(size).c_str(), strlen(std::to_string(size).c_str()), 0);
        if (iResult <= 0) break;
    }
    memset(&tempmsg, NULL, sizeof(tempmsg));
    iResult = recv(client.socket, tempmsg, DEFAULT_MSG_LENGTH, 0);
    if (iResult <= 0)
        return false;


    //Sending file processing
    long long int sizetemp = size;

    while (sizetemp > 0) {
        if (sizetemp < DEFAULT_TRANSFER_BUFFER_SIZE) {
            char* buffer = new char[sizetemp];
            fp.read(buffer, sizetemp);
            do {
                memset(&tempmsg, NULL, sizeof(tempmsg));
                iResult = send(client.socket, std::to_string(sizetemp).c_str(), strlen(std::to_string(sizetemp).c_str()), 0);
                while (iResult <= 0) {
                    sleep_for(milliseconds(10));
                    iResult = send(client.socket, std::to_string(sizetemp).c_str(), strlen(std::to_string(sizetemp).c_str()), 0);
                    if (iResult <= 0) break;
                }
                iResult = recv(client.socket, tempmsg, DEFAULT_MSG_LENGTH, 0);              ///////////
                if (iResult <= 0)
                    return false;
                memset(&tempmsg, NULL, sizeof(tempmsg));
                iResult = send(client.socket, buffer, sizetemp, 0);
                while (iResult <= 0) {
                    sleep_for(milliseconds(10));
                    iResult = send(client.socket, buffer, sizetemp, 0);
                    if (iResult <= 0) break;
                }
                iResult = recv(client.socket, tempmsg, DEFAULT_MSG_LENGTH, 0);              ///////////
                if (iResult <= 0)
                    return false;
            } while (strcmp(tempmsg, "NO") == 0);
            delete[] buffer;
            sizetemp = 0;
        }
        else {
            char* buffer = new char[DEFAULT_TRANSFER_BUFFER_SIZE];
            fp.read(buffer, DEFAULT_TRANSFER_BUFFER_SIZE);
            do {
                memset(&tempmsg, NULL, sizeof(tempmsg));
                iResult = send(client.socket, std::to_string(DEFAULT_TRANSFER_BUFFER_SIZE).c_str(), strlen(std::to_string(DEFAULT_TRANSFER_BUFFER_SIZE).c_str()), 0);
                while (iResult <= 0) {
                    sleep_for(milliseconds(10));
                    iResult = send(client.socket, std::to_string(DEFAULT_TRANSFER_BUFFER_SIZE).c_str(), strlen(std::to_string(DEFAULT_TRANSFER_BUFFER_SIZE).c_str()), 0);
                    if (iResult <= 0) break;
                }
                iResult = recv(client.socket, tempmsg, DEFAULT_MSG_LENGTH, 0);                  //////////
                if (iResult <= 0)
                    return false;
                memset(&tempmsg, NULL, sizeof(tempmsg));
                iResult = send(client.socket, buffer, DEFAULT_TRANSFER_BUFFER_SIZE, 0);
                while (iResult <= 0) {
                    sleep_for(milliseconds(10));
                    iResult = send(client.socket, buffer, DEFAULT_TRANSFER_BUFFER_SIZE, 0);
                    if (iResult <= 0) break;
                }
                iResult = recv(client.socket, tempmsg, DEFAULT_MSG_LENGTH, 0);                  /////////
                if (iResult <= 0)
                    return false;
            } while (strcmp(tempmsg, "NO") == 0);
            delete[] buffer;
            sizetemp -= DEFAULT_TRANSFER_BUFFER_SIZE;
        }
    }
    fp.close();
    remove(("Temp\\" + fileName).c_str());

    iResult = send(client.socket, "-end", 5, 0);
    while (iResult <= 0) {
        sleep_for(milliseconds(10));
        iResult = send(client.socket, "-end", 5, 0);
        if (iResult <= 0) break;
    }
    recv(client.socket, tempmsg, DEFAULT_MSG_LENGTH, 0);
    return true;
}

void Client_Thread(SOCKET NewSockid, std::vector<client_type>& client_List, std::vector<client_type>& client, std::thread my_thread[], int temp_id, std::thread& thread) {
    char temp[DEFAULT_MSG_LENGTH];
    while (true) {   
        int iResult = recv(NewSockid, temp, DEFAULT_MSG_LENGTH, 0);
        if (iResult <= 0 || strcmp(temp, "-logout") == 0) {
            CloseSocket(client[temp_id].socket);
            CloseSocket(NewSockid);
            std::cout << client[temp_id].Username << " has disconnected" << std::endl;
            client[temp_id].Username = "";
            client[temp_id].id = -1;
            client[temp_id].Online = false;
            break;
        }

        else if (strcmp(temp, "-register") == 0) {
            send(NewSockid, "OK", 3, 0);
            if (Register(NewSockid, client_List) == true)
                Write_Account(client_List);
        }

        else if (strcmp(temp, "-login") == 0) {
            std::string username;
            send(NewSockid, "OK", 3, 0);
            if (Login(NewSockid, client_List, username) == true) {
                client[temp_id].Username = username;
                client[temp_id].Online = true;
                
                std::cout << client[temp_id].Username << " has connected" << std::endl;

                while (true) {
                    memset(&temp, NULL, sizeof(temp));
                    iResult = recv(NewSockid, temp, DEFAULT_MSG_LENGTH, 0);
                    if (iResult <= 0 || strcmp(temp, "-logout") == 0) {
                        CloseSocket(client[temp_id].socket);
                        CloseSocket(NewSockid);
                        client[temp_id].id = -1;
                        client[temp_id].Online = false;
                        break;
                    }

                    else if (strcmp(temp, "-private-chat") == 0) {
                        // Receive check user command
                        memset(&temp, NULL, sizeof(temp));
                        iResult = recv(NewSockid, temp, DEFAULT_MSG_LENGTH, 0);
                        if (iResult <= 0 || strcmp(temp, "-back") == 0) {
                            continue;
                        }
                        
                        else {
                            while (true) {
                                Check_Users_Online(NewSockid, client);

                                // receive online-user username
                                memset(&temp, NULL, sizeof(temp));
                                iResult = recv(NewSockid, temp, DEFAULT_MSG_LENGTH, 0);
                                if (iResult <= 0)
                                    break;

                                if (strcmp(temp, "-check-users") != 0) break;
                            }

                            if (iResult <= 0 || strcmp(temp, "-back") == 0)
                                continue;

                            string tempo = std::string(temp);

                            my_thread[temp_id] = std::thread(Client_Single_Chatting, std::ref(client[temp_id]), std::ref(client), tempo, std::ref(my_thread[temp_id]));
                            my_thread[temp_id].join();
                        }
                    }

                    else if (strcmp(temp, "-public-chat") == 0) {
                        while (true) {
                            // Receive create or join room command
                            memset(&temp, NULL, sizeof(temp));
                            iResult = recv(NewSockid, temp, DEFAULT_MSG_LENGTH, 0);

                            if (iResult <= 0 || strcmp(temp, "-back") == 0) {
                                break;
                            }

                            else if (strcmp(temp, "-create-room") == 0) {
                                //Receive room id
                                send(NewSockid, "OK", 3, 0);
                                memset(&temp, NULL, sizeof(temp));
                                iResult = recv(NewSockid, temp, DEFAULT_MSG_LENGTH, 0);
                                if (iResult <= 0) {
                                    break;
                                }

                                client[temp_id].RoomID = string(temp);

                                int i;
                                for (i = 0; i < client.size(); i++) {
                                    if (i != temp_id && client[i].RoomID.compare(string(temp)) == 0) {
                                        send(NewSockid, "NO", 3, 0);
                                        break;
                                    }
                                }

                                if (i == client.size()) {
                                    send(NewSockid, "OK", 3, 0);
                                    my_thread[temp_id] = std::thread(Client_Multiple_Chatting, std::ref(client[temp_id]), std::ref(client), std::ref(my_thread[temp_id]));
                                    my_thread[temp_id].join();
                                    break;
                                }
                            }

                            else if (strcmp(temp, "-join-room") == 0) {
                                //Receive room id
                                send(NewSockid, "OK", 3, 0);
                                memset(&temp, NULL, sizeof(temp));
                                iResult = recv(NewSockid, temp, DEFAULT_MSG_LENGTH, 0);
                                if (iResult <= 0) {
                                    break;
                                }

                                int i;
                                for (i = 0; i < client.size(); i++) {
                                    if (i != temp_id && client[i].RoomID.compare(string(temp)) == 0) {
                                        send(NewSockid, "OK", 3, 0);
                                        client[temp_id].RoomID = string(temp);
                                        break;
                                    }
                                }

                                if (i == client.size())
                                    send(NewSockid, "NO", 3, 0);
                                else {
                                    my_thread[temp_id] = std::thread(Client_Multiple_Chatting, std::ref(client[temp_id]), std::ref(client), std::ref(my_thread[temp_id]));
                                    my_thread[temp_id].join();
                                    break;
                                }
                            }
                        }
                        client[temp_id].RoomID = "";
                    }

                    else if (strcmp(temp, "-other-option") == 0) {
                        while (true) {
                            memset(&temp, NULL, sizeof(temp));
                            iResult = recv(NewSockid, temp, DEFAULT_MSG_LENGTH, 0);
                            if (iResult <= 0 || strcmp(temp, "-back") == 0)
                                break;

                            else if (strcmp(temp, "-change-password") == 0) {
                                send(NewSockid, "OK", 3, 0);
                                Change_Password(NewSockid, client_List);
                            }

                            else if (strcmp(temp, "-check-user") == 0) {
                                send(NewSockid, "OK", 3, 0);
                                Check_User(NewSockid, client, client_List);
                            }

                            else if (strcmp(temp, "-change-info") == 0) {
                                send(NewSockid, "OK", 3, 0);
                                Change_Info(NewSockid, client_List);
                            }
                        }
                    }
                }
            }
        }
    }
    thread.detach();
}

void Check_Users_Online(SOCKET NewSockid, std::vector<client_type> User_List) {
    std::string str = "";
    for (int i = 0; i < MAX_CLIENTS; i++) 
        if (User_List[i].socket != INVALID_SOCKET && User_List[i].Username != "" && User_List[i].Online == true)
            str.append(User_List[i].Username + "\n");
        
    int iResult = send(NewSockid, str.c_str(), strlen(str.c_str()), 0);
    while (iResult <= 0) {
        sleep_for(milliseconds(10));
        iResult = send(NewSockid, str.c_str(), strlen(str.c_str()), 0);
        if (iResult <= 0) break;
    }
}

void Change_Password(SOCKET NewSockid, std::vector<client_type>& client_List) {
    char temp[DEFAULT_MSG_LENGTH];
    int iResult = recv(NewSockid, temp, DEFAULT_MSG_LENGTH, 0);

    if (iResult <= 0 || strcmp(temp, "-cancel") == 0)
        return;

    for (int i = 0; i < client_List.size(); i++) {
        if (client_List[i].Username.compare(string(temp)) == 0) {
            send(NewSockid, "OK", 3, 0);

            memset(&temp, NULL, sizeof(temp));
            iResult = recv(NewSockid, temp, DEFAULT_MSG_LENGTH, 0);

            if (iResult <= 0)
                return;

            client_List[i].Password = string(temp);
            send(NewSockid, "OK", 3, 0);

            Write_Account(client_List);
            break;
        }
    }
}

void Check_User(SOCKET NewSockid, std::vector<client_type> client, std::vector<client_type> client_List) {
    string user_info = "";
    char temp[DEFAULT_MSG_LENGTH];
    int iResult = recv(NewSockid, temp, DEFAULT_MSG_LENGTH, 0);

    if (iResult <= 0 || strcmp(temp, "-cancel") == 0)
        return;

    int i;
    for (i = 0; i < client_List.size(); i++) {
        if (client_List[i].Username.compare(string(temp)) == 0) {
            user_info.append(client_List[i].Fullname + "\n").append(client_List[i].DOB + "\n").append(client_List[i].Email + "\n").append(client_List[i].Bio + "\n");
            break;
        }
    }

    if (i == client_List.size())
        send(NewSockid, "NO", 3, 0);

    else {
        for (i = 0; i < MAX_CLIENTS; i++) {
            if (client[i].socket != INVALID_SOCKET && client[i].Username.compare(string(temp)) == 0) {
                if (client[i].Online == true)
                    user_info.append("Online\n");
                else
                    user_info.append("Offline\n");
                break;
            }
        }

        if (i == MAX_CLIENTS)
            user_info.append("Offline\n");

        iResult = send(NewSockid, user_info.c_str(), strlen(user_info.c_str()), 0);
        while (iResult <= 0) {
            sleep_for(milliseconds(10));
            iResult = send(NewSockid, user_info.c_str(), strlen(user_info.c_str()), 0);
            if (iResult <= 0) break;
        }
    }
}

void Change_Info(SOCKET NewSockid, std::vector<client_type>& client_List) {
    char temp[DEFAULT_MSG_LENGTH];
    int iResult = recv(NewSockid, temp, DEFAULT_MSG_LENGTH, 0);
    if (iResult <= 0 || strcmp(temp, "-cancel") == 0)
        return;

    for (int i = 0; i < client_List.size(); i++) {
        if (client_List[i].Username.compare(string(temp)) == 0) {
            send(NewSockid, "OK", 3, 0);

            memset(&temp, NULL, sizeof(temp));
            iResult = recv(NewSockid, temp, DEFAULT_MSG_LENGTH, 0);
            if (iResult <= 0)
                return;
            send(NewSockid, "OK", 3, 0);
            client_List[i].Fullname = string(temp);
            
            memset(&temp, NULL, sizeof(temp));
            iResult = recv(NewSockid, temp, DEFAULT_MSG_LENGTH, 0);
            if (iResult <= 0)
                return;
            send(NewSockid, "OK", 3, 0);
            client_List[i].DOB = string(temp);

            memset(&temp, NULL, sizeof(temp));
            iResult = recv(NewSockid, temp, DEFAULT_MSG_LENGTH, 0);
            if (iResult <= 0)
                return;
            send(NewSockid, "OK", 3, 0);
            client_List[i].Email = string(temp);

            memset(&temp, NULL, sizeof(temp));
            iResult = recv(NewSockid, temp, DEFAULT_MSG_LENGTH, 0);
            if (iResult <= 0)
                return;
            send(NewSockid, "OK", 3, 0);
            client_List[i].Bio = string(temp);

            Write_Account(client_List);
            break;
        }
    }
}
