using System.Collections.Generic;
using System.IO;

namespace Ping
{
    public class SettingsManager
    {
        readonly string _serverListFile = @"/serverlist";
        MainWindow _host;


        public SettingsManager(MainWindow host)
        {
            host = _host;

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
            }
            _host.Operations = _tmpList.ToArray();
        }
    }
}
