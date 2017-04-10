using System;
using System.ComponentModel;       

namespace Ping
{
    public class Engine
    {
        private readonly MainWindow _host;
        private readonly BackgroundWorker _worker = new BackgroundWorker();
        private bool _isRunning = true;
        private int _chartIndex = -1;

        public bool IsRunning
        {
            get
            {
                return _isRunning;
            }
            set
            {
                _isRunning = value;
            }
        }


        public Engine(MainWindow host)
        {
            _host = host;
        }

        public void Run()
        {
            _worker.DoWork += RunThreaded;
            _worker.RunWorkerAsync();    
        }

        private void RunThreaded(object sender, DoWorkEventArgs e)
        {
            var ping = new PingManager(this);

            var startTime = DateTime.Now;
            var lastTime = startTime;
            var interval = new TimeSpan(0, 0, 1);
            while(_isRunning)
            {
                if((DateTime.Now - lastTime) < interval)
                {
                    continue;
                }
                lastTime = DateTime.Now;
                string _trayText = "";
                for(var i = 0; i < _host.Operations.Length; i++)
                {
                    if(_host.Operations[i] == null)
                    {
                        continue;
                    }
                    if(_host.Operations[i].LastOperationTime + _host.Operations[i].Interval < DateTime.Now)
                    {   
                        _host.Operations[i].Cursor++;
                        if(_host.Operations[i].Ip == "")
                        {
                            _host.RemoveOperationEntry(i);
                            continue;
                        }
                        if(_host.Operations[i].HostName == "")
                        {
                            try
                            { 

                                _host.Operations[i].HostName = System.Net.Dns.GetHostEntry(_host.Operations[i].Ip).HostName;
                                _host.Operations[i].Ip = System.Net.Dns.GetHostEntry(_host.Operations[i].Ip).AddressList[0].ToString();
                            }
                            catch
                            {
                                Log("Host '" + _host.Operations[i].Ip + "' was not found. removing entry!");
                                _host.RemoveOperationEntry(i);
                                continue;
                            }  
                        }
                        if(_host.Operations[i].Cursor >= _host.Operations[i].ResponseTime.Length)
                        {
                            _host.Operations[i].Cursor--;
                            _host.Operations[i].PushResponses();
                        }
                        _host.Operations[i].LastOperationTime = DateTime.Now;
                        var reply = ping.SendPing(_host.Operations[i].Ip);
                        if(reply.Status != System.Net.NetworkInformation.IPStatus.Success)
                        {
                            _host.Log(_host.Operations[i].HostName + " error: " + reply.Status.ToString());
                        }
                        _host.Operations[i].ResponseMessage[_host.Operations[i].Cursor] = reply.Status.ToString();
                        _host.Operations[i].ResponseTime[_host.Operations[i].Cursor] = reply.RoundtripTime;
                        _host.Operations[i].ResponseTimestamp[_host.Operations[i].Cursor] = DateTime.Now;
                        if(_host.Operations[i].ResponseTime[_host.Operations[i].Cursor] >= _host.PingLimit)
                        {
                            _host.Log(_host.Operations[i].HostName + " has high ping! (" + reply.RoundtripTime + "ms)");           
                        }
                        if(_host.Operations[i].ResponseTime[_host.Operations[i].Cursor] > _host.Operations[i].MaxPing)
                        {
                            _host.Operations[i].MaxPing = _host.Operations[i].ResponseTime[_host.Operations[i].Cursor];
                        }
                        if(Chart(Convert.ToDouble(_host.Operations[i].ResponseTime[_host.Operations[i].Cursor]), _host.Operations[i].ChartIndex, _host.Operations[i].HostName, lastTime))
                        {
                            _host.Operations[i].ChartIndex = _chartIndex;
                        }
                        
                    }
                    _trayText += _host.Operations[i].HostName + " (Cur:" + _host.Operations[i].ResponseTime[_host.Operations[i].Cursor].ToString() + "ms Max:" + _host.Operations[i].MaxPing + "ms)";
                    if(i < _host.Operations.Length)
                    {
                        _trayText += Environment.NewLine;
                    }
                }
                _host.TrayIcon.Text = _trayText.Length > 64 ? _trayText.Substring(0, 60) + "..." : _trayText;
                _host.UpdateInfo(_trayText);
            }    
        }

        public void Log(string text)
        {
            _host.Log(text);
        }

        public bool Chart(double value, int index, string title, DateTime? date=null)
        {
            bool _return = false;
            if (_host.Chart.ChartValues==null||index >= _host.Chart.ChartValues.Count || index == -1)
            {
                _chartIndex++;
                index = _chartIndex;
                _host.Chart.AddSerie(_chartIndex, title);
                _return = true;
            }
            _host.Chart.UpdateSerie(index, new MeasureModel() { Value = value == 0 ? double.NaN:value, Series = index, DateTime = date ?? DateTime.Now});
            return _return;
        }
                              
    }    
}
