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

        /*private void Button_Click(object sender, RoutedEventArgs e)
        {
            string port = this.portTextBox.Text;
            if (validatedAndParsePort(port))
            {

            }
            
        }*/


        /*private bool validatedAndParsePort(string portStr)
        {
            int port = 0;
            if(int.TryParse(portStr, out port))
            {
                return (port > 1024 && port < 49151);

            }
        }*/

        private bool validateIp(string ip)
        {
            return true;
        }

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {

            
        }
    }
}
