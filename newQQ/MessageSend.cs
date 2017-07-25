using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace newQQ
{
    class MessageSend
    {

        private static IPEndPoint server = new IPEndPoint(IPAddress.Parse("xxx.xxx.xxx.xxx"), 4016);

        // 向客户端发送消息
        public static void sendToClient(IPEndPoint socket, string message)
        {
            try
            {
                byte[] sendBytes = Encoding.UTF8.GetBytes(message);
                Client.udp.Send(sendBytes, sendBytes.Length, socket);
            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.Message);
            }
         }

        public static void sendToServer(string message)
        {
            try
            {
                byte[] sendBytes = Encoding.UTF8.GetBytes(message);
                Client.udp.Send(sendBytes, sendBytes.Length, server);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
