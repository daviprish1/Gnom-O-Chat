namespace Gnom_O_Chat.UI
{
    partial class LeaveFromChatForm
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
            this.lbChats = new System.Windows.Forms.ListBox();
            this.btnLeave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbChats
            // 
            this.lbChats.FormattingEnabled = true;
            this.lbChats.Location = new System.Drawing.Point(12, 12);
            this.lbChats.Name = "lbChats";
            this.lbChats.Size = new System.Drawing.Size(166, 160);
            this.lbChats.TabIndex = 1;
            // 
            // btnLeave
            // 
            this.btnLeave.Location = new System.Drawing.Point(12, 177);
            this.btnLeave.Name = "btnLeave";
            this.btnLeave.Size = new System.Drawing.Size(83, 37);
            this.btnLeave.TabIndex = 2;
            this.btnLeave.Text = "Leave";
            this.btnLeave.UseVisualStyleBackColor = true;
            this.btnLeave.Click += new System.EventHandler(this.btnLeave_Click);
            // 
            // LeaveFromChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(202, 226);
            this.Controls.Add(this.btnLeave);
            this.Controls.Add(this.lbChats);
            this.Name = "LeaveFromChatForm";
            this.Text = "LeaveFromChatForm";
            this.Load += new System.EventHandler(this.LeaveFromChatForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbChats;
        private System.Windows.Forms.Button btnLeave;
    }
}