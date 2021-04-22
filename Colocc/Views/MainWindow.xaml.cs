using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace Colocc.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            InitialTray();
        }


        #region 制作右下角的图标方法。
        //声明一个NotifyIcon类型的变量。
        private NotifyIcon _notifyIcon = null;
        private bool _shutDownFlag=true;

        /// <summary>
        /// 此方法用于制作右下角的图标。
        /// </summary>
        private void InitialTray()
        {
            //窗体打开的时候隐藏。
            //this.Visibility = Visibility.Hidden;
            //实例化NotifyIcon以及为其设置属性。
            _notifyIcon = new NotifyIcon();
            //当运行程序的时候右下角先显示服务开启中。
            _notifyIcon.BalloonTipText = "服务开启中。";
            //服务开启中这几个字显示的秒数。
            _notifyIcon.ShowBalloonTip(2000);
            //当光标放在上面之后，显示视频预览几个字。
            _notifyIcon.Text = "休息计划";
            //图标在通知栏区域中可见。
            _notifyIcon.Visible = true;

            //设置图标。
            _notifyIcon.Icon = Properties.Resources.f81;
            //设置菜单栏。有3个item选项，分别是显示、隐藏、退出。并为其添加事件。
            System.Windows.Forms.MenuItem showMenuItem = new System.Windows.Forms.MenuItem("显示");
            showMenuItem.Click += new EventHandler(ShowMenuItem_Click);
            System.Windows.Forms.MenuItem hideMenuItem = new System.Windows.Forms.MenuItem("隐藏");
            hideMenuItem.Click += new EventHandler(HideMenuItem_Click);
            System.Windows.Forms.MenuItem quitMenuItem = new System.Windows.Forms.MenuItem("退出");
            quitMenuItem.Click += new EventHandler(QuitMenuItem_Click);
            //将上面的3个自选项加入到parentMenuitem中。
            System.Windows.Forms.MenuItem[] parentMenuitem = new System.Windows.Forms.MenuItem[] { showMenuItem, hideMenuItem, quitMenuItem };
            //为notifyIconContextMenu。
            _notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(parentMenuitem);
            //notifyIcon的MouseDown事件。
            _notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(NotifyIcon_MouseClick);
            //窗体的StateChanged事件。
            this.StateChanged += new EventHandler(MainWindow_StateChanged);
            //不让在任务栏显示。
            this.ShowInTaskbar = false;
        }

        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            //如果最小化就隐藏。
            if (this.WindowState == System.Windows.WindowState.Minimized)
            {
                this.Visibility = System.Windows.Visibility.Hidden;
            }
        }
        private void NotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            //如果点击了鼠标左键。
            if (e.Button == MouseButtons.Left)
            {
                //如果隐藏就显示，否则就隐藏。
                if (this.Visibility == System.Windows.Visibility.Visible)
                {
                    this.Hide();
                }
                else
                {
                    this.Visibility = System.Windows.Visibility.Visible;
                    //将窗口至于前台并激活。
                    this.Activate();
                }
            }
        }
        private void QuitMenuItem_Click(object sender, EventArgs e)
        {
            if (System.Windows.MessageBox.Show("你确定要退出吗？", "温馨提醒。", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                _shutDownFlag = false;
                this.Close();
            }
        }
        private void HideMenuItem_Click(object sender, EventArgs e)
        {
            //如果此窗口是显示模式就隐藏。
            if (this.Visibility == System.Windows.Visibility.Visible)
            {
                this.Visibility = System.Windows.Visibility.Hidden;
            }
        }
        private void ShowMenuItem_Click(object sender, EventArgs e)
        {
            //如果此窗口是隐藏模式就显示。
            if (this.Visibility == System.Windows.Visibility.Hidden)
            {
                this.Visibility = System.Windows.Visibility.Visible;
            }
        }

        #endregion

        private void Mian_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_shutDownFlag)
            {
                e.Cancel = true;
                this.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                e.Cancel = false;
            }
        }
    }
}
