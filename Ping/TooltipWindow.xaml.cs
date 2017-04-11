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

        int _buffer = 20;

        public TooltipWindow(MainWindow host)
        {
            _host = host;
            System.Drawing.Rectangle resolution = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
            
            InitializeComponent();
            this.Left = resolution.Width - this.Width - _buffer;
            this.Top = resolution.Height - this.Height - _buffer;
        } 

        private void Window_Deactivated(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
