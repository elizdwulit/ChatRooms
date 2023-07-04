# ChatRooms

## Brief Overview
This project implements a Peer-To-Peer Messaging System using WCF and WPF in .Net Framework. The current implementation is designed for the clients to run on separate ports on the same system. The port is specified by the user and is validated before the user can be connected to the messaging system.

## Supported Functionalities
The System supports the following functionalities:
1. Enter the system with a unique username and port assignment
2. Create a topic in the system with a unique topic name
3. Access all user and topic names
4. Join a topic discussion by selecting the topic name
5. Send a private message to a selected user
6. Send a message to a topic

## How to Use
It uses a Client-Server architecture, where
Client.exe is located: \Client\bin\Release\net48\Client.exe
Server.exe is located: \Server\bin\Release\net48\Server.exe

The Server.exe should only be run once, while the Client executable can be run repeatedly to simulate multiple users in the system. The only limitation is that each client, in the current implementation, must use unique ports to connect to the system.

A demonstration video of the programs can be seen in DEMO_VIDEO.mkv in this repository.

## Brief Code Overview
Client:
- IClientMethods.cs / ClientMethods.cs - Methods that a client (user) can perform
- IServerMethods.cs / ServerMethods.cs - Methods that the server supports
- ConcurrentList.cs - Thread-safe implementation of a List
- MainWindow.xaml[.cs] - UI and UI support for the client executable result
- Topic.cs - Class representing a Topic (group chat)
- TopicWindow.xaml[.cs] - UI and UI support for a window designated for participating in a Topic (group chatting)
- User.cs - Class representing a User in the system

Server:
- IClientMethods.cs / ClientMethods.cs - Methods that a client (user) can perform. Matches the equivalent Client classes.
- IServerMethods.cs / ServerMethods.cs - Methods that the server supports. Matches the equivalent Client classes.
- ConcurrentList.cs - Thread-safe implementation of a List
- Program.cs - Contains Main
- Topic.cs - Class representing a Topic (group chat)
- User.cs - Class representing a User in the system
- TopicManager.cs - Contains functions that affect Topics (CRUD)
- UserManager.cs - Contains functions that affect Users (CRUD)
