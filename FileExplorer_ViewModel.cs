using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using System.Windows.Input;
using FlightGearTestExec.Commands;

namespace FlightGearTestExec
{
    class FileExplorer_ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private IFlightSimulator simulator;

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
            set { simulator.configuration().SimulatorPath = value; }
        }
        public FileExplorer_ViewModel(IFlightSimulator simulator)
        {
            this.simulator = simulator;
            this.simulator.PropertyChanged +=
            delegate (Object sender, PropertyChangedEventArgs e)
            {
                this.NotifyPropertyChanged("vm_connection_" + e.PropertyName);
            };
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
