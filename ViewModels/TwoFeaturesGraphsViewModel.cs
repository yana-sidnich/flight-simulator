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
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

// using FlightGearTestExec.Models;

namespace FlightGearTestExec.ViewModels
{
    public class TwoFeaturesGraphsViewModel : BaseViewModel
    {
        public class SeriesHolder
        {
            public ObservableCollection<ObservablePointF> points;

            public ObservableCollection<ISeries> series;
            public SeriesHolder()
            {
                points = new ObservableCollection<ObservablePointF>();
            }

        }
        private readonly FlightSimulator _model;
        private const int STROKE_THICKNESS = 5;

        private readonly SKColor Color_0, Color_1;
        private ObservableCollection<ObservablePointF> Points_0 { get; set; }
        private ObservableCollection<ObservablePointF> Points_1 { get; set; }
        private SKColor[] ColorsArray { get; set; }
        
        public ObservableCollection<ISeries> Series_0 { get; set; }
        public ObservableCollection<ISeries> Series_1 { get; set; }

        public List<SeriesHolder> seriesData { get; set; }  = new List<SeriesHolder>(2);

        private Task task;
        private Random rnd;

        public TwoFeaturesGraphsViewModel()
        {
            ColorsArray = new SKColor[] { SKColors.DarkTurquoise, SKColors.DarkSeaGreen };

            _model = model as FlightSimulator;
            _model.PropertyChanged +=
                delegate(Object sender, PropertyChangedEventArgs e)
                {
                    this.NotifyPropertyChanged("VM_TwoFeaturesGraphs_" + e.PropertyName);
                };

            // Points_0 = new ObservableCollection<ObservablePointF>();
            // Points_1 = new ObservableCollection<ObservablePointF>();
            setGraphValues();

        }

        private void setGraphValues()
        {
            for (int i = 0; i < 2; i++)
            {
                seriesData.Add(new SeriesHolder());
                seriesData[i].series = new ObservableCollection<ISeries>
                {
                    new LineSeries<ObservablePointF>
                    {
                        Values = seriesData[i].points,
                        Fill = null,
                        GeometryFill = null,
                        GeometrySize = 0,
                        Stroke = new SolidColorPaintTask { Color = ColorsArray[i], StrokeThickness = STROKE_THICKNESS },
                    }
                };
            }

            Series_0 = seriesData[0].series;
            Series_1 = seriesData[1].series;
        }

        public void resetPoints(int index, List<ObservablePointF> points)
        {
            seriesData[index].points.Clear();
            foreach (ObservablePointF point in points)
            {
                seriesData[index].points.Add(point);
            }
        }

        public double[] getFeatureValues(string name)
        {
            if (_model.dataDictionary.ContainsKey(name))
            {
                return _model.dataDictionary[name].values;
            }

            return null;
        }

        public string VM_TwoFeaturesGraphs_SelectedString => _model.SelectedString;

        public string VM_TwoFeaturesGraphs_CorrelatedString => _model.CorrelatedString;
    }
}