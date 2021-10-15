using System;
using System.Collections.Generic;
using System.Text;

namespace TCPChatServer {
    class TCPServerHandle {
        

        public static void ReturnedWelcomeReceived(int clientID, Packet packet) {

            int receivedClientID = packet.PacketReadInt(true);
            string receivedUserName = packet.PacketReadString(true);

            Funcs.printMessage(3, $"{ChatServer.connections[clientID].tcp.socket.Client.RemoteEndPoint} connected to this server!"
                + $" (ID {clientID} with name {receivedUserName})", true);

            if(clientID != receivedClientID) {

                Console.WriteLine();
                Funcs.printMessage(0, $"Client {receivedUserName} with ID {clientID} has the wrong ID: {receivedClientID}!", false);
                Console.WriteLine();
            }


            ServerCommand.CommandLoop();
        }
    }
}