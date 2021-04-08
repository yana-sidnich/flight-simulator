using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace FlightGearTestExec
{
    class joystickViewModel : INotifyPropertyChanged
    {
        private IFlightSimulator flightSimulatorModel;
        public event PropertyChangedEventHandler PropertyChanged;

        public joystickViewModel(IFlightSimulator flightSimulatorModel)
        {
            this.flightSimulatorModel = flightSimulatorModel;
            this.flightSimulatorModel.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {
                    this.NotifyPropertyChanged("vm_joystick_" + e.PropertyName);
                };
        }

        private double convert(double d)
        {
            return 125 + 80 * d; 
        }

        public double vm_joystick_throttle_1
        {
            get {
                Trace.WriteLine($"throttle_1 val : {flightSimulatorModel.getRequetedProp("throttle_1")}"); 
                return flightSimulatorModel.getRequetedProp("throttle_1"); }
            set { }
        }
        public double vm_joystick_rudder
        {
        get { return flightSimulatorModel.getRequetedProp("rudder"); }
            set { }
        }
        public double vm_joystick_elevator
        {   
            get {
                return convert(flightSimulatorModel.getRequetedProp("elevator")); }
            set { }
        }
        public double vm_joystick_aileron
        {
            get { return convert(flightSimulatorModel.getRequetedProp("aileron")); }
            set { }
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
