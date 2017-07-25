namespace newQQ
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.addButton = new System.Windows.Forms.ToolStripButton();
            this.messageButton = new System.Windows.Forms.ToolStripButton();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.messageTimer = new System.Windows.Forms.Timer(this.components);
            this.faceTimer = new System.Windows.Forms.Timer(this.components);
            this.sb = new Aptech.UI.SideBar();
            this.ilFaces = new System.Windows.Forms.ImageList(this.components);
            this.sendSocket = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackColor = System.Drawing.Color.AliceBlue;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addButton,
            this.messageButton});
            this.toolStrip1.Location = new System.Drawing.Point(-2, 414);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(256, 30);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "hunsh";
            // 
            // addButton
            // 
            this.addButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.addButton.Image = ((System.Drawing.Image)(resources.GetObject("addButton.Image")));
            this.addButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(23, 27);
            this.addButton.Text = "添加好友";
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // messageButton
            // 
            this.messageButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.messageButton.Image = ((System.Drawing.Image)(resources.GetObject("messageButton.Image")));
            this.messageButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.messageButton.Name = "messageButton";
            this.messageButton.Size = new System.Drawing.Size(23, 27);
            this.messageButton.Text = "系统消息";
            this.messageButton.Click += new System.EventHandler(this.messageButton_Click);
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(121, 367);
            this.treeView1.Name = "treeView1";
            this.treeView1.ShowLines = false;
            this.treeView1.ShowRootLines = false;
            this.treeView1.Size = new System.Drawing.Size(55, 33);
            this.treeView1.TabIndex = 6;
            // 
            // messageTimer
            // 
            this.messageTimer.Interval = 500;
            this.messageTimer.Tick += new System.EventHandler(this.messageTimer_Tick);
            // 
            // faceTimer
            // 
            this.faceTimer.Enabled = true;
            this.faceTimer.Interval = 500;
            this.faceTimer.Tick += new System.EventHandler(this.faceTimer_Tick);
            // 
            // sb
            // 
            this.sb.AllowDragItem = false;
            this.sb.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.sb.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.sb.FlatStyle = Aptech.UI.SbFlatStyle.Normal;
            this.sb.GroupHeaderBackColor = System.Drawing.Color.White;
            this.sb.GroupTextColor = System.Drawing.Color.Black;
            this.sb.ImageList = this.ilFaces;
            this.sb.ItemContextMenuStrip = null;
            this.sb.ItemStyle = Aptech.UI.SbItemStyle.PushButton;
            this.sb.ItemTextColor = System.Drawing.Color.Black;
            this.sb.Location = new System.Drawing.Point(-2, -1);
            this.sb.Name = "sb";
            this.sb.RadioSelectedItem = null;
            this.sb.Size = new System.Drawing.Size(256, 412);
            this.sb.TabIndex = 7;
            this.sb.View = Aptech.UI.SbView.LargeIcon;
            this.sb.VisibleGroup = null;
            this.sb.VisibleGroupIndex = -1;
            this.sb.ItemDoubleClick += new Aptech.UI.SbItemEventHandler(this.sb_ItemDoubleClick);
            // 
            // ilFaces
            // 
            this.ilFaces.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilFaces.ImageStream")));
            this.ilFaces.TransparentColor = System.Drawing.Color.Empty;
            this.ilFaces.Images.SetKeyName(0, "1");
            this.ilFaces.Images.SetKeyName(1, "back.bmp");
            this.ilFaces.Images.SetKeyName(2, "9fdca9773912b31bc6ab50768618367adbb4e178.png");
            // 
            // sendSocket
            // 
            this.sendSocket.Interval = 120000;
            this.sendSocket.Tick += new System.EventHandler(this.sendSocket_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(252, 442);
            this.Controls.Add(this.sb);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "MainForm";
            this.Text = "好友";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton addButton;
        private System.Windows.Forms.ToolStripButton messageButton;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Timer messageTimer;
        private System.Windows.Forms.Timer faceTimer;
        private Aptech.UI.SideBar sb;
        private System.Windows.Forms.ImageList ilFaces;
        private System.Windows.Forms.Timer sendSocket;
    }
}