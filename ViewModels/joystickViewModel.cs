using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using FlightGearTestExec.Models;


namespace FlightGearTestExec.ViewModels
{
    class JoystickViewModel : BaseViewModel
    {
        private readonly IFlightSimulator model;
        private readonly Dictionary<string, FlightDataContainer> dataDictionary;
        private int line;
        public JoystickViewModel()
        {
            this.model = simulator;
            this.dataDictionary = this.model.DataDictionary;
            this.model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {
                    if (e.PropertyName == "CurrentLineNumber")
                    {
                        updateLine();
                    }
                    else
                    {
                        this.NotifyPropertyChanged("VM_Joystick_" + e.PropertyName);
                    }
                };
        }

        public int VM_Joystick_CurrentLineNumber => this.model.GetCurrentLine();


        private double _throttle_1;
        private double _rudder;
        private double _elevator;
        private double _aileron;
        public void updateLine()
        {
            int newLine = VM_Joystick_CurrentLineNumber;
            if (newLine != line)
            {
                line = newLine;
                VM_Joystick_throttle_1 = convert(dataDictionary["throttle_1"].values[line]);
                VM_Joystick_rudder = convert(dataDictionary["rudder"].values[line]);
                VM_Joystick_elevator = convert(dataDictionary["elevator"].values[line]);
                VM_Joystick_aileron = convert(dataDictionary["aileron"].values[line]);
            }
        }
        public double VM_Joystick_throttle_1
        {
            get
            {
                return _throttle_1;
            }
            set
            {
                _throttle_1 = value;
                NotifyPropertyChanged("VM_Joystick_throttle_1");
            }
        }

        public double VM_Joystick_rudder
        {
            get
            {
                return _rudder;
            }
            set
            {
                _rudder = value;
                NotifyPropertyChanged("VM_Joystick_rudder");
            }
        }
        public double VM_Joystick_elevator
        {
            get
            {
                return _elevator;
            }
            set
            {
                _elevator = value;
                NotifyPropertyChanged("VM_Joystick_elevator");
            }
        }
        public double VM_Joystick_aileron
        {
            get
            {
                return _aileron;
            }
            set
            {
                _aileron = value;
                NotifyPropertyChanged("VM_Joystick_aileron");
            }
        }
        
        private double convert(double d)
        {
            return 125 + 80 * d;
        }

    }
}
