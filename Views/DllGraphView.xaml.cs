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
using static FlightGearTestExec.Models.FlightDataContainer;


namespace FlightGearTestExec.Views
{
    public partial class DllGraphView : UserControl
    {
        private DllGraphViewModel vm;

        public DllGraphView()
        {
            InitializeComponent();
            vm = this.DataContext as DllGraphViewModel;

            vm.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {
                    string name = e.PropertyName;
                    if (name == "VM_DllGraph_SelectedString" ||
                        name == "VM_DllGraph_CorrelatedString")
                    {
                        vm.UpdatePoints(name);
                    }

                    else if (name == "VM_DllGraph_CurrentLineNumber")
                    {
                        int lineNumber = vm.VM_DllGraph_CurrentLineNumber;
                        vm.UpdateFrame(lineNumber);
                    }
                    else if (name == "VM_DllGraph_SelectedDll")
                    {
                        dllGraphDock.Children?.Clear();
                        vm.dllGraphOutput.Height = 500;
                        vm.dllGraphOutput.Width = 500;
                        dllGraphDock.Children.Add(vm.dllGraphOutput);
                    }
                };

            // set default value
            DllComboBox.SelectedIndex = 0;
        }

        public void doneDraggingThreshold(object sender, RoutedEventArgs e)
        {
            vm.ThresholdDragDone();
        }
    }
}