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
using System.Threading;

using Gnom_O_Chat.DAL;
using Gnom_O_Chat.EntityFr;
using Gnom_O_Chat.Repository;

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

        public MainWindow()
        {
            InitializeComponent();

            this._dal = new ChatDBmanager();
            getchatmsgs += GetNewChatMessages;
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
                }
                catch(Exception ex)
                {
                    ShowException(ex);
                }

                CreateNewTab("MainChat");

                this.PeriodicChatUpdate();
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
        }

        private Object thisLock = new Object();
        public delegate void getchatmsgsDel();
        public getchatmsgsDel getchatmsgs;

        public async void PeriodicChatUpdate()
        {
            while (true)
            {
                await Task.Run(() => getchatmsgs);
                //await this.Dispatcher.BeginInvoke(getchatmsgs, null);
            }
        }

        private async void GetNewChatMessages()
        {
            await Task.Run(() =>
                {
                    lock (thisLock)
                    {
                        Thread.Sleep(1500);

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
                                List<NewMessage> msgs = this._dal.GetNewMessages(((int)ti.Tag), chatTitle);

                                foreach (var msg in msgs)
                                {
                                    StringBuilder sb = new StringBuilder();
                                    sb.Append("[");
                                    sb.AppendFormat("{0}/{1}/{2}  {3}:{4}:{5}", msg.messageDate.Day, msg.messageDate.Month, msg.messageDate.Year,
                                        msg.messageDate.Hour, msg.messageDate.Minute, msg.messageDate.Second);
                                    sb.Append("]");
                                    sb.AppendFormat("{0}: {1}", msg.userName, msg.message);
                                    sb.AppendLine();

                                    var richcontrol = ((RichTextBox)((Grid)ti.Content).Children[1]);
                                    if (richcontrol != null)
                                    {
                                        Dispatcher.Invoke(() => richcontrol.AppendText(sb.ToString()));
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                ShowException(ex);
                            }
                        }
                    }
                });
        }//private void GetNewChatMessages()


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
    }
}
