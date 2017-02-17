using System;
using System.Collections.Generic;
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
            List<PingOperation> _tmpList = new List<PingOperation>();
            for(int i = 0; i < _serverList.Length; i++)
            {
                _serverList[i] = _serverList[i].Replace("\r", "");
                if(_serverList[i] == "")
                {
                    continue;
                }
                bool _exists = false;                          
                for(int x = 0; x < _host.Operations.Length; x++)
                {
                    if(_host.Operations[x] == null)
                    {
                        continue;
                    }
                    if(_serverList[i] == _host.Operations[x].OriginalHost)
                    {
                        _exists = true;
                        continue;
                    }          
                }
                if(!_exists)
                {
                    _tmpList.Add(new PingOperation("", _serverList[i], new TimeSpan(0, 0, 5)));
                }
            }
            _host.Operations = _tmpList.ToArray();
            for(int i = 0; i < _host.Operations.Length; i++)
            {
                if(_host.Operations[i] == null)
                {
                    continue;
                }
                bool _isRemoved = true;
                for(int x = 0; x < _serverList.Length; x++)
                {
                    if(_host.Operations[i].OriginalHost == _serverList[x])
                    {
                        _isRemoved = false;
                        continue;
                    }
                }
                if(_isRemoved)
                {
                    _host.RemoveOperationEntry(i);
                }
            }
            _host.Settings.WriteServerlist(serverList.Text);
        }
    }
}
