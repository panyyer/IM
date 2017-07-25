using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;

namespace newQQ_Server
{
    class MessageSend
    {

        private static IPEndPoint server = new IPEndPoint(IPAddress.Parse("xxx.xxx.xxx.xxx"), 4016);

        private static UdpClient sendUdpClient = new UdpClient(4016);

        public static void sendtoClient(IPEndPoint socket, string message)
        {
            byte[] sendBytes = Encoding.UTF8.GetBytes(message);
            Console.WriteLine(socket.Address.ToString() + ":" + socket.Port.ToString() + " " + message.ToString());
            sendUdpClient.Send(sendBytes, sendBytes.Length, socket);
        }

        public static IPEndPoint getServer()
        {
            return server;
        }
    }
}
