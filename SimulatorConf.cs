using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace FlightGearTestExec
{
    class SimulatorConf
    {
        private string simulatorPath;
        private string flightXMLPath = "C:\\Program Files\\FlightGear 2019.1.2\\data\\Protocol\\playback_small.xml";
        private string fgRoot = "C:\\Program Files\\FlightGear 2019.1.2\\data\\";
        public string SimulatorPath
        {
            get { return this.simulatorPath; }
            set
            {
                this.simulatorPath = value;// + "\\bin\\fgfs";
                //this.flightXMLPath = value + "\\data\\Protocol\\playback_small.xml";
                //this.fgRoot = value + "\\data\\";
            }
        }
        public string FlightTestCSVPath { get; set; }
        public string FlightTrainCSVPath { get; set; }

        public string FlightXMLPath { get { return flightXMLPath; } }
        public string FgRoot { get { return fgRoot; } }
    }

    

}
