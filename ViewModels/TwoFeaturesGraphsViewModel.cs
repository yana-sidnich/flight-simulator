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
    public class TwoFeaturesGraphsViewModel : BaseViewModel
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


        public ObservableCollection<SeriesHolder> SeriesData
        {
            get { return _seriesData; }
            set { _seriesData = value; }
        }

        private readonly FlightSimulator _model;

        private const int STROKE_THICKNESS = 5;

        private Visibility _isGraphVisible = Visibility.Visible;
        public Visibility IsGraphVisible
        {
            get
            {
                return _isGraphVisible;
            }
            set
            {
                if (value != _isGraphVisible)
                {
                    _isGraphVisible = value;
                    NotifyPropertyChanged("IsGraphVisible");
                    NotifyPropertyChanged("IsErrorVisible");
                }
            }
        }

        public Visibility IsErrorVisible
        {
            get
            {
                if (IsGraphVisible == Visibility.Hidden)
                    return Visibility.Visible;
                else
                    return Visibility.Hidden;
            }
        }


        private ObservableCollection<SeriesHolder> _seriesData = new ObservableCollection<SeriesHolder>();

        const int GRAPHS_NUM = 2;

        public TwoFeaturesGraphsViewModel()
        {
            _model = model as FlightSimulator;
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
                _seriesData.Add(new SeriesHolder());

                _seriesData[i].series = new ObservableCollection<ISeries>
                {
                    new LineSeries<ObservablePointF>
                    {
                        Values = _seriesData[i].points,
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

            List<ObservablePointF> points = new List<ObservablePointF>();
            int SIZE;
            int index;

            FlightDataContainer data;
            switch (name)
            {
                case "VM_TwoFeaturesGraphs_SelectedString":
                    index = 0;
                    if (string.IsNullOrEmpty(VM_TwoFeaturesGraphs_SelectedString))
                    {
                        _seriesData[index].points.Clear();
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

            _seriesData[index].points.Clear();

            if (data?.values == null)
            {
                // force restart of points
                ObservablePointF temp = new ObservablePointF(0, 0);
                _seriesData[index].points.Add(temp);
                return;
            }

            float y;
            SIZE = data.values.Length;

            for (int x = 0; x < SIZE; x++)
            {
                y = (float)data.values[x];
                _seriesData[index].points.Add(new ObservablePointF(x, y));
            }

            // set y axis range a bit bigger than min and max values
            _seriesData[index].YAxes[0].MinLimit = data.minValue * 1.1;
            _seriesData[index].YAxes[0].MaxLimit = data.maxValue * 1.1;
        }

        public FlightDataContainer getFeatureData(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;
            if (_model.dataDictionary.ContainsKey(name))
            {
                return _model.dataDictionary[name];
            }

            return null;
        }

        public string VM_TwoFeaturesGraphs_SelectedString => _model.SelectedString;

        public string VM_TwoFeaturesGraphs_CorrelatedString => _model.CorrelatedString;

        public int VM_TwoFeaturesGraphs_CurrentLineNumber => _model.CurrentLineNumber;
    }
}