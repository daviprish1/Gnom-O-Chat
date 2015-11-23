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
    public partial class LeaveFromChatForm : Form
    {
        private IChatDAL _dal;
        public ChatUser curUser
        { get; set; }

        public LeaveFromChatForm(ChatUser user, IChatDAL dal)
        {
            InitializeComponent();

            curUser = user;
            this._dal = dal;
        }

        private void LeaveFromChatForm_Load(object sender, EventArgs e)
        {
            this.lbChats.DataSource = this._dal.GetListOfUserChats(curUser);
        }

        private void btnLeave_Click(object sender, EventArgs e)
        {
            if (this.lbChats.SelectedItem == null)
                return;

            this._dal.LeaveFromMembership(this.lbChats.SelectedItem.ToString(), this.curUser);
            this.Close();
        }
    }
}
