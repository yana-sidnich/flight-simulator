using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace FlightGearTestExec.ViewModels
{
    class DashboardViewModel : BaseViewModel
    {
        private readonly IFlightSimulator model;
        public DashboardViewModel()
        {
            this.model = simulator;
            this.model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {
                    this.NotifyPropertyChanged(/*"VM_Joystick_" +*/e.PropertyName);
                };
        }


        public double altitude
        {
            get {
                /*Trace.WriteLine($"throttle_1 val : {this.model.getRequetedProp("throttle_1")}");*/
                return this.model.getRequetedProp("altitude-ft"); }
            set { }
        }

        public double airspeed
        {
        get { return this.model.getRequetedProp("airspeed-kt"); }
            set { }
        }
        public double heading
        {   
            get {
                return convert(this.model.getRequetedProp("heading-deg")); }
            set { }
        }
        public double pitch
        {
            get { return convert(this.model.getRequetedProp("pitch-deg")); }
            set { }
        }
        public double roll
        {
            get { return convert(this.model.getRequetedProp("roll-deg")); }
            set { }
        }
        public double yaw
        {
            get { return convert(this.model.getRequetedProp("side-slip-deg")); }
            set { }
        }

        //TODO CREATE CONVERTER IN VIEW
        private double convert(double d)
        {
            return 125 + 80 * d; 
        }

    }
}
