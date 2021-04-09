using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;

namespace FlightGearTestExec
{
    class connectionViewModel : INotifyPropertyChanged
    {
        private IFlightSimulator flightSimulatorModel;
        public event PropertyChangedEventHandler PropertyChanged;
        int port;
        string ip;


        public connectionViewModel(IFlightSimulator flightSimulatorModel)
        {
            this.flightSimulatorModel = flightSimulatorModel;
            this.flightSimulatorModel.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {
                    this.NotifyPropertyChanged("vm_connection_" + e.PropertyName);
                };
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public void connect(string ip, string port)
        {
            Trace.WriteLine($"ip {ip}, port {port}");
            this.flightSimulatorModel.Connect(ip, Int32.Parse(port));
        }

    }
}
