using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.NetworkInformation;    
using LiveCharts;
using LiveCharts.Wpf;

namespace Ping
{
    public class Engine
    {
        MainWindow _host;      
        BackgroundWorker _worker = new BackgroundWorker();
        bool _isRunning = true;

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

        void RunThreaded(object sender, DoWorkEventArgs e)
        {
            PingManager _ping = new PingManager(this);

            DateTime _startTime = DateTime.Now;
            DateTime _lastTime = _startTime;
            TimeSpan _interval = new TimeSpan(0, 0, 1);
            while(_isRunning)
            {
                if((DateTime.Now - _lastTime) < _interval)
                {
                    continue;
                }
                _lastTime = DateTime.Now;

                for(int i = 0; i < _host.Operations.Length; i++)
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
                        PingReply _reply = _ping.SendPing(_host.Operations[i].IP);
                        _host.Operations[i].ResponseMessage[_host.Operations[i].Cursor] = _reply.Status.ToString();
                        _host.Operations[i].ResponseTime[_host.Operations[i].Cursor] = _reply.RoundtripTime;
                        _host.Operations[i].ResponseTimestamp[_host.Operations[i].Cursor] = DateTime.Now;
                        Chart(value: Convert.ToDouble(_host.Operations[i].ResponseTime[_host.Operations[i].Cursor]), index: i, date: _lastTime);
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
