using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Input;
//using System.Threading;
using Forms = System.Windows.Forms;

using Gnom_O_Chat.DAL;
using Gnom_O_Chat.EntityFr;
using Gnom_O_Chat.Repository;
using System.Threading;
using System.Windows.Threading;

namespace Gnom_O_Chat.UI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private IChatDAL _dal;
        public ChatUser curUser
        { get; set; }

        //DELEGATE FOR Threading.Timer
        private TimerCallback tcb;

        //DELEGATE FOR FUNC WHAT UPD THE CHAT
        private delegate void voidDel();
        private voidDel updDel;

        //SYNC TIMER, RIGHT NOW NO USE
        private Forms.Timer chatUpdateTimer = new Forms.Timer();

        //Threading.Timer
        private Timer ChatUpdateThreadTimer; 

        public MainWindow()
        {
            InitializeComponent();

            this._dal = new ChatDBmanager();

            chatUpdateTimer.Interval = 2000;
            chatUpdateTimer.Tick += chatUpdateTimer_Tick;

            this.tcb = TimerCllb;
            this.updDel = ChatUpdate;           
        }

        private void TimerCllb(object state)
        {
            Dispatcher.Invoke(updDel);
        }

        void chatUpdateTimer_Tick(object sender, EventArgs e)
        {
            ChatUpdate();
        }

        private void ChatUpdate()
        {
            if (this.tcChatTabs.Items.Count == 0)
            {
                return;
            }
            foreach (var item in this.tcChatTabs.Items)
            {
                TabItem ti = item as TabItem;
                string chatTitle = ((TextBlock)((StackPanel)ti.Header).Children[0]).Text;
                try
                {
                    var richcontrol = ((RichTextBox)((Grid)ti.Content).Children[1]);
                    Chat tabtag = ti.Tag as Chat;
                    List<MessageInfo> msgs = this._dal.GetNewMessages((int)richcontrol.Tag, chatTitle);

                    foreach (var msg in msgs)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("[");
                        sb.AppendFormat("{0}/{1}/{2}  {3}:{4}:{5}", msg.messageDate.Day, msg.messageDate.Month, msg.messageDate.Year,
                            msg.messageDate.Hour, msg.messageDate.Minute, msg.messageDate.Second);
                        sb.Append("]");
                        sb.AppendFormat("{0}: {1}", msg.userName, msg.message);
                        sb.AppendFormat("{0}", Environment.NewLine);

                        //var richcontrol = ((RichTextBox)((Grid)ti.Content).Children[1]);
                        if (richcontrol != null)
                        {
                            richcontrol.AppendText(sb.ToString());
                            richcontrol.Tag = this._dal.GetLastMessageId(chatTitle);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowException(ex);
                }
            }
        }



        void btnClose_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.DependencyObject Grandparent = ((Button)sender).Parent;
            System.Windows.DependencyObject parent = ((StackPanel)Grandparent).Parent;
            TabItem ti = parent as TabItem;
            this.tcChatTabs.Items.Remove(ti);
        }

        private void CreateNewTab(string ChatTitle)
        {
            //CREATE HEADER FOR TAB ITEM
            StackPanel stHeader = new StackPanel();
            stHeader.Orientation = Orientation.Horizontal;
            stHeader.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            TextBlock tbHeader = new TextBlock();
            Button btnClose = new Button();
            btnClose.Margin = new Thickness(3, 0, 0, 0);
            btnClose.Content = "X";
            btnClose.Background = Brushes.Red;
            btnClose.Click += btnClose_Click;

            tbHeader.Text = ChatTitle;
            stHeader.Children.Add(tbHeader);
            stHeader.Children.Add(btnClose);
            //END CREATE HEADER FOR TAB ITEM

            Grid tabGrid = new Grid();
            RowDefinition rd = new RowDefinition();
            tabGrid.RowDefinitions.Add(rd);
            rd = new RowDefinition();
            rd.Height = new GridLength(150, GridUnitType.Pixel);
            tabGrid.RowDefinitions.Add(rd);

            TextBox tbInput = new TextBox();
            tbInput.Margin = new Thickness(3, 3, 3, 3);
            tbInput.BorderThickness = new Thickness(2);
            tbInput.BorderBrush = Brushes.Black;
            tbInput.MaxLength = 299;
            tbInput.TextWrapping = TextWrapping.Wrap;
            tbInput.AcceptsReturn = true;
            tbInput.SetValue(Grid.RowProperty, 1);
            tbInput.KeyDown += tb_InputKeyDown;

            RichTextBox rtbOutput = new RichTextBox();
            rtbOutput.Margin = new Thickness(3, 3, 3, 3);
            rtbOutput.BorderThickness = new Thickness(2);
            rtbOutput.BorderBrush = Brushes.Black;
            rtbOutput.IsReadOnly = true;
            rtbOutput.SetValue(Grid.RowProperty, 0);
            rtbOutput.Tag = this._dal.GetLastMessageId(ChatTitle);

            tabGrid.Children.Add(tbInput);
            tabGrid.Children.Add(rtbOutput);

            Chat chatBound = this._dal.GetChatFromTitle(ChatTitle);

            TabItem ti = new TabItem();
            ti.Header = stHeader;
            ti.Content = tabGrid;
            ti.Tag = chatBound;

            this.tcChatTabs.Items.Add(ti);
            ((TabItem)this.tcChatTabs.Items[this.tcChatTabs.Items.Count - 1]).Focus();
        }

        private void tb_InputKeyDown(object sender, KeyEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (e.Key == Key.Return && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                string msg = tb.Text.Trim();
                if (string.IsNullOrEmpty(msg))
                {
                    tb.Text = "";
                    return;
                }

                DependencyObject parentGrid = tb.Parent;
                DependencyObject gridParentTab = ((Grid)parentGrid).Parent;
                TabItem ti = gridParentTab as TabItem;

                string chatTitle = ((TextBlock)((StackPanel)ti.Header).Children[0]).Text;
                try
                {
                    this._dal.AddMessageToHistory(this.curUser, chatTitle, tb.Text);
                }
                catch(Exception ex)
                {
                    ShowException(ex);
                }
                tb.Text = "";
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LogRegForm RegFormChild = new LogRegForm(this, this._dal);
            RegFormChild.ShowDialog();
            if (this.curUser != null)
            {
                try
                {
                    this._dal.AddConnection(this.curUser.IdUser, true);
                    this._dal.SetUserOnlineOffline(this.curUser, true);
                    this.SetTVChatViewDataSource();

                }
                catch(Exception ex)
                {
                    ShowException(ex);
                }

                CreateNewTab("MainChat");

                //this.chatUpdateTimer.Start();
                //this.PeriodicChatUpdate();
               
                ChatUpdateThreadTimer = new Timer(tcb, null, 1000, 1000);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.curUser != null)
            {
                try
                {
                    this._dal.AddConnection(this.curUser.IdUser, false);
                    this._dal.SetUserOnlineOffline(this.curUser, false);
                }
                catch(Exception ex)
                {
                    ShowException(ex);
                }
            }
            ChatUpdateThreadTimer.Dispose();
        }

        private void SetTVChatViewDataSource()
        {        
            try
            {
                this.tvChatView.ItemsSource = null;
                List<string> source = this._dal.GetListOfUserChats(this.curUser);
                this.tvChatView.ItemsSource = source;
            }
            catch (Exception ex)
            {
                ShowException(ex);
            }
        }
       


        private void ShowException(Exception ex)
        {
            StringBuilder sb = new StringBuilder(ex.Message);
            Exception inner = ex.InnerException;
            while (inner != null)
            {
                sb.Append("\n");
                sb.Append(inner.Message);
                inner = inner.InnerException;
            }
            MessageBox.Show(sb.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void MenuItem_Click_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void MenuItem_Click_LogOut(object sender, RoutedEventArgs e)
        {
            this.curUser = null;
        }

        private void btnLoginHistory(object sender, RoutedEventArgs e)
        {
            HistoryForm hiform = new HistoryForm(this, this._dal, HistoryFormStatus.LoginHistory);
            hiform.Show();
        }

        private void btnMessageHistory(object sender, RoutedEventArgs e)
        {
            HistoryForm hiform = new HistoryForm(this, this._dal, HistoryFormStatus.MessageHistory);
            hiform.Show();
        }

        private void btnAddNewChat_Click(object sender, RoutedEventArgs e)
        {
            string chatname = this.tbNewChatName.Text.Trim();
            if(chatname.Length < 3)
            {
                MessageBox.Show("ChatName to short!");
                this.tbNewChatName.Text = "";
                return;
            }

            try
            {
                this._dal.AddNewChat(chatname);
                Chat newChat = this._dal.GetChatFromTitle(chatname);
                this._dal.AddUserToChat(this.curUser, newChat.IdChat);
                this.CreateNewTab(chatname);
                this.tbNewChatName.Text = "";
            }
            catch(Exception ex)
            {
                this.ShowException(ex);
            }
        }

 
        private void AddToChat_Click(object sender, RoutedEventArgs e)
        {
            AddToChatForm child = new AddToChatForm(this.curUser, this._dal);
            child.ShowDialog();
        }

        private void btnRefreshChat_Click(object sender, RoutedEventArgs e)
        {
            this.SetTVChatViewDataSource();
        }

        private void tvChatView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (this.tvChatView.SelectedItem == null)
                return;

            this.CreateNewTab(this.tvChatView.SelectedItem.ToString());
        }

        private void LeaveFromChat_Click(object sender, RoutedEventArgs e)
        {
            LeaveFromChatForm child = new LeaveFromChatForm(this.curUser, this._dal);
            child.ShowDialog();
        }
    }
}
