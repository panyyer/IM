using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Web;

namespace newQQ
{
    public partial class ChatForm : Form
    {
        private int friendID;
        private string  friendName;
        private string chatMessage;

        public ChatForm(string friendName,int friendID)
        {
            InitializeComponent();
            this.Text = "与 " + friendName + " 聊天";
            this.friendID = friendID;
            this.friendName = friendName;
        }

        private void send_Click(object sender, EventArgs e)
        {
            chatMessage = HttpUtility.UrlEncode(txSend.Text);
            string message = "chat," + Client.getUid() + "," + friendID.ToString() + "," + chatMessage;
            MessageSend.sendToServer(message);
            txSend.Text = null;
            //textBox1.Text = textBox1.Text+'\n'+this.friendName+"  "+
        }

        private void cancel_Click(object sender, EventArgs e)
        {

            this.Visible = false;
        }

        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
        }

        public TextBox getTextBox()
        {
            return textBox1;
        }

        public void addMessage(string name, string time, string content)
        {
            string append = string.Format("{0}  {1}：{2}{3}{4}{5}", name, time, Environment.NewLine, HttpUtility.UrlDecode(content), Environment.NewLine, Environment.NewLine);
            textBox1.AppendText(append);
            textBox1.ScrollToCaret();
        }

        public int getFriendID()
        {
            return friendID;
        }

        public string getSendMessage()
        {
            return chatMessage;
        }

    }
}
