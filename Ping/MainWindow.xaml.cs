using System;
using System.Collections.Generic;   
using System.Windows;


namespace Ping
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly List<PingOperation> _operations = new List<PingOperation>();
        private readonly Engine _engine;
        private readonly TrayIcon _icon;

        public static readonly DependencyProperty PingListProperty = DependencyProperty.Register(
            "PingList", typeof(string), typeof(MainWindow), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty InfoWindowProperty = DependencyProperty.Register(
            "InfoWindow", typeof(string), typeof(MainWindow), new PropertyMetadata(default(string)));

        public string PingList
        {   
            get {
                if(!_engine.IsRunning)
                {
                    return "";
                }
                return Dispatcher.Invoke(() => (string)GetValue(PingListProperty)) ; }
            set { Dispatcher.BeginInvoke((Action)(()=>SetValue(PingListProperty, value))) ; }
        }

        public string InfoWindow
        {
            get
            {
                if(!_engine.IsRunning)
                {
                    return "";
                }
                return Dispatcher.Invoke(() => (string)GetValue(InfoWindowProperty));
            }
            set
            {
                Dispatcher.BeginInvoke((Action)(() => SetValue(InfoWindowProperty, value)));
            }
        }

        public TrayIcon TrayIcon
        {
            get
            {
                return _icon;
            }
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
            _icon = new TrayIcon(this);
            

            //Google
            _operations.Add(new PingOperation("Primary Google DNS", "8.8.8.8", new TimeSpan(0, 0, 5)));
            _operations.Add(new PingOperation("Secondary Google DNS", "8.8.4.4", new TimeSpan(0, 0, 5)));

            //OpenDns
            /*_operations.Add(new PingOperation("Primary OpenDNS DNS", "208.67.222.222", new TimeSpan(0, 0, 5))); 
            _operations.Add(new PingOperation("Secondary OpenDNS DNS", "208.67.220.220", new TimeSpan(0, 0, 5)));

            //OpenNIC 
            _operations.Add(new PingOperation("OpenNIC DE DNS", "5.9.49.12", new TimeSpan(0, 0, 5)));//(DE)
            _operations.Add(new PingOperation("OpenNIC FR DNS", "87.98.175.58", new TimeSpan(0, 0, 5)));//(FR)
            _operations.Add(new PingOperation("OpenNIC IT DNS", "193.183.98.154", new TimeSpan(0, 0, 5)));//(IT)
            _operations.Add(new PingOperation("Secondary OpenNIC FR DNS", "5.135.183.146", new TimeSpan(0, 0, 5)));//(FR)

            //fooldns                                                                                         
            _operations.Add(new PingOperation("FoolDNS", "213.187.11.62", new TimeSpan(0, 0, 5)));

            //Yandex.DNS
            _operations.Add(new PingOperation("Primary Yandex.DNS", "77.88.8.8", new TimeSpan(0, 0, 5)));
            _operations.Add(new PingOperation("Secondary Yandex.DNS", "77.88.8.1", new TimeSpan(0, 0, 5)));     

            //Comodo Secure DNS
            _operations.Add(new PingOperation("Primary Comodo Secure DNS", "8.26.56.26", new TimeSpan(0, 0, 5)));
            _operations.Add(new PingOperation("Secondary Comodo Secure DNS", "8.20.247.20", new TimeSpan(0, 0, 5))); */   

            //need more? http://public-dns.info/nameserver/se.html
            
            _engine = new Engine(this);
            _engine.Run();

            
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if(WindowState == WindowState.Minimized)
                this.Hide();

            base.OnStateChanged(e);
        }

        public void Log(string text)
        {
            PingList += PingList!=null? Environment.NewLine + text :text;
        }

        public void UpdateInfo(string text)
        {
            InfoWindow = text;
        }

        private void ClickMenuPreferences(object sender, RoutedEventArgs e)
        {
            var prefWindow = new Preferences(this);
            prefWindow.Show();
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _engine.IsRunning = false;   
        }

    }
}
