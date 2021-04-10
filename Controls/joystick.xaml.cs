using FlightGearTestExec.ViewModels;
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
    /// Interaction logic for joystick.xaml
    /// </summary>
    public partial class joystick : UserControl
    {
        private joystickViewModel joystick_vm;

        public joystick()
        {
            InitializeComponent();

            this.joystick_vm = new joystickViewModel();
            this.DataContext = joystick_vm;

        }

        public void centerKnob_Completed(Object sender, EventArgs e)
        {
            
        }
    }
}
