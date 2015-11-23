using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Gnom_O_Chat.DAL;
using Gnom_O_Chat.EntityFr;
using Gnom_O_Chat.Repository;

namespace Gnom_O_Chat.UI
{
    public partial class AddToChatForm : Form
    {

        private IChatDAL _dal;
        public ChatUser curUser
        { get; set; }

        public AddToChatForm(ChatUser user, IChatDAL dal)
        {
            InitializeComponent();

            this._dal = dal;
            this.curUser = user;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddToChatForm_Load(object sender, EventArgs e)
        {
            this.lbChats.DataSource = this._dal.GetListOfUserChats(this.curUser);
        }

        private void lbChats_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lbChats.SelectedItem == null)
                return;

            this.lbUsers.DataSource = null;
            this.lbUsers.DataSource = this._dal.GetListOfUsersWhatCanBeAddedToChat(this.lbChats.SelectedItem.ToString(), this.curUser);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (this.lbChats.SelectedItem == null || this.lbUsers.SelectedItem == null)
                return;

            Chat chat = this._dal.GetChatFromTitle(this.lbChats.SelectedItem.ToString());
            ChatUser user = this._dal.GetUserByAcc(this.lbUsers.SelectedItem.ToString());
            this._dal.AddUserToChat(user, chat.IdChat);

            this.Close();
        }
    }
}
