using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace TCPChatServer {

    class ChatServer {


        public static int MaxConnections { get; private set; }
        public static int Port { get; private set; }



        private static TcpListener tcpListener;     // A listener is an object which 'listens' for incoming connections,
                                                    // this is needed because in the tcp protocol you establish a connection


        // Keep a dictionary of all connections
        public static Dictionary<int, ChatClient> connections = new Dictionary<int, ChatClient>();
        

        // Initialize the server
        public static void StartServer(int maxConnections, int port) {

            Console.WriteLine("Starting server...");
            InitialiseServerData();

            MaxConnections = maxConnections;
            Port = port;

            // These 3 lines start the tcp listener on the 'any' IPAddress with our specific port, and makes sure the
            // callback function is called when a connection is established
            tcpListener = new TcpListener(IPAddress.Any, Port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(ServerConnectCallback), null);

            Console.WriteLine("Server initialized on port: " + Port);

        }

        // Handle connection once it has been established
        private static void ServerConnectCallback(IAsyncResult aResult) {

            // Store the tcp client instance in a local variable here
            TcpClient client = tcpListener.EndAcceptTcpClient(aResult);
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(ServerConnectCallback), null);   // We have to call this function
                                                                                                // again, otherwise the tcplistener would stop
                                                                                                // listening and no other connections could be made

            Console.WriteLine("Someone is trying to connect from: " + client.Client.RemoteEndPoint + 
                                                        "  (INCOMING CONNECTION MESSAGE)");


            for (int i = 1; i <= MaxConnections; i++) {

                // Check if socket of this id in the dictionary is null, that would mean it is vacant
                if(connections[i].tcp.socket == null) {

                    connections[i].tcp.Connect(client);
                    return;     // Return out of method otherwise the client would take up all available spots at once
                }
            }


            Console.WriteLine(client.Client.RemoteEndPoint + " has failed to connect to server because the server is full. " +
                                " (SERVER FULL ERROR)");
        }


        // Initialise our connections dictionary
        private static void InitialiseServerData() {

            for (int i = 1; i <= MaxConnections; i++) {
                connections.Add(i, new ChatClient(i));
            }
        }
    }
}