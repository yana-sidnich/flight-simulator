using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace FlightGearTestExec
{
    class SimulatorConf
    {
        private string simulatorPath;
        public string simulatorBinaryPath;
        public string flightXMLPath;
        public string fgRoot;
        public string SimulatorPath
        {
            get { return this.simulatorPath; }
            set
            {
                Trace.Write(value);
                this.simulatorPath = value;
                this.simulatorBinaryPath = Path.Combine(this.simulatorPath, @"bin\fgfs");
                this.flightXMLPath = Path.Combine(this.simulatorPath, @"data\Protocol\playback_small.xml");
                this.fgRoot = Path.Combine(this.simulatorPath, @"data\");
                Trace.Write(this.simulatorPath);


                // + "\\bin\\fgfs";
                //this.flightXMLPath = value + "\\data\\Protocol\\playback_small.xml";
                //this.fgRoot = value + "\\data\\";
            }
        }
        public string FlightTestCSVPath { get; set; }
        public string FlightTrainCSVPath { get; set; }
        public string FlightTestCSVPath_WithColumns { get; set; }
        public string FlightTrainCSVPath_WithColumns { get; set; }
        public string AnomalyAlgorithmDLL { get; set; }

        public string FlightXMLPath { get { return flightXMLPath; } }
        public string FgRoot { get { return fgRoot; } }
    }

    

}
