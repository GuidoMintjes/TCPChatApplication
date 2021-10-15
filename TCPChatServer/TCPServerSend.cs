using System;
using System.Collections.Generic;
using System.Text;

namespace TCPChatServer {
    class TCPServerSend {


        // Send a nice welcoming message to a client that just connected
        public static void WelcomeClient(int clientID, string welcomeMessage) {

            Packet packet = new Packet((int)ServerPackets.welcome);     // Create a welcoming packet with the welcome enum

            packet.PacketWrite(welcomeMessage);                         // Add the welcome message to the packet
            packet.PacketWrite(clientID);                               // Add the client ID to the packet

            TCPSendPacket(clientID, packet);
        }


        // Send an actual instance of a packet through TCP to a client with specified ID
        private static void TCPSendPacket(int clientID, Packet packet) {

            packet.PacketWriteLength();
            ChatServer.connections[clientID].tcp.SendData(packet);
        }


        // Send a packet to all connected clients
        public static void TCPSendPacketToAll(Packet packet) {

            packet.PacketWriteLength();
            for (int i = 1; i < ChatServer.MaxConnections; i++) {

                ChatServer.connections[i].tcp.SendData(packet);

            }


            Funcs.printMessage(2, "Packet sent to all", false);
            //ServerCommand.reading = false;
        }


        // Send a packet to all connected clients except one
        private static void TCPSendPacketToAll(int excludedClient, Packet packet) {

            packet.PacketWriteLength();
            for (int i = 1; i < ChatServer.MaxConnections; i++) {

                if(i != excludedClient)
                    ChatServer.connections[i].tcp.SendData(packet);
            }
        }
    }
}