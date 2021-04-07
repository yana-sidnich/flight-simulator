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
        private ICommand connectCommand;


        public connectionViewModel(IFlightSimulator flightSimulatorModel)
        {
            connectCommand = new ConnectCommand(this);

            this.flightSimulatorModel = flightSimulatorModel;
            this.flightSimulatorModel.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {
                    this.NotifyPropertyChanged("vm_connection_" + e.PropertyName);
                };
        }

        public string vm_connection_ip
        {
            get { return ip; }
            set { ip = value; }
        }
        public int vm_connection_port
        {
            get { return port; }
            set { port = value; 
                Trace.WriteLine("port");
                Trace.WriteLine(port);
            }
        }


        public ICommand vm_connection_ConnectCommand
        {
            get
            {
                 return connectCommand;
            }
            set { }
        }
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public void connect()
        {
            this.flightSimulatorModel.Connect(this.ip, this.port);
        }

    }
}
