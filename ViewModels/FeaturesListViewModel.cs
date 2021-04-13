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
    class FeaturesListViewModel : BaseViewModel
    {
        private readonly IFlightSimulator _model;
        public FeaturesListViewModel()
        {
            _model = simulator;
            _model.PropertyChanged +=
                delegate(Object sender, PropertyChangedEventArgs e)
                {
                    this.NotifyPropertyChanged("VM_FeaturesList_" + e.PropertyName);
                };
        }

        public Dictionary<string, FlightDataContainer> getDictionary()
        {
            return _model?.DataDictionary;
        }

        public Dictionary<string, string> VM_FeatureList_CorrelatedFeatures => _model.CorrelatedFeatures;

        public string VM_FeaturesList_SelectedString
        {
            get { return _model.SelectedString; }
            set { _model.SelectedString = value;}
        }

        public string VM_FeaturesList_CorrelatedString
        {
            get { return _model.CorrelatedString; }
            set { _model.CorrelatedString = value;}
        }
    }
}