using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightGearTestExec
{
    interface IFlightSimulator : INotifyPropertyChanged
    {
        public void Connect(string IP, int Port);

        public void Disconnect();

        public bool isConnected();

        public void Start();

        public void Stop();

        public bool isRunning();

        public double getRequetedProp(string propName);
        public void executeSimulator();
        public SimulatorConf configuration();

    }
}
