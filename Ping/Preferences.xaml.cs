using System.Windows.Controls;

namespace Ping
{
    /// <summary>
    /// Interaction logic for Preferences.xaml
    /// </summary>
    public partial class Preferences
    {
        MainWindow _host;

        public Preferences(MainWindow host)
        {
            _host = host;
            InitializeComponent();
            serverList.Text = _host.Settings.ServerListContent;
        }

        void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string[] _serverList = serverList.Text.Split('\n');
            for(int i = 0; i < _serverList.Length; i++)
            {                                     
                for(int x = 0; x < _host.Operations.Length; x++)
                {
                    if(_serverList[i] == _host.Operations[x].OriginalHost)
                    {
                        continue;
                    }
                    else
                    {
                        
                    }
                }
            }
        }
    }
}
