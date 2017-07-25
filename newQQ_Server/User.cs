using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;

namespace newQQ_Server
{
    class User
    {
        private int uid;
        private string nickname;
        private IPEndPoint socket;

        public User(int uid, string nickname, IPEndPoint socket)
        {
            this.uid = uid;
            this.nickname = nickname;
            this.socket = socket;
        }

        public User() { }

        public int getUid()
        {
            return this.uid;
        }

        public string getNickname()
        {
            return this.nickname;
        }

        public IPEndPoint getSocket()
        {
            return this.socket;
        }

        public void setSocket(IPEndPoint _socket)
        {
            this.socket = _socket;
        }
    }
}
