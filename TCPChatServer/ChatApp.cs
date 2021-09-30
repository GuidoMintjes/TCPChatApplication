using System;

namespace TCPChatServer {
    class ChatApp {

        static void Main(string[] args) {

            Console.Title = "TCP Chat Server Demo";

            ChatServer.StartServer(10, 8900);

            Console.ReadLine();
        }
    }
}