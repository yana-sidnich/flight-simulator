using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace FlightGearTestExec.ViewModels
{
    class TwoFeaturesGraphsViewModel : BaseViewModel
    {
        public class SeriesHolder
        {
            public ObservableCollection<ObservablePointF> points { get; set; }

            public ObservableCollection<ISeries> series { get; set; }

            public List<Axis> XAxes { get; set; }

            public List<Axis> YAxes { get; set; }

            public SeriesHolder()
            {
                SolidColorPaintTask separatorsBrush = new SolidColorPaintTask { Color = SKColors.FloralWhite, StrokeThickness = 0.3f };
                points = new ObservableCollection<ObservablePointF>();

                XAxes = new List<Axis>
                {
                    new Axis
                    {
                        MaxLimit = 1,
                        MinStep = 1,
                        SeparatorsBrush = separatorsBrush,  }
                };
                YAxes = new List<Axis>
                {
                    new Axis
                    {
                        ShowSeparatorLines = false,
                        ShowSeparatorWedges = false,
                        SeparatorsBrush = separatorsBrush,
                        MinStep = 1,
                    }
                };
            }

        }


        public ObservableCollection<SeriesHolder> SeriesData { get; set; } = new ObservableCollection<SeriesHolder>();

        private readonly IFlightSimulator _model;

        private const int STROKE_THICKNESS = 5;

        private Visibility _isGraphVisible = Visibility.Visible;
        public Visibility IsGraphVisible
        {
            get => _isGraphVisible;
            set
            {
                if (value == _isGraphVisible) return;
                _isGraphVisible = value;
                NotifyPropertyChanged("IsGraphVisible");
                NotifyPropertyChanged("IsErrorVisible");
            }
        }

        public Visibility IsErrorVisible => IsGraphVisible == Visibility.Hidden ? Visibility.Visible : Visibility.Hidden;


        const int GRAPHS_NUM = 2;

        public TwoFeaturesGraphsViewModel()
        {
            _model = simulator;
            _model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {
                    this.NotifyPropertyChanged("VM_TwoFeaturesGraphs_" + e.PropertyName);
                };
            SetGraphValues();

        }
        private void SetGraphValues()
        {
            for (int i = 0; i < GRAPHS_NUM; i++)
            {
                SeriesData.Add(new SeriesHolder());

                SeriesData[i].series = new ObservableCollection<ISeries>
                {
                    new LineSeries<ObservablePointF>
                    {
                        Values = SeriesData[i].points,
                        Fill = null,
                        GeometryFill = null,
                        GeometrySize = 0,
                        LineSmoothness = 0.1,
                        Stroke = new SolidColorPaintTask { Color = SKColors.FloralWhite, StrokeThickness = STROKE_THICKNESS },
                    }
                };
            }
        }

        public void UpdatePoints(string name)
        {
            if (name == null)
                return;

            int index;

            FlightDataContainer data;
            switch (name)
            {
                case "VM_TwoFeaturesGraphs_SelectedString":
                    index = 0;
                    if (string.IsNullOrEmpty(VM_TwoFeaturesGraphs_SelectedString))
                    {
                        SeriesData[index].points.Clear();
                        return;
                    }
                    data = getFeatureData(VM_TwoFeaturesGraphs_SelectedString);
                    break;
                case "VM_TwoFeaturesGraphs_CorrelatedString":
                    index = 1;
                    if (string.IsNullOrEmpty(VM_TwoFeaturesGraphs_CorrelatedString))
                    {
                        IsGraphVisible = Visibility.Hidden;
                        return;
                    }
                    IsGraphVisible = Visibility.Visible;
                    data = getFeatureData(VM_TwoFeaturesGraphs_CorrelatedString);
                    break;
                default:
                    return;
            }

            int OLD_SIZE = SeriesData[index].points.Count;
            int NEW_SIZE = data.values.Count;
            
            // if new points size is bigger than current - clean all current points
            if (OLD_SIZE > NEW_SIZE)
            {
                SeriesData[index].points.Clear();
                OLD_SIZE = 0;
            }

            float y;
            int x = 0;
            // first update old points values to new
            for (x = 0; x < OLD_SIZE; x++)
            {
                y = data.values[x];
                SeriesData[index].points[x].Y = y;
            }
            // add additional points
            for (; x < NEW_SIZE; x++)
            {
                y = data.values[x];
                SeriesData[index].points.Add(new ObservablePointF(x, y));
            }

            // set y axis range a bit bigger than min and max values
            SeriesData[index].YAxes[0].MinLimit = data.minValue * 1.1;
            SeriesData[index].YAxes[0].MaxLimit = data.maxValue * 1.1;
        }

        public FlightDataContainer getFeatureData(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;
            if (_model.DataDictionary.ContainsKey(name))
            {
                return _model.DataDictionary[name];
            }

            return null;
        }

        public string VM_TwoFeaturesGraphs_SelectedString => _model.SelectedString;

        public string VM_TwoFeaturesGraphs_CorrelatedString => _model.CorrelatedString;

        public int VM_TwoFeaturesGraphs_CurrentLineNumber => _model.GetCurrentLine();
    }
}