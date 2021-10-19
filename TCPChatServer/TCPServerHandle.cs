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


            ChatServer.connections[clientID].userName = receivedUserName;   // Saves the username for this client


            // Notify other chatters of person being connected

            Packet connectionPacket = new Packet(5);
            connectionPacket.PacketWrite(receivedUserName);

            TCPServerSend.TCPSendPacketToAll(clientID, connectionPacket);



            if(clientID != receivedClientID) {

                Console.WriteLine();
                Funcs.printMessage(0, $"Client {receivedUserName} with ID {clientID} has the wrong ID: {receivedClientID}!", false);
                Console.WriteLine();

                ChatServer.connections[clientID].Disconnect();
            }
        }


        public static void PassChatMessage(int clientID, Packet packet) {

            string message = packet.PacketReadString(true);
            Funcs.printMessage(4, message, false);

            TCPServerSend.TCPSendPacketToAll(packet);
        }
    }
}