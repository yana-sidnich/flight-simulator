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

namespace FlightGearTestExec.Controls
{
    /// <summary>
    /// Interaction logic for connection.xaml
    /// </summary>
    public partial class connection : UserControl
    {
        private connectionViewModel connection_vm;
        public connection()
        {
            InitializeComponent();
            this.connection_vm = new connectionViewModel(FlightSimuatorSingleton.simulator);
            this.DataContext = connection_vm;

        }


        private void connectButton_Click(object sender, RoutedEventArgs e)
        {

            //this.validatedAndParsePort()
            try
            {
                this.connection_vm.connect(this.ipTextBox.Text, this.portTextBox.Text);
            }
            catch (Exception )
            {
                ErrorMessageWindow errWindow = new ErrorMessageWindow();
                Application.Current.MainWindow = errWindow;
                errWindow.Show();
                return;
            }
            MainWindow mainWindow = new MainWindow();
            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();
            var myWindow = Window.GetWindow(this);
            myWindow.Close();
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            FlightSimuatorSingleton.simulator.executeSimulator(this.ipTextBox.Text, this.portTextBox.Text);
        }
    }
}
