using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace FlightGearTestExec.ViewModels
{
    public class joystickViewModel : INotifyPropertyChanged
    {

        public joystickViewModel()
        {
            this.simulator.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {
                    this.NotifyPropertyChanged("vm_joystick_" + e.PropertyName);
                };
        }


        public double vm_joystick_throttle_1
        {
            get {
                Trace.WriteLine($"throttle_1 val : {simulator.getRequetedProp("throttle_1")}"); 
                return simulator.getRequetedProp("throttle_1"); }
            set { }
        }

        public double vm_joystick_rudder
        {
        get { return simulator.getRequetedProp("rudder"); }
            set { }
        }
        public double vm_joystick_elevator
        {   
            get {
                return convert(simulator.getRequetedProp("elevator")); }
            set { }
        }
        public double vm_joystick_aileron
        {
            get { return convert(simulator.getRequetedProp("aileron")); }
            set { }
        }

        //TODO CREATE CONVERTER IN VIEW
        private double convert(double d)
        {
            return 125 + 80 * d; 
        }

    }
}
