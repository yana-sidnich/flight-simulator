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
    class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected static IFlightSimulator simulator = new FlightSimulator();

        public static void  SetModel(IFlightSimulator simulator_model)
        {
            simulator = simulator_model;
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