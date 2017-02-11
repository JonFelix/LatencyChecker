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
    /// <summary>
    /// Interaction logic for Preferences.xaml
    /// </summary>
    public partial class Preferences : Window
    {
        MainWindow _host;

        public Preferences(MainWindow host)
        {
            _host = host;
            InitializeComponent();
            for(int i = 0; i < _host.Operations.Length; i++)
            {
                ListViewItem _lvi = new ListViewItem();
                listView.Items.Add(_lvi);
                   
            }
        }
    }
}
