using System;
using WinForms = System.Windows.Forms;
using System.Drawing;

namespace Ping
{
    public class TrayIcon
    {
        WinForms.NotifyIcon _icon;
        public TrayIcon()
        {
            _icon = new WinForms.NotifyIcon();
            _icon.Icon = new System.Drawing.Icon("icon.ico");
            _icon.Visible = true;
           /* _icon.DoubleClick +=
                delegate (object sender, EventArgs args)
                {
                    this.Show();
                    this.WindowState = WindowState.Normal;
                };*/
        }
    }
}
