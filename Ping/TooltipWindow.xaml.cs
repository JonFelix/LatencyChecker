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
using System.Windows.Shapes;

namespace Ping
{ 
    public partial class TooltipWindow : Window
    {
        MainWindow _host;

        int _windowBuffer = 20;
        int _rectBuffer = 5;
        int _rectHeight = 30;
        Color _green = Color.FromRgb(158, 214, 97);
        Color _yellow = Color.FromRgb(255, 255, 117);
        Color _red = Color.FromRgb(237, 90, 94);
        Color _blue = Color.FromRgb(131, 131, 241);

        public TooltipWindow(MainWindow host)
        {
            _host = host;
            System.Drawing.Rectangle resolution = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;   
            InitializeComponent();    
            this.Height = (_host.Operations.Length * (_rectHeight + _rectBuffer)) + (_rectBuffer * 2);
            this.Left = resolution.Width - this.Width - _windowBuffer;
            this.Top = resolution.Height - this.Height - _windowBuffer;
            int ignored = 0;
            for(int i = 0; i < _host.Operations.Length; i++)
            {
                if(_host.Operations[i] == null)
                {
                    ignored++;        
                    continue;
                }
                if(_host.Operations[i].Cursor == -1)
                {
                    ignored++;
                    continue;
                }
                Rectangle rect = new Rectangle();
                
                rect.Height = _rectHeight;
                float pingValue = _host.Operations[i].ResponseTime[_host.Operations[i].Cursor];
                float percentage = Math.Min(pingValue / _host.PingLimit, 1f);
                int maxWidth = (int)(this.Width - (2 * _rectBuffer));
                int width = Math.Max((int)(percentage * maxWidth), 0);
                rect.Width = width;
                if(percentage < 0.5f)
                {
                    rect.Fill = new SolidColorBrush(_green);
                }
                else if(percentage >= 1)
                {
                    rect.Fill = new SolidColorBrush(_red);
                }
                else if(percentage == 0)
                {
                    rect.Fill = new SolidColorBrush(_blue);
                }
                else
                {
                    rect.Fill = new SolidColorBrush(_yellow);
                }
                Canvas.SetLeft(rect, _rectBuffer);
                Canvas.SetTop(rect, _rectBuffer + ((i - ignored) * (_rectHeight + _rectBuffer)));
                Canvas.Children.Add(rect);
                TextBlock textbox = new TextBlock();
                textbox.Foreground = Brushes.BlueViolet;
                textbox.FontSize = 18;
                textbox.Text = _host.Operations[i].HostName + " " + pingValue + "ms";
                Canvas.Children.Add(textbox);
                Canvas.SetLeft(textbox, _rectBuffer);
                Canvas.SetTop(textbox, _rectBuffer + ((i - ignored) * (_rectHeight + _rectBuffer)));
            }
        } 

        private void Window_Deactivated(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
