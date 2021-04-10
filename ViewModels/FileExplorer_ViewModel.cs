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
    class FileExplorer_ViewModel : BaseViewModel
    {
        public string vm_file_explorer_train_csv
        {
            get { return simulator.configuration().FlightTrainCSVPath; }
            set { simulator.configuration().FlightTrainCSVPath = value; }
        }

        public string vm_file_explorer_test_csv
        {
            get { return simulator.configuration().FlightTestCSVPath; }
            set { simulator.configuration().FlightTestCSVPath = value; }
        }

        public string vm_file_explorer_simulator_path
        {
            get { return simulator.configuration().SimulatorPath; }
            set {
                Trace.WriteLine($"change simulator path ${value}");
                simulator.configuration().SimulatorPath = value; }
        }
        public FileExplorer_ViewModel()
        {
            this.simulator.PropertyChanged +=
            delegate (Object sender, PropertyChangedEventArgs e)
            {
                this.NotifyPropertyChanged("vm_file_explorer_" + e.PropertyName);
            };
        }
    }

}
