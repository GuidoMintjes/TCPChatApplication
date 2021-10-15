using System;
using System.Collections.Generic;
using System.Text;

namespace TCPChatServer {
    public static class ServerCommand {

        public static void CommandLoop() {

            string commandRaw = Console.ReadLine();
            string command, argument;

            if(commandRaw[0] == '/') {

                commandRaw = commandRaw.Substring(1);

                string[] commandsRaw = commandRaw.Split(" ");

                command = commandsRaw[0];
                argument = commandsRaw[1];


                switch(command) {

                    case "say":

                        TCPServerSend.TCPSendPacketToAll(CreateMessagePacket(argument));
                        break;


                    default:
                        break;
                }
            }
        }


        private static Packet CreateMessagePacket(string message) {

            Packet packet = new Packet((int) ServerPackets.message);

            packet.PacketWrite(message);

            return packet;
        }
    }
}