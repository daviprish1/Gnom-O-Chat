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

namespace Gnom_O_Chat.UI
{
    public partial class LogRegForm : Form
    {

        private MainWindow _parent;
        private IChatDAL _dal;

        public LogRegForm(MainWindow parent, IChatDAL dal)
        {
            InitializeComponent();
            this._parent = parent;
            this._dal = dal;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string username = this.tbUserName.Text.Trim();
            string pass = this.tbPass.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Empty pass or UserName, try again");
                this.tbUserName.Text = "";
                this.tbPass.Text = "";
                return;
            }

            if (username.Length < 3 || pass.Length < 3)
            {
                MessageBox.Show("Pass or UserName must have at least 3 characters");
                this.tbUserName.Text = "";
                this.tbPass.Text = "";
                return;
            }

            if (this.cbIsReg.Checked)
            {
                int mainIdx = this._dal.GetMainChatIdx();

                this._dal.AddChatUser(username, pass);
                this._dal.AddUserByNameToChat(username, mainIdx);
                Login(username, pass);
            }
            else
            {
                Login(username, pass);
            }
        }

        private void Login(string username, string pass)
        {
            try
            {
                ChatUser curUser = this._dal.GetUserByAccPass(username, pass);
                this._parent.curUser = curUser;
                if (curUser != null)
                {
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Incorrect name or pass!");
                    this.tbUserName.Text = "";
                    this.tbPass.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
