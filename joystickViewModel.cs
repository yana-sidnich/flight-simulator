using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightGearTestExec
{
    class joystickViewModel : INotifyPropertyChanged
    {
        private IFlightSimulator flightSimulatorModel;
        public joystickViewModel(IFlightSimulator flightSimulatorModel)
        {
            this.flightSimulatorModel = flightSimulatorModel;
            this.flightSimulatorModel.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {
                    this.NotifyPropertyChanged("vm_joystick_" + e.PropertyName);
                };
        }

        public double vm_joystick_throttle
        {
            get { return flightSimulatorModel.getRequetedProp("throttle"); }
            set { }
        }
        public double vm_joystick_rudder
        {
        get { return flightSimulatorModel.getRequetedProp("rudder"); }
            set { }
        }
        public double vm_joystick_elevator
        {   
            get { return flightSimulatorModel.getRequetedProp("elevator"); }
            set { }
        }
        public double vm_joystick_aileron
        {
            get { return flightSimulatorModel.getRequetedProp("aileron"); }
            set { }
        }

        public event PropertyChangedEventHandler PropertyChanged;




        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
