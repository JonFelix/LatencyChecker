using System;
using WinForms = System.Windows.Forms;
using System.Drawing;
using System.Windows;

namespace Ping
{
    public class TrayIcon
    {
        WinForms.NotifyIcon _icon;
        MainWindow _host;

        public TrayIcon(MainWindow host)
        {
            _host = host;
            _icon = new WinForms.NotifyIcon();
            _icon.Icon = new System.Drawing.Icon("icon.ico");
            _icon.Visible = true;
           _icon.DoubleClick +=
                delegate (object sender, EventArgs args)
                {
                    if(_host.IsVisible)
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
