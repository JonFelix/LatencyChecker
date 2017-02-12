using System.Collections.Generic;
using System.IO;

namespace Ping
{
    public class SettingsManager
    {
        readonly string _serverListFile = @"serverlist";
        string _serverListContent = "";
        MainWindow _host;

        public string ServerListContent
        {
            get
            {
                return _serverListContent;
            }
            set
            {
                _serverListContent = value;
            }
        }


        public SettingsManager(MainWindow host)
        {
            _host = host;

            if(!File.Exists(_serverListFile))
            {
                File.Create(_serverListFile);
            }
            StreamReader _sreader = new StreamReader(_serverListFile);
            string line = "";
            List<PingOperation> _tmpList = new List<PingOperation>();
            while((line = _sreader.ReadLine()) != null)
            {
                _tmpList.Add(new PingOperation("", line, new System.TimeSpan(0, 0, 5)));
                _serverListContent += line + System.Environment.NewLine;
            }
            if(_tmpList.Count > 0)
            {
                _host.Operations = _tmpList.ToArray();
            }
            else
            {
                _host.Log("Serverlist is empty!");
            }
        }
    }
}
