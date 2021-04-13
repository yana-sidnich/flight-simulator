using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;

namespace FlightGearTestExec.ViewModels
{
    class ConnectionViewModel : BaseViewModel
    {
        private readonly IFlightSimulator model;
        public ConnectionViewModel()
        {
            this.model = simulator;
            this.model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {
                    this.NotifyPropertyChanged("vm_connection_" + e.PropertyName);
                };
        }

        public void connect(string ip, string port)
        {
            Trace.WriteLine($"ip {ip}, port {port}");
            this.model.Connect(ip, Int32.Parse(port));
        }

        public void Start()
        {
            this.model.StartAfterConnect();
        }

        public void executeSimulator(string ip, string port)
        {
            this.model.executeSimulator(ip, port);
        }
    }
}
