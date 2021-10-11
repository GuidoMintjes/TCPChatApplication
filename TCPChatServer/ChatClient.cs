using System;
using System.Net;
using System.Net.Sockets;

namespace TCPChatServer {

    public class ChatClient {

        public readonly static int dataBufferSize = 4096;    // Set the (default) buffer size to 4 megabytes
        public int clientID;
        public TCP tcp;


        public ChatClient(int _clientID) {

            clientID = _clientID;
            tcp = new TCP(clientID);
        }


        public class TCP {

            public TcpClient socket;    // Information stored that gets saved in server in the callback method
            private readonly int id;

            private NetworkStream stream;
            private byte[] receiveByteArray;


            public TCP(int _id) {

                id = _id;
            }


            public void Connect(TcpClient _socket) {

                //Console.WriteLine("Trying to connect to server...");

                socket = _socket;

                socket.ReceiveBufferSize = dataBufferSize;  // Set the send and receive buffer sizes to the declared
                socket.SendBufferSize = dataBufferSize;     // buffer sizes at the start of the client class

                stream = socket.GetStream();                    // Gets the 'stream' of info provided by the socket
                receiveByteArray = new byte[dataBufferSize];

                stream.BeginRead(receiveByteArray, 0, dataBufferSize, StreamReceiveCallback, null);


                TCPSend.WelcomeClient(id, $"Welcome to this secure channel! " +
                    $"My time is currently {DateTime.Now.ToString("HH:mm:ss")}!");  // Send welcome message
            }


            // Send data to client through TCP
            public void SendData(Packet packet) {

                try {

                    if (socket != null) {

                        stream.BeginWrite(packet.GetPacketBytes(), 0, packet.GetPacketSize(), null, null);
                    }

            } catch (Exception exc) {

                    Funcs.printMessage(0, $"Unable to send data to client {id} through TCP, err msg: {exc}", false);
                }
            }


            // Method that gets called back on when client connects to server
            void StreamReceiveCallback(IAsyncResult aResult) {

                // Handle this in a try catch block to be able to handle crashes
                try {

                    int dataLength = stream.EndRead(aResult);   // Returns an integer indicating the amount of bytes read
                                                                // in the data 'stream'

                    if (dataLength <= 0) {
                        return;             // Return out of the method when no bytes have been read ==>
                                            // (amount of bytes read = 0)
                    }


                    byte[] dataReceived = new byte[dataLength];             // Move the received data to a local variable...
                    Array.Copy(receiveByteArray, dataReceived, dataLength); // ...


                    // Start reading data from the stream again (if this would not be done the client would stop functioning
                    // here pretty much, just like if the server would stop listening for new connections)
                    stream.BeginRead(receiveByteArray, 0, dataBufferSize, StreamReceiveCallback, null);


                } catch (Exception exc) {

                    Console.WriteLine("Disconnected due to error: " + exc.Message);
                }
            }
        }
    }
}