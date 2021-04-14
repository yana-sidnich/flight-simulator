using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FlightGearTestExec.Models;
using FlightGearTestExec.Views;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;

namespace FlightGearTestExec.ViewModels
{
    class MapsViewModel : BaseViewModel
    {
        private readonly IFlightSimulator _model;
        private List<float[]> _gpsDataXY { get; set; }
        private List<float[]> _gpsDataLatLon { get; set; }

        const string urlTemplate = "https://static-maps.yandex.ru/1.x/?lang=en-US&ll={0},{1}&z={2}&l=map&size={3},{3}";

        private int size = 300;
        private int _zoom;
        private float latAvg = 0;
        private float lonAvg = 0;
        public MapsViewModel()
        {
            _gpsDataLatLon = new List<float[]>();
            GpsDataXY = new List<float[]>();
            _model = simulator;
            VM_Maps_Zoom = 11;
            getGpsPointsData();
            downloadImage(getUrl(latAvg, lonAvg));
            _model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {
                    this.NotifyPropertyChanged("VM_Maps_" + e.PropertyName);
                };
        }

        const double OFFSET = 268435456;
        const double RADIUS = OFFSET / Math.PI;

        static int[] Adjust(double X, double Y, double xcenter, double ycenter,
            int zoom, int size)
        {
            int xr = (LToX(X) - LToX(xcenter)) >> (21 - zoom);
            int yr = (LToY(Y) - LToY(ycenter)) >> (21 - zoom);
            int[] p = new[] { size / 2 - xr, size / 2 - yr };
            return p;
        }

        static int LToX(double x)
        {
            return (int)(Math.Round(OFFSET + RADIUS * x * Math.PI / 180));
        }

        static int LToY(double y)
        {
            return (int)(Math.Round(OFFSET - RADIUS * Math.Log((1 +
                                                                Math.Sin(y * Math.PI / 180)) / (1 - Math.Sin(y *
                Math.PI / 180))) / 2));
        }

        private void updateLatLonPoitnsToXY()
        {
            double tileSize = size / Math.Pow(2, _zoom);
            for (int i = 0; i < _gpsDataLatLon.Count; i++)
            {
                int[] xy = Adjust(_gpsDataLatLon[i][0], _gpsDataLatLon[i][1], latAvg, lonAvg, _zoom, size);
                _gpsDataXY[i][0] = (float)xy[1];
                _gpsDataXY[i][1] = (float)xy[0];
            }
        }

        private void getGpsPointsData()
        {
            int size = Math.Min(_model.DataDictionary["latitude-deg"].values.Count, _model.DataDictionary["longitude-deg"].values.Count);
            float lat, lon;
            for (int i = 0; i < size; i++)
            {
                lat = _model.DataDictionary["latitude-deg"].values[i];
                lon = _model.DataDictionary["longitude-deg"].values[i];
                _gpsDataLatLon.Add(new[] { lat, lon });
                _gpsDataXY.Add(new[] { -1.0f, -1.0f });
                latAvg += lat;
                lonAvg += lon;
            }
            latAvg /= size;
            lonAvg /= size;
        }


        private string getUrl(float lat, float lon)
        {
            string url = String.Format(urlTemplate, lon.ToString(), lat.ToString(), _zoom.ToString(), size.ToString());
            return url;
        }

        public async void downloadImage(string url)
        {
            var imgUrl = new Uri(url);
            VM_Maps_BitmapArray = await new System.Net.WebClient().DownloadDataTaskAsync(imgUrl);

        }

        public int VM_Maps_CurrentLineNumber => this._model.GetCurrentLine();

        private byte[] _bitmapImageArray;
        public byte[] VM_Maps_BitmapArray
        {
            get { return _bitmapImageArray; }
            set
            {
                _bitmapImageArray = value;
                NotifyPropertyChanged("VM_Maps_BitmapArray");
            }
        }

        public int VM_Maps_Zoom
        {
            get => _zoom;
            set
            {
                _zoom = value;
                NotifyPropertyChanged("VM_Maps_Zoom");
            }
        }
        public List<float[]> GpsDataXY
        {
            get
            {

                updateLatLonPoitnsToXY();
                downloadImage(getUrl(latAvg, lonAvg));
                return _gpsDataXY;
            }
            set
            {
                _gpsDataXY = value;
                NotifyPropertyChanged("GpsDataXY");
            }
        }

    }
}