using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using System.Windows.Input;
using System.Diagnostics;

namespace FlightGearTestExec.ViewModels
{
    class FileExplorerViewModel : BaseViewModel
    {
        private readonly IFlightSimulator model;
        public FileExplorerViewModel()
        {
            model = simulator;
            this.model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {
                    this.NotifyPropertyChanged("VM_FileExplorer_" + e.PropertyName);
                };

            //set default values:
            VM_FileExplorer_SimulatorPath = "C:\\Program Files\\FlightGear 2019.1.2";
        }
        public string VM_FileExplorer_FlightTrainCSVPath
        {
            get { return simulator.Configuration.FlightTrainCSVPath; }
            set
            {
                if (string.IsNullOrEmpty(value)) return;
                this.model.Configuration.FlightTrainCSVPath = value;
                NotifyPropertyChanged("VM_FileExplorer_FlightTrainCSVPath");
            }
        }

        public string VM_FileExplorer_FlightTestCSVPath
        {
            get { return simulator.Configuration.FlightTestCSVPath; }
            set
            {
                if (string.IsNullOrEmpty(value)) return;
                this.model.Configuration.FlightTestCSVPath = value;
                NotifyPropertyChanged("VM_FileExplorer_FlightTestCSVPath");
            }
        }

        public string VM_FileExplorer_SimulatorPath
        {
            get { return this.model.Configuration.SimulatorPath; }
            set
            {
                if (string.IsNullOrEmpty(value)) return;
                Trace.WriteLine($"change simulator path ${value}");
                this.model.Configuration.SimulatorPath = value; NotifyPropertyChanged("VM_FileExplorer_SimulatorPath");
            }
        }
        public string VM_FileExplorer_AnomalyAlgorithmDLL
        {
            get { return this.model.Configuration.AnomalyAlgorithmDLL; }
            set
            {
                if (string.IsNullOrEmpty(value)) return;
                Trace.WriteLine($"change anomaly dll path ${value}");
                this.model.Configuration.AnomalyAlgorithmDLL = value;
                NotifyPropertyChanged("VM_FileExplorer_AnomalyAlgorithmDLL");
            }
        }
    }

}
