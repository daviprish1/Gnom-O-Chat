using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Gnom_O_Chat.Repository;
using Gnom_O_Chat.DAL;

namespace Gnom_O_Chat.UI
{
    public partial class HistoryForm : Form
    {
        HistoryFormStatus status;
        MainWindow _parent;
        private IChatDAL _dal;

        public HistoryForm(MainWindow parent, IChatDAL dal, HistoryFormStatus status)
        {
            InitializeComponent();

            this.status = status;
            this._dal = dal;
            this._parent = parent;

            this.cbChats.DataSource = this._dal.GetListOfChats();
            this.cbUsers.DataSource = this._dal.GetListOfUsers();

            if(status == HistoryFormStatus.LoginHistory)
            {
                this.checkbForAllChats.Enabled = false;
            }
        }

        private void checkbForAllUsers_CheckedChanged(object sender, EventArgs e)
        {
            if(this.checkbForAllUsers.Checked)
            {
                this.cbUsers.Enabled = false;
            }
            else
            {
                this.cbUsers.Enabled = true;
            }
        }

        private void cbBetweenDate_CheckedChanged(object sender, EventArgs e)
        {
            if(this.cbBetweenDate.Checked)
            {
                this.dtpFirstDate.Enabled = true;
                this.dtpSecDate.Enabled = true;
            }
            else
            {
                this.dtpFirstDate.Enabled = false;
                this.dtpSecDate.Enabled = false;
            }
        }

        private void checkbForAllChats_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkbForAllChats.Checked)
            {
                this.cbChats.Enabled = false;
            }
            else
            {
                this.cbChats.Enabled = true;
            }
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            if(this.status == HistoryFormStatus.LoginHistory)
            {
                this.GetLoginHistory();
            }
            else
            {
                GetMessageHistory();
            }
        }

        private void GetLoginHistory()
        {
            this.tbHistory.Text = "";
            List<ConnectionInfo> history = new List<ConnectionInfo>();

            if(cbBetweenDate.Checked && !checkbForAllUsers.Checked)
            {
                history = this._dal.GetConnectionInfoForSomeUserBetweenDates(this.cbUsers.SelectedItem.ToString(), 
                    this.dtpFirstDate.Value.Date, this.dtpSecDate.Value);
            }
            else if (!cbBetweenDate.Checked && !checkbForAllUsers.Checked)
            {
                history = this._dal.GetConnectionInfoForSomeUser(this.cbUsers.SelectedItem.ToString());
            }
            else if (cbBetweenDate.Checked && checkbForAllUsers.Checked)
            {
                history = this._dal.GetConnectionInfoForAllUsersBetweenDates(this.dtpFirstDate.Value.Date, this.dtpSecDate.Value);
            }
            else
            {
                history = this._dal.GetConnectionInfoForAllUsers();
            }

            if (history.Count < 1)
                return;

            foreach(var h in history)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("[");
                sb.AppendFormat("{0}/{1}/{2}  {3}:{4}:{5}", h.activitydate.Day, h.activitydate.Month, h.activitydate.Year,
                    h.activitydate.Hour, h.activitydate.Minute, h.activitydate.Second);
                sb.Append("]");
                sb.AppendFormat(" {0}: Is going Online: {1}", h.username, h.isLogin);
                sb.AppendFormat("{0}", Environment.NewLine);
                this.tbHistory.AppendText(sb.ToString());
            }
        }

        private void GetMessageHistory()
        {
            this.tbHistory.Text = "";
            List<MessageInfo> history = new List<MessageInfo>();

            if(!this.checkbForAllChats.Checked && !checkbForAllUsers.Checked && cbBetweenDate.Checked)
            {
                history = this._dal.GetMessageHistoryForSpecifiedChatAndUserBetweenDates(this.cbChats.SelectedItem.ToString(),
                    this.cbUsers.SelectedItem.ToString(), this.dtpFirstDate.Value.Date, this.dtpSecDate.Value);
            }
            else if (!this.checkbForAllChats.Checked && !checkbForAllUsers.Checked && !cbBetweenDate.Checked)
            {
                history = this._dal.GetMessageHistoryForSpecifiedChatAndUser(this.cbChats.SelectedItem.ToString(),
                    this.cbUsers.SelectedItem.ToString());
            }
            else if(!this.checkbForAllChats.Checked && checkbForAllUsers.Checked && cbBetweenDate.Checked)
            {
                history = this._dal.GetMessageHistoryForSpecifiedChatBetweenDates(this.cbChats.SelectedItem.ToString(),
                    this.dtpFirstDate.Value.Date, this.dtpSecDate.Value);
            }
            else if (!this.checkbForAllChats.Checked && checkbForAllUsers.Checked && !cbBetweenDate.Checked)
            {
                history = this._dal.GetMessageHistoryForSpecifiedChat(this.cbChats.SelectedItem.ToString());
            }
            else if (this.checkbForAllChats.Checked && checkbForAllUsers.Checked && cbBetweenDate.Checked)
            {
                history = this._dal.GetMessageHistoryForAllUsersBetweenDates(this.dtpFirstDate.Value.Date, this.dtpSecDate.Value);
            }
            else
            {
                history = this._dal.GetMessageHistoryForAllUsers();
            }

            foreach(var h in history)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("[");
                sb.AppendFormat("{0}/{1}/{2}  {3}:{4}:{5}", h.messageDate.Day, h.messageDate.Month, h.messageDate.Year,
                    h.messageDate.Hour, h.messageDate.Minute, h.messageDate.Second);
                sb.Append("]");
                sb.AppendFormat(" {0}: {1}", h.userName, h.message);
                sb.AppendFormat("{0}", Environment.NewLine);
                this.tbHistory.AppendText(sb.ToString());
            }
        }
    }
}
