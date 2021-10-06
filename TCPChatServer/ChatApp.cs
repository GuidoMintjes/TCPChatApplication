using System;
using System.IO;
using TCPChatServer;

namespace TCPChatServer {
    class ChatApp {

        static void Main(string[] args) {

            Console.Title = "TCP Chat Server Demo";
            int maxConnectionsStart = 10, portStart = 0;

            string INFO = File.ReadAllText(@"SERVER_INFO.txt");

            string[] INFOS = INFO.Split(';');


            try {
                maxConnectionsStart = Convert.ToInt32(INFOS[0]);
                portStart = Convert.ToInt32(INFOS[1]);
            } catch {

                Console.WriteLine("File (SERVER_INFO.txt) contents broken!");
                Console.ReadLine();
                Environment.Exit(0);
            }


            ChatServer.StartServer(maxConnectionsStart, portStart);

            Console.Read();
        }
    }
}