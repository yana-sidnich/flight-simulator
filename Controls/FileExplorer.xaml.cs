using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace FlightGearTestExec.Controls
{
    /// <summary>
    /// Interaction logic for FileExplorer.xaml
    /// </summary>
    public partial class FileExplorer : UserControl
    {
        private FileExplorer_ViewModel file_explorer_vm;
        public FileExplorer()
        {
            InitializeComponent();
            this.file_explorer_vm = new FileExplorer_ViewModel(FlightSimuatorSingleton.simulator);
            this.DataContext = file_explorer_vm;

        }
        private void OpenExplorer(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            string s = ofd.FileName;
            Trace.WriteLine(s);
            if (b == chooseTestCSV)
                file_explorer_vm.vm_file_explorer_test_csv = s;
            else if (b == chooseTrainCSV)
                file_explorer_vm.vm_file_explorer_train_csv = s;
            else if (b == chooseFlightSimulator)
                file_explorer_vm.vm_file_explorer_simulator_path = s;
            else
                ;//pathDLL = s;
        }
    }
}

