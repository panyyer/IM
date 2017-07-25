using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

using System.Web;
using MySql.Data.MySqlClient;

using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace newQQ_Server
{
    class Server
    {
        private const int port = 4016;

        private UdpClient MessageReceive;

        private List<User> onlineUsers = new List<User>();

        public Server()
        {
            

        }

        //运行服务器端程序
        public void start()
        {
            Thread messageThread = new Thread(receivMessage);
            messageThread.Start();
        }

        private void receivMessage()
        {
            IPEndPoint remote_point = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                try
                {
                    MessageReceive = new UdpClient(4016);
                    byte[] receiveBytes = MessageReceive.Receive(ref remote_point);
                    string receiveString = Encoding.UTF8.GetString(receiveBytes);
                    Console.WriteLine(receiveString);
                    string[] split = receiveString.Split(',');
                    string op = split[0];
                    switch (op)
                    {
                        case "login":
                            handleLogin(receiveString,remote_point);
                            break;
                        case "logout":
                            handleLogout(receiveString, remote_point);
                            break;
                        case "add":
                            handleAdd(receiveString, remote_point);
                            break;
                        case "chat":
                            handleChat(receiveString, remote_point);
                            break;
                        case "register":
                            handleRegister(receiveString, remote_point);
                            break;
                        case "update":
                            handleUpdate(receiveString, remote_point);
                            break;
                        default:
                            handleOther(receiveString, remote_point);
                            break;
                    }
                }
                catch(Exception ex)
                {
                    Console.Write(ex.Message);
                    break;
                }
            }
        }

        private void handleUpdate(string str, IPEndPoint remote_point)
        {
            string[] split = str.Split(',');
            int hostID = Convert.ToInt32(split[1]);
            for(int i=onlineUsers.Count-1; i>=0; i--)
            {
                if (onlineUsers[i].getUid() == hostID)
                {
                    onlineUsers[i].setSocket(remote_point);
                }
            }
        }

        private void handleRegister(string str, IPEndPoint remote_point)
        {
            string[] split = str.Split(',');
            string username = split[1];
            string password = split[2];
            string nickname = split[3];
            string message = null;

            String unique = String.Format("select * from user where username = '{0}'", username);
            MySqlDataReader dr = DBHelper.read(unique);
            if (dr.HasRows)
            {
                message = "fail,exist";
                dr.Close();
            }
            else
            {
                dr.Close();
                String sql = String.Format("insert into user(username,password,nickname) values('{0}','{1}','{2}')", username, password, nickname);
                if (DBHelper.nonQuery(sql) == 1)
                {
                    message = "succeed,succeed";
                }
                else
                {
                    message = "fail,error";
                }
            }
            MessageSend.sendtoClient(remote_point, message);
        }

        private void handleLogout(string str, IPEndPoint remote_point)
        {
            string[] split = str.Split(',');
            int hostID = Convert.ToInt32(split[1]);
            for (int i = onlineUsers.Count - 1; i >= 0; i--)
            {
                if (onlineUsers[i].getUid() == hostID && remote_point.Address.ToString() == onlineUsers[i].getSocket().Address.ToString())
                {
                    onlineUsers.Remove(onlineUsers[i]);
                    break;
                }
            }
        }

        private void handleOther(string str, IPEndPoint remote_point)
        {
            string[] split = str.Split(',');
            switch (split[1])
            {
                //好友请求结果反馈
                case "add" :
                    //friendID 和 hostID 分别为接受和发送好友请求的双方
                    string friendID = split[3];
                    string hostID = split[4];
                    MySqlDataReader dr = DBHelper.read("select * from user where id=" + friendID + ";");
                    string friendQQ = null , nickname = null, message = null;
                    while (dr.Read())
                    {
                        friendQQ = dr["username"].ToString();
                        nickname = dr["nickname"].ToString();
                    }
                    dr.Close();
                    if (split[2] == "accept")
                    {
                        //互为好友
                        MySqlDataReader dr2 = DBHelper.read(string.Format("select * from friends where hostID='{0}' and friendID='{1}';",hostID,friendID));
                        if (!dr2.HasRows)
                        {
                            dr2.Close();
                            int res1, res2;
                            res1 = DBHelper.nonQuery(string.Format("insert into friends(hostID,friendID) values('{0}','{1}');", friendID, hostID));
                            res2 = DBHelper.nonQuery(string.Format("insert into friends(hostID,friendID) values('{0}','{1}');", hostID, friendID));
                            if (res1 == 1 && res2 == 1)
                            {
                                message = "add,accept," + friendID + "," + nickname + "," + friendQQ;
                            }
                        }
                        else
                        {
                            dr2.Close();
                        }

                    }
                    else if (split[2] == "refuce")
                    {
                        message = "add,refuce," + friendID + "," + nickname + "," + friendQQ;
                    }
                    IPEndPoint userSocket = isOnline(int.Parse(hostID));
                    if (userSocket == null)
                    {
                        string sql = string.Format("insert into notify(toID,content,time) values('{0}','{1}','{2}');", hostID, message, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")); 
                        DBHelper.nonQuery(sql);
                    }
                    else
                    {
                        MessageSend.sendtoClient(userSocket, message);
                    }
                    break;

            }
        }

        //处理添加好友请求
        private void handleAdd(string str, IPEndPoint remote_point)
        {
            string[] split = str.Split(',');
            int hostID = int.Parse(split[1]);
            string friendQQ = split[2];
            string message = null;
            int friendID = -1;
            IPEndPoint userSocket = null;

            //检验是否合法用户
            for (int i = onlineUsers.Count-1; i >=0 ; i--)
            {
                if(onlineUsers[i].getUid() == hostID)
                {
                    userSocket = onlineUsers[i].getSocket();
                    break;
                }
            }

            //是否存在此Q号
            string sql = string.Format("select * from user where username = '{0}'", friendQQ);
            MySqlDataReader dr = DBHelper.read(sql);
            if (!dr.HasRows)
            {
                message = "fail,add";
            }
            else
            {
                while (dr.Read())
                {
                    friendID = (int)dr["id"];
                }
            }
            dr.Close();

            //已经是好友
            sql = string.Format("select * from friends where hostID='{0}' and friendID='{1}';", hostID, friendID);
            MySqlDataReader dr2 = DBHelper.read(sql);
            if (dr2.HasRows)
            {
                message = "fail,add";
            }
            dr2.Close();

            if (message == null)
            {
                message = "succeed,add";
            }

            //如果请求成功，则向指定好友发送添加请求
            if (message == "succeed,add" && friendID != -1)
            {
                sql = string.Format("select * from user where id = '{0}';", hostID);
                MySqlDataReader dr3 = DBHelper.read(sql);
                string hostQQ = null;
                string hostNickname = null;
                while (dr3.Read())
                {
                    hostQQ = dr3["username"].ToString();
                    hostNickname = dr3["nickname"].ToString();
                }
                dr3.Close();
                //检测对方是否在线
                int i;
                for (i = onlineUsers.Count - 1; i >= 0; i--)
                {
                    if (onlineUsers[i].getUid() == friendID)
                    {
                        string msg = string.Format("add,{0},{1},{2},{3}",hostID, hostQQ, hostNickname,DateTime.Now.ToString());
                        MessageSend.sendtoClient(onlineUsers[i].getSocket(), msg);
                        MessageSend.sendtoClient(userSocket, message);
                        break;
                    }
                }

                //不在线
                if (i == -1)
                {
                    sql = string.Format("insert into friendRequest(fromID,fromQQ,fromNickname,toID,type,isRead,time) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}');", hostID, hostQQ, hostNickname, friendID, 0, 0, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (DBHelper.nonQuery(sql) == 1)
                    {
                        MessageSend.sendtoClient(userSocket, message);
                    }
                }
            }
            else
            {
                MessageSend.sendtoClient(userSocket, message);
            }
        }

        private void handleChat(string receiveString, IPEndPoint remote_point)
        {
            try
            {
                //message格式：chat,hostID,friendID,content,ip:port
                string[] split = receiveString.Split(',');
                int friendID = int.Parse(split[2]);
                int hostID = int.Parse(split[1]);
                string content = split[3];
                int i;
                int cnt = onlineUsers.Count;
                string message = null;
                if (isFriend(hostID, friendID))
                {
                    for (i = 0; i < cnt; i++)
                    {
                        if (onlineUsers[i].getUid() == friendID)
                        {
                            message = string.Format("chat,{0},{1},{2}", hostID, content, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            MessageSend.sendtoClient(onlineUsers[i].getSocket(), message);

                            //反馈：给好友发送信息成功
                            message = "succeed,chat,"+friendID;
                            break;
                        }
                    }
                    //好友未上线
                    if (i == cnt)
                    {
                        //存入数据库
                        String sql = String.Format("insert into chat(hostID,friendID,content,time) values('{0}','{1}','{2}','{3}')", hostID, friendID, content, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        if (DBHelper.nonQuery(sql) == 1)
                        {
                            message = "succeed,chat,"+friendID;
                        }
                    }
                }
                //操作结果反馈
                if (message == null)
                {
                    message = "fail,chat";
                }
                MessageSend.sendtoClient(remote_point, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("chat error");
            }
        }

        private void handleLogin(string receiveString, IPEndPoint remote_point)
        {
            try
            {
  
                IPEndPoint userSocket = null;
                string[] split = receiveString.Split(',');
                IPAddress remote_ip = remote_point.Address;
                int port = remote_point.Port;
                string op = split[0];
                string username = split[1];
                string password = split[2];
                string message = null;
                string sql = string.Format("select * from user where username='{0}' and password='{1}';", username, password);
                MySqlDataReader dr = DBHelper.read(sql);
                IPEndPoint t = null;
                User u = null;
                if (dr.HasRows)
                {
                    message = "accept,";
                    while (dr.Read())
                    {
                        if ((t = isOnline((int)dr["id"])) != null)
                        {
                            string offline = "offline";
                            MessageSend.sendtoClient(t, offline);
                            for (int i = onlineUsers.Count - 1; i >= 0; i--)
                            {
                                if (onlineUsers[i].getSocket() == t)
                                {
                                    onlineUsers.Remove(onlineUsers[i]);
                                    break;
                                }
                            }
                        }
                        userSocket = new IPEndPoint(remote_ip, port);
                        u = new User((int)dr["id"], (string)dr["nickname"], userSocket);
                        onlineUsers.Add(u);
                        message += dr["nickname"] + "," + dr["id"] + ",";
                    }
                    dr.Close();
                    if (u != null)
                    {
                        string findFriends = String.Format("select u.nickname,u.id from user u, friends f where f.hostID='{0}' and f.friendID=u.id;", u.getUid());
                        MySqlDataReader dr2 = DBHelper.read(findFriends);
                        while (dr2.Read())
                        {
                            message += (int)dr2["id"] + "/" + (string)dr2["nickname"] + ";";
                        }
                        dr2.Close();
                    }
                }
                else
                {
                    userSocket = new IPEndPoint(remote_ip, port);
                    dr.Close();
                    message = "refuce";
                }
     
                MessageSend.sendtoClient(userSocket, message);
                Thread.Sleep(500);
                if(u!=null)
                    checkSend(u.getUid(),u.getSocket());   //检查未读的消息
            }
            catch (Exception ex)
            {
                Console.WriteLine("login error:"+ex.Message);
            }
        }

        private void checkSend(int uid, IPEndPoint userSocket)
        {
            //未读的好友信息
            string sql = string.Format("select * from chat where friendID='{0}' order by time desc",uid);
            MySqlDataReader dr = DBHelper.read(sql);
            string message = null;
            while (dr.Read())
            {
                message = string.Format("chat,{0},{1},{2}", dr["hostID"], dr["content"], Convert.ToDateTime(dr["time"]).ToString("yyyy-MM-dd HH:mm:ss"));
                MessageSend.sendtoClient(userSocket, message);
            }
            dr.Close();
            string del = string.Format("delete from chat where friendID='{0}'", uid);
            DBHelper.nonQuery(del);

            //未读的好友请求
            string sql2 = string.Format("select * from friendrequest where toID='{0}' and isRead=0 and type=0;", uid);
            MySqlDataReader dr2 = DBHelper.read(sql2);
            string message2 = null;
            while (dr2.Read())
            {
                message2 = string.Format("add,{0},{1},{2},{3}", dr2["fromID"], dr2["fromQQ"], dr2["fromNickname"], dr2["time"]);
                MessageSend.sendtoClient(userSocket, message2);
            }
            dr2.Close();
            string del2 = string.Format("delete from friendrequest where toID='{0}'", uid);
            DBHelper.nonQuery(del2);

            //未读的系统通知
            string sql3 = string.Format("select * from notify where toID='{0}';", uid);
            MySqlDataReader dr3 = DBHelper.read(sql3);
            string message3 = null;
            while (dr3.Read())
            {
                message3 = dr3["content"].ToString();
                MessageSend.sendtoClient(userSocket, message3);
            }
            dr3.Close();
            string del3 = string.Format("delete from notify where toID='{0}'", uid);
            DBHelper.nonQuery(del3);
        }

        private bool isFriend(int hostID, int friendID)
        {
            string sql = string.Format("select * from friends where hostID='{0}' and friendID='{1}';",hostID,friendID);
            MySqlDataReader dr = DBHelper.read(sql);
            if (dr.HasRows)
            {
                dr.Close();
                return true;
            }
            dr.Close();
            return false;
        }

        private IPEndPoint isOnline(int id)
        {
            for (int i = onlineUsers.Count - 1; i >= 0; i--)
            {
                if (onlineUsers[i].getUid() == id)
                {
                    return onlineUsers[i].getSocket();
                }
            }
            return null;
        }



    }
}
