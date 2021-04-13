using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;

namespace FlightGearTestExec.ViewModels
{
    class GraphsViewModel : BaseViewModel
    {
        private readonly IFlightSimulator _model;
        public GraphsViewModel()
        {
            _model = simulator;
            _model.PropertyChanged +=
                delegate(Object sender, PropertyChangedEventArgs e)
                {
                    this.NotifyPropertyChanged("VM_Graphs_" + e.PropertyName);
                };
        }

        public string VM_Graphs__SelectedString
        {
            get;
            set;
        }
    }
}