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
        private string dialog(FileDialog dialog)
        {
            dialog.ShowDialog();
            string file = dialog.FileName;
            Trace.WriteLine($"Dialog found file {file}");
            return file;
        }
        private string dialog(System.Windows.Forms.FolderBrowserDialog dialog)
        {
            dialog.ShowDialog();
            string folder = dialog.SelectedPath;
            Trace.WriteLine($"Dialog found folder {folder}");
            return folder;
        }
        private void OpenExplorer(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            OpenFileDialog ofd = new OpenFileDialog();
            if (b == chooseTestCSV)
                file_explorer_vm.vm_file_explorer_test_csv = dialog(new OpenFileDialog());
            else if (b == chooseTrainCSV)
                file_explorer_vm.vm_file_explorer_train_csv = dialog(new OpenFileDialog());
            else if (b == chooseFlightSimulator)
                file_explorer_vm.vm_file_explorer_simulator_path = dialog(new System.Windows.Forms.FolderBrowserDialog());
            else
                ;//pathDLL = s;
        }
    }
}

