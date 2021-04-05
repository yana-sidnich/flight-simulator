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
        
        public MainWindow()
        {
            this.sim = new FlightSimulator();
            joystickViewModel vm = new joystickViewModel(sim);
            DataContext = vm;
            InitializeComponent();
            //sim.executeSimulator();
            sim.Connect("127.0.0.1", 5400);
            sim.Start();
        }

        
       
    }
}
