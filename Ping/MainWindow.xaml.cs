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
        private readonly SettingsManager _settings;

        bool _userExit = false;

        public static readonly DependencyProperty PingListProperty = DependencyProperty.Register(
            "PingList", typeof(string), typeof(MainWindow), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty InfoWindowProperty = DependencyProperty.Register(
            "InfoWindow", typeof(string), typeof(MainWindow), new PropertyMetadata(default(string)));

        public string PingList
        {   
            get { 
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

        public SettingsManager Settings
        {
            get
            {
                return _settings;
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
            _settings = new SettingsManager(this);    

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

        public void RemoveOperationEntry(int index)
        {
            _operations.RemoveAt(index);
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

        private void ClickMenuExit(object sender, RoutedEventArgs e)
        {
            _userExit = true;
            _engine.IsRunning = false;
            this.Close();
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(_userExit)
            {
                return;
            }
            e.Cancel = true;
            Hide();  
        }

    }
}
