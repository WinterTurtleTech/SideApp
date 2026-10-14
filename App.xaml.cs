using Forms = System.Windows.Forms;
using System.Windows;

namespace SideApp
{    
    public partial class App : Application
    {
        private readonly Forms.NotifyIcon _notifyIcon;
        public App()
        {
            _notifyIcon = new Forms.NotifyIcon();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            try { 
            _notifyIcon.Icon = new System.Drawing.Icon("SideApp.ico");
            }
            catch { }
            _notifyIcon.Text = "Open Kanji Side App";
            _notifyIcon.Visible = true;
            _notifyIcon.Click += NotifyIcon_Click;
            base.OnStartup(e);
        }

        private void NotifyIcon_Click(object sender, EventArgs e)
        { 
            MainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _notifyIcon.Dispose();
            base.OnExit(e);
        }
    }
}
