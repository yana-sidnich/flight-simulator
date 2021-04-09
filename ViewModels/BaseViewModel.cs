using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;


namespace FlightGearTestExec.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected static IFlightSimulator model;

        public BaseViewModel(IFlightSimulator flightSimulatorModel)
        {
            model = flightSimulatorModel;
        }

        public BaseViewModel()
        {
        }

        protected void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
