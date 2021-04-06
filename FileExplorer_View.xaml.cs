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
using Microsoft.Win32;

namespace WPF_OpenFileExplorer
{
    /// <summary>
    /// Interaction logic for DialogFileExplorer.xaml
    /// </summary>
    public partial class FileExplorer_View : UserControl
    {
        string pathTrainCSV = null;
        string pathTestCSV = null;
        string pathDLL = null;
        FileExplorer_ViewModel vm;
        public FileExplorer_View()
        {
            InitializeComponent();
            this.vm = new FileExplorer_ViewModel();
        }
        private void OpenExplorer(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            string s=ofd.FileName;
            if (b == chooseTestCSV)
                pathTestCSV = s;
            else if (b == chooseTrainCSV)
                pathTrainCSV = s;
            else
                pathDLL = s;
            if (pathDLL != null && pathTrainCSV != null && pathTestCSV != null)
                sendPaths();
        }
        private void sendPaths()
        {
            vm.sendPaths(pathTrainCSV, pathTestCSV, pathDLL);
        }
    }
}
