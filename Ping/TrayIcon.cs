using WinForms = System.Windows.Forms;

namespace Ping
{
    public class TrayIcon
    {
        WinForms.NotifyIcon _icon;
        public TrayIcon()
        {
            _icon = new WinForms.NotifyIcon
            {
                Icon = Properties.Resources.Icon1,
                Visible = true
            };

            /* _icon.DoubleClick +=
     delegate (object sender, EventArgs args)
     {
         this.Show();
         this.WindowState = WindowState.Normal;
     };*/
        }
    }
}
