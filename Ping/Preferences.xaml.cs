using System.Windows.Controls;

namespace Ping
{
    /// <summary>
    /// Interaction logic for Preferences.xaml
    /// </summary>
    public partial class Preferences
    {
        public Preferences(MainWindow host)
        {
            InitializeComponent();
            for(var i = 0; i < host.Operations.Length; i++)
            {
                var lvi = new ListViewItem();
                ListView.Items.Add(lvi);
                   
            }
        }
    }
}
