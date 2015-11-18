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

using Gnom_O_Chat.DAL;
using Gnom_O_Chat.EntityFr;

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

            tabGrid.Children.Add(tbInput);
            tabGrid.Children.Add(rtbOutput);

            TabItem ti = new TabItem();
            ti.Header = stHeader;
            ti.Content = tabGrid;

            this.tcChatTabs.Items.Add(ti);
            ((TabItem)this.tcChatTabs.Items[this.tcChatTabs.Items.Count - 1]).Focus();
        }

        private void tb_InputKeyDown(object sender, KeyEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (e.Key == Key.Return && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                tb.Text = "";
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LogRegForm RegFormChild = new LogRegForm(this, this._dal);
            RegFormChild.ShowDialog();
            if (this.curUser != null)
            {
                this._dal.AddConnection(this.curUser.IdUser, true);
                this._dal.SetUserOnlineOffline(this.curUser, true);
                CreateNewTab("Chat");
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.curUser != null)
            {
                this._dal.AddConnection(this.curUser.IdUser, false);
                this._dal.SetUserOnlineOffline(this.curUser, false);
            }
        }
    }
}
