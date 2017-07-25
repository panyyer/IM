namespace newQQ
{
    partial class RequestForm
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
            this.accept = new System.Windows.Forms.Button();
            this.refuced = new System.Windows.Forms.Button();
            this.contentBox = new System.Windows.Forms.TextBox();
            this.ok = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // accept
            // 
            this.accept.Location = new System.Drawing.Point(177, 111);
            this.accept.Name = "accept";
            this.accept.Size = new System.Drawing.Size(75, 23);
            this.accept.TabIndex = 0;
            this.accept.Text = "接受";
            this.accept.UseVisualStyleBackColor = true;
            this.accept.Click += new System.EventHandler(this.accept_Click);
            // 
            // refuced
            // 
            this.refuced.Location = new System.Drawing.Point(274, 111);
            this.refuced.Name = "refuced";
            this.refuced.Size = new System.Drawing.Size(75, 23);
            this.refuced.TabIndex = 1;
            this.refuced.Text = "拒绝";
            this.refuced.UseVisualStyleBackColor = true;
            this.refuced.Click += new System.EventHandler(this.refuced_Click);
            // 
            // contentBox
            // 
            this.contentBox.BackColor = System.Drawing.SystemColors.Control;
            this.contentBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.contentBox.Location = new System.Drawing.Point(65, 28);
            this.contentBox.Multiline = true;
            this.contentBox.Name = "contentBox";
            this.contentBox.Size = new System.Drawing.Size(217, 54);
            this.contentBox.TabIndex = 2;
            // 
            // ok
            // 
            this.ok.Location = new System.Drawing.Point(133, 111);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 3;
            this.ok.Text = "确定";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.ok_Click);
            // 
            // RequestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 146);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.contentBox);
            this.Controls.Add(this.refuced);
            this.Controls.Add(this.accept);
            this.Name = "RequestForm";
            this.Text = "好友验证";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button accept;
        private System.Windows.Forms.Button refuced;
        private System.Windows.Forms.TextBox contentBox;
        private System.Windows.Forms.Button ok;
    }
}