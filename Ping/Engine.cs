using System;
using System.ComponentModel;

namespace Ping
{
    public class Engine
    {
        private readonly MainWindow _host;
        private readonly BackgroundWorker _worker = new BackgroundWorker();
        private bool _isRunning = true;

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

                for(var i = 0; i < _host.Operations.Length; i++)
                {
                    if(_host.Operations[i].LastOperationTime + _host.Operations[i].Interval < DateTime.Now)
                    {
                        _host.Operations[i].Cursor++;
                        if(_host.Operations[i].Cursor >= _host.Operations[i].ResponseTime.Length)
                        {
                            _host.Operations[i].Cursor--;
                            _host.Operations[i].PushResponses();
                        }
                        _host.Operations[i].LastOperationTime = DateTime.Now;
                        var reply = ping.SendPing(_host.Operations[i].Ip);
                        _host.Operations[i].ResponseMessage[_host.Operations[i].Cursor] = reply.Status.ToString();
                        _host.Operations[i].ResponseTime[_host.Operations[i].Cursor] = reply.RoundtripTime;
                        _host.Operations[i].ResponseTimestamp[_host.Operations[i].Cursor] = DateTime.Now;
                        Chart(Convert.ToDouble(_host.Operations[i].ResponseTime[_host.Operations[i].Cursor]), i, lastTime);
                    }
                }
            }    
        }

        public void Log(string text)
        {
            _host.Log(text);
        }

        public void Chart(double value, int index, DateTime? date=null)
        {
            if (_host.Chart.ChartValues==null||index >= _host.Chart.ChartValues.Count)
            {
                //_host.Chart.ChartValues.Add(null);
                _host.Chart.AddSerie(index);
            }
            _host.Chart.UpdateSerie(index, new MeasureModel() { Value = value == 0 ? double.NaN:value, Series = index, DateTime = date ?? DateTime.Now});
        }
                              
    }    
}
