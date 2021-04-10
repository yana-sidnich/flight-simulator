using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
    public partial class TwoFeaturesGraphsView : UserControl
    {
        public SolidColorPaintTask strokeColor;

        private ObservableCollection<ObservablePointF> points;

        public List<Axis> axesList;

        public List<Axis> XAxes { get; set; }

        public List<Axis> YAxes { get; set; }

        /// 
        private TwoFeaturesGraphsViewModel vm;

        private CartesianChart char_0;

        private CartesianChart char_1;

        private Axis x_axis_0;

        private Axis x_axis_1;

        private readonly TimeSpan fastAnimationSpeed;

        private TimeSpan originalAnimation_0;

        private TimeSpan originalAnimation_1;

        public TwoFeaturesGraphsView()
        {
            InitializeComponent();
            vm = this.DataContext as TwoFeaturesGraphsViewModel;
            char_0 = graph_0 as CartesianChart;
            char_1 = graph_1 as CartesianChart;
            x_axis_0 = (char_0?.XAxes as List<Axis>)?[0];
            x_axis_1 = (char_1?.XAxes as List<Axis>)?[0];
            vm.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {
                    string name = e.PropertyName;
                    if (name == "VM_TwoFeaturesGraphs_SelectedString" ||
                        name == "VM_TwoFeaturesGraphs_CorrelatedString")
                    {
                        // temporarily faster the animation speed between points changes
                        char_0.AnimationsSpeed = fastAnimationSpeed;
                        char_1.AnimationsSpeed = fastAnimationSpeed;
                        vm.UpdatePoints(name);
                        
                        // wait before restoring default speed
                        Task.Run(() =>
                        {
                            Thread.Sleep(1000);
                            char_0.AnimationsSpeed = originalAnimation_0;
                            char_1.AnimationsSpeed = originalAnimation_1;
                        });
                    }

                    else if (name == "VM_TwoFeaturesGraphs_CurrentLineNumber")
                    {
                        int lineNumber = vm.VM_TwoFeaturesGraphs_CurrentLineNumber;
                        if (x_axis_0 != null)
                            x_axis_0.MaxLimit = lineNumber;
                        if (x_axis_1 != null)
                            x_axis_1.MaxLimit = lineNumber;

                    }
                };

            // FOR TESTING
            // Task.Run(() =>
            // {
            //     int lineNumber = 0;
            //     while (true)
            //     {
            //         Thread.Sleep(50);
            //         // if (Application.Current != null)
            //         // {
            //         //     Application.Current.Dispatcher.Invoke(() =>
            //         //     {
            //         //         BaseViewModel.modelllllllll.CurrentLineNumber++; 
            //         //     });
            //         // }   
            //         if (Application.Current != null)
            //         {
            //             Application.Current.Dispatcher.Invoke(() =>
            //             {
            //                 if (x_axis_0 != null)
            //                     x_axis_0.MaxLimit = lineNumber;
            //                 if (x_axis_1 != null)
            //                     x_axis_1.MaxLimit = lineNumber;
            //                 lineNumber++;
            //             });
            //         }

            //     }
            // });
        }
    }
}