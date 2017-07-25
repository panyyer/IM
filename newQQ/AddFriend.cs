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
    public partial class addFriend : Form
    {
        public addFriend()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String qq = textBox1.Text;
            string message = "add," + Client.getUid() + "," + qq;
            MessageSend.sendToServer(message);
        }
    }
}
