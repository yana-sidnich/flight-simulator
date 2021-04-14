using System;
using System.Collections.Generic;
using System.ComponentModel;
using FlightGearTestExec.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace FlightGearTestExec.ViewModels
{
    class DashboardViewModel : BaseViewModel
    {
        private readonly IFlightSimulator model;
        private readonly Dictionary<string, FlightDataContainer> dataDictionary;
        private int line;
        public DashboardViewModel()
        {
            this.model = simulator;
            this.dataDictionary = this.model.DataDictionary;
            this.model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {
                    this.NotifyPropertyChanged("VM_Dashboard" + e.PropertyName);
                    if (e.PropertyName == "CurrentLineNumber")
                    {
                        updateLine();
                    }
                };
        }

        public int VM_DashboardCurrentLineNumber => this.model.GetCurrentLine();

        private void updateLine()
        {
            int newLine = VM_DashboardCurrentLineNumber;
            if (newLine != line)
            {
                line = newLine
;

                VM_Dashboard_Altitude = dataDictionary["altitude-ft"].values[line];
                VM_Dashboard_Airspeed = dataDictionary["airspeed-kt"].values[line];
                VM_Dashboard_Heading = dataDictionary["heading-deg"].values[line];
                VM_Dashboard_Pitch = dataDictionary["pitch-deg"].values[line];
                VM_Dashboard_Roll = dataDictionary["roll-deg"].values[line];
                // yaw
                VM_Dashboard_Side = dataDictionary["side-slip-deg"].values[line];
            }
        }


        private double _altitude;
        private double _airspeed;
        private double _heading;
        private double _pitch;
        private double _roll;
        private double _side_slip;

        public double VM_Dashboard_Altitude
        {
            get
            {
                return _altitude;
            }
            set
            {
                _altitude = value;
                NotifyPropertyChanged("VM_Dashboard_Altitude");
            }
        }


        public double VM_Dashboard_Airspeed
        {
            get
            {
                return _airspeed;
            }
            set
            {
                _airspeed = value;
                NotifyPropertyChanged("VM_Dashboard_Airspeed");
            }
        }
        public double VM_Dashboard_Heading
        {
            get
            {
                return _heading;
            }
            set
            {
                _heading = value;
                NotifyPropertyChanged("VM_Dashboard_Heading");
            }
        }
        public double VM_Dashboard_Pitch
        {
            get
            {
                return _pitch;
            }
            set
            {
                _pitch = value;
                NotifyPropertyChanged("VM_Dashboard_Pitch");
            }
        }
        public double VM_Dashboard_Roll
        {
            get
            {
                return _roll;
            }
            set
            {
                _roll = value;
                NotifyPropertyChanged("VM_Dashboard_Roll");
            }
        }
        // yaw
        public double VM_Dashboard_Side
        {
            get
            {
                return _side_slip;
            }
            set
            {
                _side_slip = value;
                NotifyPropertyChanged("VM_Dashboard_Side");
            }
        }
    }
}
