using System;
using System.Windows;
using WinForms = System.Windows.Forms;

namespace Ping
{
    public class TrayIcon
    {
        WinForms.NotifyIcon _icon;
        MainWindow _host;
        public string Text
        {
            get
            {
                return _icon.BalloonTipText;
            }
            set
            {
                _icon.Text = value;
            }
        }
        public TrayIcon(MainWindow host)
        {
            _host = host;
            _icon = new WinForms.NotifyIcon();
            _icon.Icon = Properties.Resources.Icon1;
            _icon.Visible = true;
            _icon.DoubleClick +=
                 delegate (object sender, EventArgs args)
                 {
                     if (_host.IsVisible)
                     {
                         _host.Hide();
                     }
                     else
                     {
                         _host.Show();
                         _host.WindowState = WindowState.Normal;
                         _host.Focus();
                     }

                 };
        }
    }
}
