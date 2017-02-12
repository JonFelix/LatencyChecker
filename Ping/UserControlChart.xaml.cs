using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;

namespace Ping
{
    /// <summary>
    /// Interaction logic for UserControlChart.xaml
    /// </summary>
    public sealed partial class ConstantChangesChart : UserControl, INotifyPropertyChanged
    {
        private double _axisMax;
        private double _axisMin;

        public List<ChartValues<MeasureModel>> ChartValues
        {
            get;
        }
        public Func<double, string> DateTimeFormatter
        {
            get; set;
        }

        private double AxisStep
        {
            get; set;
        }

        public ConstantChangesChart()
        {
            InitializeComponent();

            //To handle live data easily, in this case we built a specialized type
            //the MeasureModel class, it only contains 2 properties
            //DateTime and Value
            //We need to configure LiveCharts to handle MeasureModel class
            //The next code configures MEasureModel  globally, this means
            //that livecharts learns to plot MeasureModel and will use this config every time
            //a ChartValues instance uses this type.
            //this code ideally should only run once, when application starts is reccomended.
            //you can configure series in many ways, learn more at http://lvcharts.net/App/examples/v1/wpf/Types%20and%20Configuration

            var mapper = Mappers.Xy<MeasureModel>()
                .X(model => model.DateTime.Ticks)   //use DateTime.Ticks as X
                .Y(model => model.Value);           //use the value property as Y

            //lets save the mapper globally.
            Charting.For<MeasureModel>(mapper);



            //the values property will store our values array
           ChartValues = new List<ChartValues<MeasureModel>>();

            //lets set how to display the X Labels
            DateTimeFormatter = value => new DateTime((long)value).ToString("mm:ss");

            AxisStep = TimeSpan.FromSeconds(1).Ticks;
            SetAxisLimits(DateTime.Now);

            //The next code simulates data changes every 300 ms
            Timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(300)
            };
            Timer.Tick += TimerOnTick;
            IsDataInjectionRunning = false;
            R = new Random();

            DataContext = this;

            Timer.Start();
            IsDataInjectionRunning = true;
        }



        public double AxisMax
        {
            get { return _axisMax; }
            set
            {
                _axisMax = value;
                OnPropertyChanged("AxisMax");
            }
        }
        public double AxisMin
        {
            get { return _axisMin; }
            set
            {
                _axisMin = value;
                OnPropertyChanged("AxisMin");
            }
        }

        public DispatcherTimer Timer { get; set; }
        public bool IsDataInjectionRunning { get; set; }
        public Random R { get; set; }

        public void AddSerie(int index, string title, object options = null)
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                ChartCartesianChart.Series.Add(new LineSeries()
                {
                    Values = new ChartValues<MeasureModel>(),
                    Title = title
                    //Fill = Brushes.Transparent
                });
            }));

        }

        private void RunDataOnClick(object sender, RoutedEventArgs e)
        {
            if (IsDataInjectionRunning)
            {
                Timer.Stop();
                IsDataInjectionRunning = false;
            }
            else
            {
                Timer.Start();
                IsDataInjectionRunning = true;
            }
        }

        private void TimerOnTick(object sender, EventArgs eventArgs)
        {
            var now = DateTime.Now;
                                                  
            SetAxisLimits(now);

            for (var i = 0; i < ChartValues?.Count; i++)
            {
                ChartCartesianChart.Series[i].Values.AddRange(ChartValues[i]);
                ChartValues[i].Clear();

                if (ChartCartesianChart.Series[i].Values.Count > 38)
                    ChartCartesianChart.Series[i].Values.RemoveAt(0);
            }       

        }

        private void SetAxisLimits(DateTime now)
        {
            AxisMax = now.Ticks + TimeSpan.FromSeconds(-5).Ticks; // lets force the axis to be 100ms ahead
            AxisMin = now.Ticks - TimeSpan.FromMinutes(3).Ticks; //we only care about the last 8 seconds
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void UpdateSerie(int index, MeasureModel measureModel)
        {
            if (ChartValues.Count==index)
                ChartValues.Add(new ChartValues<MeasureModel>());
            ChartValues[index].Add(measureModel ?? new MeasureModel());
        }
    }
}
