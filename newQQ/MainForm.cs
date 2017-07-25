using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web;

using Aptech.UI;

using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Security.Cryptography;

namespace newQQ
{
    public partial class MainForm : Form
    {
        private List<ChatForm> cf = new List<ChatForm>();
        private List<chatMsg> msgBuffer = new List<chatMsg>(); //未读信息缓冲区
        private List<sysMsg> sysMsgBuffer = new List<sysMsg>();
        public Dictionary<int, string> friends = new Dictionary<int,string>();

        public MainForm(string[] _friends)
        {
            InitializeComponent();
            for (int i = 0; i < _friends.Length - 1; i++)
            {
                string[] t = _friends[i].Split('/');
                this.friends.Add(int.Parse(t[0]), HttpUtility.UrlDecode(t[1]));
            }
            showFriends();

            Thread receiveThread = new Thread(receiveMessage);
            receiveThread.Start();

            //允许线程间操作组件
            CheckForIllegalCrossThreadCalls = false;

            faceTimer.Start();
            messageTimer.Start();
            sendSocket.Start();
        }

        private void receiveMessage()
        {
            IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                try 
                {
                    byte[] receiveBytes = Client.udp.Receive(ref remoteIPEndPoint);
                    string receiveString = Encoding.UTF8.GetString(receiveBytes, 0, receiveBytes.Length);
                    string[] splitString = receiveString.Split(',');

                    switch(splitString[0])
                    {
                        case "chat":
                            handleChat(splitString);
                            break;
                        case "add": 
                            handleAdd(splitString);
                            break;
                        case "offline":
                            handleOffline(splitString);
                            break;
                        case "succeed":
                            handleSucceed(splitString);
                            break;
                        case "fail":
                            handleFail(splitString[1]);
                            break;
                    }
                }
                catch (Exception ex) 
                {
                   MessageBox.Show(ex.Message);
                }
            }
        }

        private void handleOffline(string[] str)
        {
            DialogResult dr = MessageBox.Show("你的账号已在别处上线");

            while(true) 
            {
                if (dr == DialogResult.OK)
                {
                    break;
                }
            }
            System.Environment.Exit(0);
        }

        private void handleAdd(string[] str)
        {
            string content = null;
            int friendID = -1;
            string nickname;
            string friendQQ;
            switch (str[1])
            {
                case "accept":
                    friendID = int.Parse(str[2]);
                    nickname = HttpUtility.UrlDecode(str[3]);
                    friendQQ = str[4];
                    content = string.Format("{0}（{1}）接受了您的好友请求",nickname,friendQQ);
                    sysMsgBuffer.Add(new sysMsg(-1, friendQQ, nickname, null, 1, content));
                    if (!friends.ContainsKey(friendID))
                    {
                        friends.Add(friendID, nickname);
                    }
                    //更新好友列表
                    showFriends();
                    break;
                case "refuce":
                    friendID = int.Parse(str[2]);
                    nickname = HttpUtility.UrlDecode(str[3]);
                    friendQQ = str[4];
                    content = string.Format("{0}（{1}）拒绝了您的好友请求", nickname, friendQQ);
                    sysMsgBuffer.Add(new sysMsg(-1, friendQQ, nickname, null, 1, content));
                    break;
                default :
                    friendID = int.Parse(str[1]);
                    nickname = HttpUtility.UrlDecode(str[3]);
                    friendQQ = str[2];
                    string time = str[4];
                    content = string.Format("{0}（{1}），请求添加您为好友。", nickname, friendQQ);
                    sysMsgBuffer.Add(new sysMsg(friendID, friendQQ, nickname, time, 0,content));
                    break;
            }

        }

        private void handleChat(string[] str)
        {
            bool flag = false;
            int friendID = int.Parse(str[1]);
            string content = str[2];
            string time = str[3];
         
            for (int i = cf.Count-1; i >=0 ; i--)
            {
                //窗体已经处于隐藏状态，在此处释放资源
                if (cf[i].Visible == false)
                {
                    ChatForm temp = cf[i];
                    cf.Remove(cf[i]);
                    temp.Dispose();
                    continue;
                }

                //如果与好友聊天的窗体已经打开，直接追加信息
                if (cf[i].getFriendID() == friendID)
                {
                    cf[i].addMessage(this.friends[friendID], time, content);
                    flag = true;
                    break;
                }
            }

            //如果对应聊天窗体没打开，加入消息缓冲区
            if (!flag)
            {
                msgBuffer.Add(new chatMsg(friendID, content, time));
            }
        }

        private void handleSucceed(string[] str)
        {
            if (str[1] == "chat")
            {
                //list倒序遍历，删除元素，保证其正确性
                for (int i = cf.Count-1; i >= 0; i--)
                {
                    if (cf[i].Visible == true)
                    {
                        if (cf[i].getFriendID() == int.Parse(str[2]))
                        {
                            cf[i].addMessage(Client.getNickname(), DateTime.Now.ToString(), cf[i].getSendMessage());
                            break;
                        }
                    }
                    else 
                    {
                        ChatForm temp = cf[i];
                        cf.Remove(cf[i]);
                        temp.Dispose();
                    }
                }
            }
            else if (str[1] == "add")
            {
                MessageBox.Show("请求发送成功");
            }

        }

