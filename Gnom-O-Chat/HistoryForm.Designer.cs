namespace Gnom_O_Chat.UI
{
    partial class HistoryForm
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
            this.tbHistory = new System.Windows.Forms.TextBox();
            this.checkbForAllChats = new System.Windows.Forms.CheckBox();
            this.cbChats = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtpSecDate = new System.Windows.Forms.DateTimePicker();
            this.dtpFirstDate = new System.Windows.Forms.DateTimePicker();
            this.cbBetweenDate = new System.Windows.Forms.CheckBox();
            this.checkbForAllUsers = new System.Windows.Forms.CheckBox();
            this.cbUsers = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.gbUsers = new System.Windows.Forms.GroupBox();
            this.btnGet = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.gbUsers.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbHistory
            // 
            this.tbHistory.Location = new System.Drawing.Point(12, 12);
            this.tbHistory.Multiline = true;
            this.tbHistory.Name = "tbHistory";
            this.tbHistory.Size = new System.Drawing.Size(413, 340);
            this.tbHistory.TabIndex = 0;
            // 
            // checkbForAllChats
            // 
            this.checkbForAllChats.AutoSize = true;
            this.checkbForAllChats.Checked = true;
            this.checkbForAllChats.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkbForAllChats.Location = new System.Drawing.Point(7, 21);
            this.checkbForAllChats.Name = "checkbForAllChats";
            this.checkbForAllChats.Size = new System.Drawing.Size(55, 17);
            this.checkbForAllChats.TabIndex = 1;
            this.checkbForAllChats.Text = "For All";
            this.checkbForAllChats.UseVisualStyleBackColor = true;
            this.checkbForAllChats.CheckedChanged += new System.EventHandler(this.checkbForAllChats_CheckedChanged);
            // 
            // cbChats
            // 
            this.cbChats.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbChats.Enabled = false;
            this.cbChats.FormattingEnabled = true;
            this.cbChats.Location = new System.Drawing.Point(68, 19);
            this.cbChats.Name = "cbChats";
            this.cbChats.Size = new System.Drawing.Size(121, 21);
            this.cbChats.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dtpSecDate);
            this.groupBox1.Controls.Add(this.dtpFirstDate);
            this.groupBox1.Controls.Add(this.cbBetweenDate);
            this.groupBox1.Location = new System.Drawing.Point(438, 93);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(211, 119);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Between";
            // 
            // dtpSecDate
            // 
            this.dtpSecDate.Enabled = false;
            this.dtpSecDate.Location = new System.Drawing.Point(13, 84);
            this.dtpSecDate.Name = "dtpSecDate";
            this.dtpSecDate.Size = new System.Drawing.Size(131, 20);
            this.dtpSecDate.TabIndex = 1;
            // 
            // dtpFirstDate
            // 
            this.dtpFirstDate.Enabled = false;
            this.dtpFirstDate.Location = new System.Drawing.Point(13, 46);
            this.dtpFirstDate.Name = "dtpFirstDate";
            this.dtpFirstDate.Size = new System.Drawing.Size(131, 20);
            this.dtpFirstDate.TabIndex = 1;
            // 
            // cbBetweenDate
            // 
            this.cbBetweenDate.AutoSize = true;
            this.cbBetweenDate.Location = new System.Drawing.Point(13, 23);
            this.cbBetweenDate.Name = "cbBetweenDate";
            this.cbBetweenDate.Size = new System.Drawing.Size(56, 17);
            this.cbBetweenDate.TabIndex = 0;
            this.cbBetweenDate.Text = "Active";
            this.cbBetweenDate.UseVisualStyleBackColor = true;
            this.cbBetweenDate.CheckedChanged += new System.EventHandler(this.cbBetweenDate_CheckedChanged);
            // 
            // checkbForAllUsers
            // 
            this.checkbForAllUsers.AutoSize = true;
            this.checkbForAllUsers.Checked = true;
            this.checkbForAllUsers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkbForAllUsers.Location = new System.Drawing.Point(8, 19);
            this.checkbForAllUsers.Name = "checkbForAllUsers";
            this.checkbForAllUsers.Size = new System.Drawing.Size(55, 17);
            this.checkbForAllUsers.TabIndex = 1;
            this.checkbForAllUsers.Text = "For All";
            this.checkbForAllUsers.UseVisualStyleBackColor = true;
            this.checkbForAllUsers.CheckedChanged += new System.EventHandler(this.checkbForAllUsers_CheckedChanged);
            // 
            // cbUsers
            // 
            this.cbUsers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUsers.Enabled = false;
            this.cbUsers.FormattingEnabled = true;
            this.cbUsers.Location = new System.Drawing.Point(69, 17);
            this.cbUsers.Name = "cbUsers";
            this.cbUsers.Size = new System.Drawing.Size(121, 21);
            this.cbUsers.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbChats);
            this.groupBox2.Controls.Add(this.checkbForAllChats);
            this.groupBox2.Location = new System.Drawing.Point(438, 238);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(211, 63);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Chats";
            // 
            // gbUsers
            // 
            this.gbUsers.Controls.Add(this.cbUsers);
            this.gbUsers.Controls.Add(this.checkbForAllUsers);
            this.gbUsers.Location = new System.Drawing.Point(437, 24);
            this.gbUsers.Name = "gbUsers";
            this.gbUsers.Size = new System.Drawing.Size(211, 60);
            this.gbUsers.TabIndex = 5;
            this.gbUsers.TabStop = false;
            this.gbUsers.Text = "Users";
            // 
            // btnGet
            // 
            this.btnGet.Location = new System.Drawing.Point(440, 319);
            this.btnGet.Name = "btnGet";
            this.btnGet.Size = new System.Drawing.Size(129, 32);
            this.btnGet.TabIndex = 6;
            this.btnGet.Text = "Get!";
            this.btnGet.UseVisualStyleBackColor = true;
            this.btnGet.Click += new System.EventHandler(this.btnGet_Click);
            // 
            // HistoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(672, 364);
            this.Controls.Add(this.btnGet);
            this.Controls.Add(this.gbUsers);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tbHistory);
            this.MaximumSize = new System.Drawing.Size(688, 402);
            this.Name = "HistoryForm";
            this.Text = "HistoryForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.gbUsers.ResumeLayout(false);
            this.gbUsers.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbHistory;
        private System.Windows.Forms.CheckBox checkbForAllChats;
        private System.Windows.Forms.ComboBox cbChats;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker dtpSecDate;
        private System.Windows.Forms.DateTimePicker dtpFirstDate;
        private System.Windows.Forms.CheckBox cbBetweenDate;
        private System.Windows.Forms.CheckBox checkbForAllUsers;
        private System.Windows.Forms.ComboBox cbUsers;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox gbUsers;
        private System.Windows.Forms.Button btnGet;
    }
}