using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using static FlightGearTestExec.Models.FlightDataContainer;

namespace FlightGearTestExec.Views
{
    public partial class MapsView : UserControl
    {
        // const string urlTemplate = "http://maps.google.com/maps?output=embed&ll={0},{1}&z=9";
        const string urlTemplate = "https://www.openstreetmap.org/?mlat={0}&mlon={1}&zoom=12";
        // const string urlTemplate = "https://wego.here.com/location/?map={0},{1}";
        private MapsViewModel vm;

        public MapsView()
        {
            InitializeComponent();
            vm = DataContext as MapsViewModel;
        }

        private void updateLocation_Click(object sender, RoutedEventArgs e)
        {
            List<double> latLon = vm.getLocation();
            string lat = latLon[0].ToString();
            string lon = latLon[1].ToString();
            String url = String.Format(urlTemplate, lat, lon);
            Browser.Navigate(url);
        }

    }
}

