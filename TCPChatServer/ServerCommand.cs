using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TCPChatServer {
    public static class ServerCommand {

        public static bool reading = false;

        public static void CommandLoop() {

            string commandRaw;

            while (true) {
                if (Console.KeyAvailable) {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    switch (key.Key) {
                        case ConsoleKey.Enter:
                            Console.WriteLine("Input command:");
                            commandRaw = Console.ReadLine();

                            SendCommand(commandRaw);
                            break;
                        default:
                            break;
                    }
                }
            }
        }


        private static void SendCommand(string commandRaw) {

            string command, argument;

            if (!String.IsNullOrEmpty(commandRaw)) {
                if (commandRaw[0] == '/') {

                    commandRaw = commandRaw.Substring(1);

                    string[] commandsRaw = commandRaw.Split(" ");

                    command = commandsRaw[0];
                    argument = commandsRaw[1];


                    switch (command) {

                        case "say":

                            TCPServerSend.TCPSendPacketToAll(CreateMessagePacket(argument));
                            break;


                        default:

                            CommandLoop();
                            break;
                    }

                    reading = false;
                } else {

                    CommandLoop();
                }
            }
        }




        private static Packet CreateMessagePacket(string message) {

            Packet packet = new Packet((int)ServerPackets.message);

            packet.PacketWrite(message);
            CommandLoop();

            return packet;
        }
    }
}