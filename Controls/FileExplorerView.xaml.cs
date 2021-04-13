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
using FlightGearTestExec.ViewModels;

namespace FlightGearTestExec.Controls
{
    /// <summary>
    /// Interaction logic for FileExplorerView.xaml
    /// </summary>
    public partial class FileExplorerView : UserControl
    {
        private FileExplorerViewModel _vm;
        public FileExplorerView()
        {
            InitializeComponent();
            _vm = DataContext as FileExplorerViewModel;

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
            string output;
            if (b == chooseTestCSV)
            {
                output = dialog(new OpenFileDialog());
                _vm.VM_FileExplorer_FlightTestCSVPath = output ?? "";
            }
            else if (b == chooseTrainCSV)
            {
                output = dialog(new OpenFileDialog());
                _vm.VM_FileExplorer_FlightTrainCSVPath = output ?? "";
            }
            else if (b == chooseFlightSimulator)
            {
                output = dialog(new System.Windows.Forms.FolderBrowserDialog());
                _vm.VM_FileExplorer_SimulatorPath = output ?? "";
            }
            else if (b == chooseDLL)
            {
                output = dialog(new System.Windows.Forms.FolderBrowserDialog());
                _vm.VM_FileExplorer_AnomalyAlgorithmDLL = output ?? "";
            }
        }
    }
}

