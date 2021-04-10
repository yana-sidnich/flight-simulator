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
    class connectionViewModel : BaseViewModel
    {
        public connectionViewModel()
        {
            this.simulator.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {
                    this.NotifyPropertyChanged("vm_connection_" + e.PropertyName);
                };
        }

        public void connect(string ip, string port)
        {
            Trace.WriteLine($"ip {ip}, port {port}");
            this.simulator.Connect(ip, Int32.Parse(port));
        }

        public void executeSimulator(string ip, string port)
        {
            this.simulator.executeSimulator(ip, port);
        }
    }
}
