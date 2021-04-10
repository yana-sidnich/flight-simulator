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
        protected static IFlightSimulator model = new FlightSimulator();

        public event PropertyChangedEventHandler PropertyChanged;

        protected IFlightSimulator simulator;

        public static void  SetModel(IFlightSimulator simulator_model)
        {
            model = simulator_model;
        }
        public BaseViewModel()
        {
            this.simulator = model;
        }

        protected void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}