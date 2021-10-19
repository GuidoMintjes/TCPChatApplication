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

            Console.WriteLine();
            Console.WriteLine("Sending clientID: " + clientID);
            Console.WriteLine();

            TCPSendPacket(clientID, packet);
        }


        // Send an actual instance of a packet through TCP to a client with specified ID
        private static void TCPSendPacket(int clientID, Packet packet) {

            packet.PacketWriteLength();
            
            /*
            Console.WriteLine("Sending: ");
            foreach (byte byt in packet.GetPacketBytes()) {

                Console.Write(byt + " ");
            }
            Console.WriteLine();
            */

            ChatServer.connections[clientID].tcp.SendData(packet);
        }


        // Send a packet to all connected clients
        public static void TCPSendPacketToAll(Packet packet) {

            packet.PacketWriteLength();
            for (int i = 1; i < ChatServer.MaxConnections; i++) {

                ChatServer.connections[i].tcp.SendData(packet);

            }


            //Funcs.printMessage(2, "Packet sent to all", false);
        }


        // Send a packet to all connected clients except one
        public static void TCPSendPacketToAll(int excludedClient, Packet packet) {

            packet.PacketWriteLength();
            for (int i = 1; i < ChatServer.MaxConnections; i++) {

                if(i != excludedClient)
                    ChatServer.connections[i].tcp.SendData(packet);
            }
        }


        public static void SendDisconnectedMessageToAll(string username, int excludedID) {

            Packet packet = new Packet(6);
            packet.PacketWrite(username);

            TCPSendPacketToAll(excludedID, packet);
        }


        public static void SendNameList(int clientID) {

            Packet namePacket = new Packet( (int) ServerPackets.names);
            
            int nameCount = 0;

            for (int i = 1; i <= ChatServer.connections.Count; i++) {

                if (ChatServer.connections[i].userName != null)
                    nameCount++;
            }

            namePacket.PacketWrite(nameCount);


            for (int i = 1; i <= ChatServer.connections.Count; i++) {

                if (i != clientID) {

                    if (ChatServer.connections[i].userName != null) {

                        namePacket.PacketWrite(ChatServer.connections[i].userName.ToString());

                        /*
                        foreach (byte byt in namePacket.GetPacketBytes()) {

                            Console.Write(byt + " ");
                        }
                        Console.WriteLine();

                        //namePacket.PacketReadInt(true);
                        Console.WriteLine(namePacket.PacketReadString(true));
                        */
                    }
                }
            }


            TCPSendPacket(clientID, namePacket);
        }
    }
}