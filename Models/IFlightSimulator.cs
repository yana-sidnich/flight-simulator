using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightGearTestExec.Models;

namespace FlightGearTestExec
{
    interface IFlightSimulator : INotifyPropertyChanged
    {
        public void Connect(string IP, int Port);

        public void Disconnect();

        public bool isConnected();

        public void Start();

        public void StartAfterConnect();

        public void Stop();

        public bool isRunning();

        public double getRequetedProp(string propName);
        public void executeSimulator(string ip, string port);
        public void SetSpeed(double value);

        public int GetCurrentLine();
        public int GetNumLines();
        public void SetCurrentLine(int value);

        public void pauseRun();
        public void unPauseRun();

        // PROPERTIES

        public SimulatorConf Configuration
        {
            get;
        }

        public double Speed
        {
            get;
        }

        public string SelectedString
        {
            get;
            set;
        }
        public string CorrelatedString
        {
            get;
            set;
        }

        public Dictionary<string, string> CorrelatedFeatures
        {
            get;
            set;
        }

        public Dictionary<string, FlightDataContainer> DataDictionary
        {
            get;
            set;
        }        

    }
}