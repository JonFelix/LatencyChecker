using System;
using System.Collections.Generic;   
using System.Windows;  

namespace Ping
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<PingOperation> _operations = new List<PingOperation>();
        Engine _engine;

        public static readonly DependencyProperty PingListProperty = DependencyProperty.Register(
            "PingList", typeof(string), typeof(MainWindow), new PropertyMetadata(default(string)));

        public string PingList
        {
            get { return Dispatcher.Invoke(() => (string)GetValue(PingListProperty)) ; }
            set { Dispatcher.BeginInvoke((Action)(()=>SetValue(PingListProperty, value))) ; }
        }

        public PingOperation[] Operations
        {
            get
            {
                return _operations.ToArray();
            }
            set
            {                       
                _operations.AddRange(value);
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            //Google
            _operations.Add(new PingOperation("8.8.8.8", new TimeSpan(0, 0, 5)));
            _operations.Add(new PingOperation("8.8.4.4", new TimeSpan(0, 0, 5)));

            //OpenDns
            _operations.Add(new PingOperation("208.67.222.222", new TimeSpan(0, 0, 5))); 
            _operations.Add(new PingOperation("208.67.220.220", new TimeSpan(0, 0, 5)));

            //OpenNIC 
            _operations.Add(new PingOperation("5.9.49.12", new TimeSpan(0, 0, 5)));//(DE)
            _operations.Add(new PingOperation("87.98.175.58", new TimeSpan(0, 0, 5)));//(FR)
            _operations.Add(new PingOperation("193.183.98.154", new TimeSpan(0, 0, 5)));//(IT)
            _operations.Add(new PingOperation("5.135.183.146", new TimeSpan(0, 0, 5)));//(FR)

            //fooldns
            _operations.Add(new PingOperation("87.118.111.215", new TimeSpan(0, 0, 5)));
            _operations.Add(new PingOperation("213.187.11.62", new TimeSpan(0, 0, 5)));

            //Yandex.DNS
            _operations.Add(new PingOperation("77.88.8.8", new TimeSpan(0, 0, 5)));
            _operations.Add(new PingOperation("77.88.8.1", new TimeSpan(0, 0, 5)));
            _operations.Add(new PingOperation("77.88.8.88", new TimeSpan(0, 0, 5)));
            _operations.Add(new PingOperation("77.88.8.2", new TimeSpan(0, 0, 5)));
            _operations.Add(new PingOperation("77.88.8.7", new TimeSpan(0, 0, 5)));
            _operations.Add(new PingOperation("77.88.8.3", new TimeSpan(0, 0, 5)));

            //Comodo Secure DNS
            _operations.Add(new PingOperation("8.26.56.26", new TimeSpan(0, 0, 5)));
            _operations.Add(new PingOperation("8.20.247.20", new TimeSpan(0, 0, 5)));

            //safedns
            _operations.Add(new PingOperation("195.46.39.39", new TimeSpan(0, 0, 5)));
            _operations.Add(new PingOperation("195.46.39.40", new TimeSpan(0, 0, 5)));

            //need more? http://public-dns.info/nameserver/se.html
            
            _engine = new Engine(this);
            _engine.Run();

            
        }
        
        public void Log(string text)
        {
            PingList += PingList!=null? Environment.NewLine + text :text;
        }

        private void ClickMenuPreferences(object sender, RoutedEventArgs e)
        {
            Ping.Preferences _prefWindow = new Preferences(this);
            _prefWindow.Show();
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _engine.IsRunning = false;   
        }

    }
}
