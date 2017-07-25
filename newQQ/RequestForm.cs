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

namespace newQQ
{
    public partial class RequestForm : Form
    {
        private int friendID;
        private string nickname;
        private MainForm mf;

        public RequestForm()
        {
            InitializeComponent();
        }

        public void setContent(string con, int _friendID, string _nickname, int type, MainForm _mf)
        {
            contentBox.Text = con;
            friendID = _friendID;
            nickname = _nickname;
            mf = _mf;
            ok.Visible = false;
            if (type == 1)
            {
                accept.Visible = false;
                refuced.Visible = false;
                ok.Visible = true;
            }
        }

        private void refuced_Click(object sender, EventArgs e)
        {
            string message = "other,add,refuce," + Client.getUid() + "," + friendID;
            MessageSend.sendToServer(message);
            this.Dispose();
        }

        private void accept_Click(object sender, EventArgs e)
        {
            string message = "other,add,accept," + Client.getUid() + "," + friendID;
            MessageSend.sendToServer(message);
            //刷新好友列表
            mf.addFriend(friendID, nickname);

          this.Dispose();
        }

        private void ok_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
