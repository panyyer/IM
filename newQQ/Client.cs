using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace newQQ
{
    class Client
    {
        private static int port = new Random().Next(5000, 65535);
        //private static int port = 11000;
        private static int uid = -1;
        private static string nickname = null;
        public static UdpClient udp = new UdpClient(port);
        public static IPAddress getIP()
        {
            foreach (IPAddress i in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                //从ip地址簇中获取ipv4地址
                if (i.AddressFamily.ToString() == "InterNetwork")
                {
                    return IPAddress.Parse(i.ToString());
                }
            }
            return null;
        }

        public static int getPort()
        {
           return port;
        }

        public static UdpClient getUdpClient()
        {
            udp = new UdpClient(port);
            return udp;
        }

        public static int getUid()
        {
            return uid;
        }

        public static void setUid(int id)
        {
            uid = id;
        }

        public static void setNickname(string name)
        {
            nickname = name;
        }

        public static string getNickname()
        {
            return nickname;
        }
    }
}