        private void handleFail(string str)
        {
            if (str == "chat") 
            {
                MessageBox.Show("消息发送失败");
            }
            else if (str == "add")
            {
                MessageBox.Show("用户不存在或已经是好友");
            }

        }

        private void showFriends()
        {
            if (sb.Groups.Count == 0)
            {
                sb.AddGroup("我的好友");
                sb.AddGroup("陌生人");
            }

            //先清空原来的好友列表
            sb.Groups[0].Items.Clear();

            foreach (KeyValuePair<int,string> e in friends)
            {
                SbItem item = new SbItem(e.Value, 0);
                item.Tag = e.Key;
                sb.Groups[0].Items.Add(item);
            }

            /*
            TreeNode friend = new TreeNode();
            friend.Text = "我的好友";
            TreeNode stranger = new TreeNode();
            stranger.Text = "陌生人";
            treeView1.Nodes.Add(friend);
            treeView1.Nodes.Add(stranger);

            for (int i = 0; i < friends.Length - 1;i++ )
            {
                string[] t = friends[i].Split('/');
                TreeNode node = new TreeNode();
                node.Text = t[1].ToString();   //nickname
                node.Name = t[0].ToString();  //ID
                friend.Nodes.Add(node);
            }
            */
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            string message = "logout," + Client.getUid();
            MessageSend.sendToServer(message);
            System.Environment.Exit(0);
        }


        private void messageTimer_Tick(object sender, EventArgs e)
        {
            if (sysMsgBuffer.Count > 0)
            {
                messageButton.Visible = !messageButton.Visible;
            }
        }

        //好友头像闪烁
        private void faceTimer_Tick(object sender, EventArgs e)
        {
            int cnt = sb.Groups[0].Items.Count;
            //msgBuffer可能存在偶数条同一个好友发来的信息，这会使得闪烁连续执行两次，因此用HashSet过滤重复元素
            HashSet<int> set = new HashSet<int>();
            for (int j = msgBuffer.Count-1 ; j >=0 ; j--)
            {
                set.Add(msgBuffer[j].friendID);
            }

            for (int i = 0; i < cnt; i++)
            {
                if (set.Contains(Convert.ToInt32(sb.Groups[0].Items[i].Tag)))
                {
                    sb.Groups[0].Items[i].ImageIndex = sb.Groups[0].Items[i].ImageIndex == 0 ? 2 : 0;

                }
            }
            sb.Invalidate();
        }

        // 双击弹出聊天窗体        
        private void sb_ItemDoubleClick(SbItemEventArgs e)
        {
            //已经打开的窗体不能再打开
            for (int i = cf.Count-1; i >=0 ; i--)
            {
                if (cf[i].getFriendID() == Convert.ToInt32(e.Item.Tag) && cf[i].Visible == true)
                {
                    return;
                }
            }

            chatMsg[] removeMsg = new chatMsg[msgBuffer.Count];
            int cnt = 0;
            int friendID = Convert.ToInt32(e.Item.Tag);

            ChatForm c = new ChatForm(e.Item.Text,friendID);
            cf.Add(c);  //将打开的窗体加入窗体list中

            //展示窗体
            c.Show();

            //遍历缓冲区读取未读信息
            for (int i = msgBuffer.Count-1 ; i >=0 ; i--)
            {
                if (msgBuffer[i].friendID == friendID)
                {
                    c.addMessage(this.friends[friendID], msgBuffer[i].time, msgBuffer[i].content);
                    removeMsg[cnt++] = msgBuffer[i];
                }
            }

            //将已打开信息从缓冲区移走
            foreach (chatMsg i in removeMsg)
            {
                msgBuffer.Remove(i);
            }

            //还原头像
            e.Item.ImageIndex = 0;
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            new addFriend().Show();
        }

        private void messageButton_Click(object sender, EventArgs e)
        {
            for (int i = sysMsgBuffer.Count - 1; i >= 0; i--)
            {
                RequestForm rf = new RequestForm();
                rf.setContent(sysMsgBuffer[i].content, sysMsgBuffer[i].fromID, sysMsgBuffer[i].nickname, sysMsgBuffer[i].type, this);
                rf.Show();
            }

            //清空缓冲区
            sysMsgBuffer.Clear();

            messageButton.Visible = true;
        }

        //点击通过好友请求添加好友
        public void addFriend(int friendID, string nickname)
        {
            friends.Add(friendID, nickname);
            showFriends();
        }

        private void sendSocket_Tick(object sender, EventArgs e)
        {
            string message = "update," + Client.getUid();
            MessageSend.sendToServer(message);
        }
    }

    public class chatMsg
    {
        public int friendID;
        public string content;
        public string time;

        public chatMsg(int _friendID, string _content, string _time)
        {
            friendID = _friendID;
            content = _content;
            time = _time;
        }
    }

    public class sysMsg
    {
        public string nickname;
        public string qq;
        public string time;
        public int fromID;
        public int type;
        public string content;

        public sysMsg(int _fromID, string _qq, string _nickname, string _time, int _type, string _content)
        {
            fromID = _fromID;
            time = _time;
            qq = _qq;
            nickname = _nickname;
            type = _type;
            content = _content;
        }
    }
}
