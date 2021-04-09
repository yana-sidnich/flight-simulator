using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using FlightGearTestExec.ViewModels;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.WPF;
using static FlightGearTestExec.FlightDataContainer;


namespace FlightGearTestExec.Views
{
    public partial class TwoFeaturesGraphView : UserControl
    {
        public SolidColorPaintTask strokeColor;
        private ObservableCollection<ObservablePointF> points;
        public List<Axis> axesList;

        private Dictionary<string, FlightDataContainer> dictionarrrrrrry;

        // private double _xMin;
        // private double _xMax;
        //
        // public double XMin
        // {
        //     get => this._xMin;
        //     set
        //     {
        //         this._xMin = value;
        //         if (PropertyChanged != null)
        //             PropertyChanged(this, new PropertyChangedEventArgs("XMin"));
        //     }
        // }
        //
        // public double XMax
        // {
        //     get => this._xMax;
        //     set
        //     {
        //         this._xMax = value;
        //         if (PropertyChanged != null)
        //             PropertyChanged(this, new PropertyChangedEventArgs("XMax"));
        //     }
        // }
        //

        /// 

        public List<Axis> XAxes { get; set; }

        public List<Axis> YAxes { get; set; }

        /// 
        private TwoFeaturesGraphsViewModel vm;

        public TwoFeaturesGraphView()
        {
            InitializeComponent();
            vm = this.DataContext as TwoFeaturesGraphsViewModel;
            vm.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {
                    string name = e.PropertyName;
                    if (name == "VM_TwoFeaturesGraphs_SelectedString" || name == "VM_TwoFeaturesGraphs_CorrelatedString")
                    {
                        updatePoints(name);
                    }
                };
        }

        private void updatePoints(string name)
        {
            List<ObservablePointF> points = new List<ObservablePointF>();
            double[] values;
            int SIZE;
            int graphIndex = 0;
            if (name == "VM_TwoFeaturesGraphs_SelectedString")
            {
                graphIndex = 0;
                values = vm.getFeatureValues(vm.VM_TwoFeaturesGraphs_SelectedString);
            }
            else if (name == "VM_TwoFeaturesGraphs_CorrelatedString")
            {
                graphIndex = 1;
                values = vm.getFeatureValues(vm.VM_TwoFeaturesGraphs_CorrelatedString);
            }
            else
            {
                return;
            }

            if (values == null)
            {
                return;
            }

            SIZE = values.Length;
            for (int i = 0; i < SIZE; i++)
            {
                points.Add(new ObservablePointF(i, (float)values[i]));
            }

            vm.resetPoints(graphIndex, points);

        }
    }
    // private void CreateAxes()
    // {
    //     axesList = new List<Axis> {
    //         new Axis
    //         {
    //             MinLimit = -50, //XMin,
    //             MaxLimit = 700,//XMax,
    //         },
    //         new Axis
    //         {
    //             MinLimit = -50, //YMin,
    //             MaxLimit = 700,//YMax,
    //         }
    //     };
}