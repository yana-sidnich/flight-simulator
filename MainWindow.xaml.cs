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
using System.Threading;

namespace FlightGearTestExec
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FlightSimulator sim;
        private joystickViewModel joystick_vm;
        private connectionViewModel connection_vm;

        public MainWindow()
        {
            InitializeComponent();
            this.sim = new FlightSimulator();
            this.joystick_vm = new joystickViewModel(sim);
            this.connection_vm = new connectionViewModel(sim);

            this.joystick.DataContext = joystick_vm;
            this.connection.DataContext = connection_vm;
            //sim.executeSimulator();
            
            
        }

        
       
    }
}
