using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;     

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
            _operations.Add(new PingOperation("8.8.8.8", new TimeSpan(0, 0, 1)));
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
