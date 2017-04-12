using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        int _tickRate = 1;
        int _width;
        int _height;
        Color _green = Color.FromRgb(158, 214, 97);
        Color _yellow = Color.FromRgb(255, 255, 117);
        Color _red = Color.FromRgb(237, 90, 94);
        Color _blue = Color.FromRgb(131, 131, 241);

        Thread _uiUpdate;

        public delegate void AddRectCallback(int x, int y, int w, int h, Color c);
        public delegate void AddTextBlockCallback(string text, int x, int y, int fontsize, SolidColorBrush c);
        public delegate void ClearCanvasCallback();

        public TooltipWindow(MainWindow host)
        {
            _host = host;
            InitializeComponent();
            _height = (int)this.Height;
            _width = (int)this.Width;
            System.Drawing.Rectangle resolution = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
            int length = 0;
            for(int i = 0; i < _host.Operations.Length; i++)
            {
                if(_host.Operations[i] != null)
                {
                    if(_host.Operations[i].Cursor != -1)
                    {
                        length++;
                    }
                }
            }
            this.Height = (length * (_rectHeight + _rectBuffer)) + (_rectBuffer * 2);
            this.Left = resolution.Width - this.Width - _windowBuffer;
            this.Top = resolution.Height - this.Height - _windowBuffer;
            _uiUpdate = new Thread(new ThreadStart(UpdateUI));
            _uiUpdate.Start();

        }

        private void AddRect(int x, int y, int w, int h, Color c)
        {
            Rectangle rect = new Rectangle();
            rect.Width = w;
            rect.Height = h;
            rect.Fill = new SolidColorBrush(c);
            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
            Canvas.Children.Add(rect);
        }

        private void AddTextBlock(string text, int x, int y, int fontsize, SolidColorBrush c)
        {
            TextBlock textblock = new TextBlock();
            textblock.Text = text;
            textblock.FontSize = fontsize;
            textblock.Foreground = c;
            Canvas.SetLeft(textblock, x);
            Canvas.SetTop(textblock, y);
            Canvas.Children.Add(textblock);
        }

        private void ClearCanvas()
        {
            Canvas.Children.Clear();
        }


        private void Window_Deactivated(object sender, EventArgs e)
        {
            _uiUpdate.Abort();
            this.Close();
        }

        private void UpdateUI()
        {
            DateTime curTime = DateTime.Now;
            TimeSpan freq = new TimeSpan(0, 0, 0, _tickRate, 0);
            DateTime nextUpdate = DateTime.Now;
            while(true)
            {
                curTime = DateTime.Now;
                if(curTime > nextUpdate)
                {
                    // LOGIC     
                    Canvas.Dispatcher.Invoke(new ClearCanvasCallback(this.ClearCanvas));
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
                                                   
                        float pingValue = _host.Operations[i].ResponseTime[_host.Operations[i].Cursor];
                        float percentage = Math.Min(pingValue / _host.PingLimit, 1f);
                        int maxWidth = (int)(_width - (2 * _rectBuffer));
                        int width = Math.Max((int)(percentage * maxWidth), 0);
                        Color col;            
                        if(percentage < 0.5f)
                        {
                            col = _green;
                        }
                        else if(percentage >= 1)
                        {
                            col = _red;
                        }
                        else if(percentage == 0)
                        {
                            col = _blue;
                        }
                        else
                        {
                            col = _yellow;
                        }

                        //TEXT

                        string tmpName = _host.Operations[i].HostName == "" ? _host.Operations[i].OriginalHost : _host.Operations[i].HostName;
                        string name = tmpName.Length > 25 ? tmpName.Substring(0, 25) + "..." : tmpName;
                        name += " " + pingValue + "ms";


                        

                        Canvas.Dispatcher.Invoke(
                            new AddRectCallback(this.AddRect),
                            new object[] { _rectBuffer, _rectBuffer + ((i - ignored) * (_rectHeight + _rectBuffer)), maxWidth, _rectHeight, Color.FromRgb(100, 100, 100) }
                            );

                        Canvas.Dispatcher.Invoke(
                            new AddRectCallback(this.AddRect),
                            new object[] { _rectBuffer, _rectBuffer + ((i - ignored) * (_rectHeight + _rectBuffer)), width, _rectHeight, col }
                            );

                        Canvas.Dispatcher.Invoke(
                            new AddTextBlockCallback(this.AddTextBlock),
                            new object[] { name, _rectBuffer, _rectBuffer + ((i - ignored) * (_rectHeight + _rectBuffer)), 18, Brushes.BlueViolet }
                            );

                        
                        //AddRect(rect, _rectBuffer, _rectBuffer + ((i - ignored) * (_rectHeight + _rectBuffer)));
                        //AddTextBlock(textbox, _rectBuffer, _rectBuffer + ((i - ignored) * (_rectHeight + _rectBuffer)));
                        //AddRect(borderRect, _rectBuffer, _rectBuffer + ((i - ignored) * (_rectHeight + _rectBuffer)));
                    }




                    //ENDLOGIC    
                    nextUpdate = DateTime.Now;
                    nextUpdate = nextUpdate.Add(freq);
                }
            } 
        }
    }
}
