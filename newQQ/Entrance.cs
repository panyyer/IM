using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows.Forms;
using System.Text.RegularExpressions;

using System.Security.Cryptography;

using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace newQQ
{
    public partial class Entrance : Form
    {
        public static int uid;

        string succeed = null; //登录成功标志
        string[] friendsID = null;

        public Entrance()
        {
            InitializeComponent();

            //允许线程间操作组件
            CheckForIllegalCrossThreadCalls = false;
        }

        private void Login_Load(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(usernameBox,"请输入非零开头的6位数字");
        }

        private void Entrance_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string username = textBox1.Text;
                string password = HttpUtility.UrlEncode(textBox2.Text);

                Thread receiveThread = new Thread(receiveMessage);
                receiveThread.Start();
                string message = "login," + username + "," + password;
                MessageSend.sendToServer(message);
                while (succeed == null) { continue; } //循环等待服务器返回成功信息
                if (succeed == "true")
                {
                     receiveThread.Abort();
                     this.Visible = false;
                     MainForm mf = new MainForm(friendsID);
                     mf.Text = Client.getNickname();
                     mf.Show();
                }
                else if (succeed == "false")
                {
                    succeed = null;
                    MessageBox.Show("用户名密码错误");
                }
                else if (succeed == "invalid")
                {
                    succeed = null;
                    MessageBox.Show("该账号已在憋出登录");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void receiveMessage()
        {
            IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                try
                {
                    byte[] receiveBytes = Client.udp.Receive(ref remoteIPEndPoint);
                    string message = Encoding.UTF8.GetString(receiveBytes, 0, receiveBytes.Length);
                    // 处理消息
                    string[] splitstring = message.Split(',');
                    switch (splitstring[0])
                    {
                        case "accept":
                            Client.setUid(int.Parse(splitstring[2]));
                            Client.setNickname(HttpUtility.UrlDecode(splitstring[1]));
                            if (splitstring[3] != null)
                            {
                                friendsID = splitstring[3].Split(';');
                            }
                            succeed = "true";
                            break;
                        case "refuce":
                            succeed = "false";
                            break;
                        case "invalid":
                            succeed = "invalid";
                            break;
                    }
                    if (succeed != null)
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string username = usernameBox.Text;
            string password = HttpUtility.UrlEncode(passwordBox.Text);
            string nickname = HttpUtility.UrlEncode(nicknameBox.Text);
            Thread registerThread = new Thread(receiveRegister);

            Regex re = new Regex(@"^[1-9]\d{5}");
            if (username.Length == 6 && re.Match(username) != null)
            {
                string message = string.Format("register,{0},{1},{2}", username, password, nickname);
                MessageSend.sendToServer(message);
                registerThread.Start();
            }
            else
            {
                MessageBox.Show("账号不合法");
            }
        }

        private void receiveRegister()
        {
            IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                try
                {
                    byte[] receiveBytes = Client.udp.Receive(ref remoteIPEndPoint);
                    string message = Encoding.UTF8.GetString(receiveBytes, 0, receiveBytes.Length);

                    // 处理消息
                    string[] splitstring = message.Split(',');
                    switch (splitstring[0])
                    {
                        case "succeed":
                            MessageBox.Show("注册成功");
                            break;
                        case "fail":
                            if (splitstring[1] == "exist")
                            {
                                MessageBox.Show("账号已经被注册");
                            }
                            else
                            {
                                MessageBox.Show("系统错误");
                            }
                            break;
                    }
                    break;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
