using System;
using System.Collections.Generic;
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

namespace FlightGearTestExec.Views
{
    public partial class LiveMapsView : UserControl
    {
        // const string urlTemplate = "http://maps.google.com/maps?output=embed&ll={0},{1}&z=9";
        const string yandexUrl = "https://static-maps.yandex.ru/1.x/?lang=en-US&ll={0},{1}&z={2}&l=map&size={3},{3}";
        // const string urlTemplate = "https://wego.here.com/location/?map={0},{1}";
        private MapsViewModel vm;
        private int Zoom = 11;
        double size = 300;
        private BitmapImage background;
        public LiveMapsView()
        {
            InitializeComponent();
            vm = DataContext as MapsViewModel;
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            double latMiddle = (63.97978031 + 63.99375811);
            double lonMiddle = (-22.64160255 + -22.60492759);
            // String url = String.Format(yandexUrl, lonMiddle, latMiddle, Zoom.ToString(), size.ToString());
            // bitmapImage.UriSource = new Uri(url);
            // bitmapImage.EndInit();
            //
            // background = bitmapImage;
        }

        private void updateLocation_Click(object sender, RoutedEventArgs e)
        {
            double latMiddle = (63.97978031 + 63.99375811);
            double lonMiddle = (-22.64160255 + -22.60492759);

            List<double> latLon = null; // vm.getLocation();
            string lat = latLon[0].ToString();
            string lon = latLon[1].ToString();  
            double latd = latLon[0];
            double lond = latLon[1];

            double tileSize = size / Math.Pow(2, Zoom);
            var xy = MapConverter.PositionToGlobalPixel(new double[] {latd, lond}, Zoom, tileSize);
            Debug.Print(xy[0] + "    y=" + xy[1]);
        }

    }
}

