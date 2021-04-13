using FlightGearTestExec.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace FlightGearTestExec.Controls
{
    /// <summary>
    /// Interaction logic for connection.xaml
    /// </summary>
    public partial class ConnectionView : UserControl
    {
        private ConnectionViewModel _vm;
        public ConnectionView()
        {
            InitializeComponent();
            _vm = DataContext as ConnectionViewModel;
        }


        private void connectButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                this._vm.connect(this.ipTextBox.Text, this.portTextBox.Text);
            }
            catch (Exception er)
            {
                ErrorMessageWindow errWindow = new ErrorMessageWindow();
                Application.Current.MainWindow = errWindow;
                errWindow.Show();
                return;
            }
            this._vm.Start();
            MainWindow mainWindow = new MainWindow();
            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();
            var myWindow = Window.GetWindow(this);
            myWindow.Close();
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            this._vm.executeSimulator(this.ipTextBox.Text, this.portTextBox.Text);
        }
    }
}
