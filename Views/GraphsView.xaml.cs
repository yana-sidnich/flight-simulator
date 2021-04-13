using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using FlightGearTestExec.ViewModels;
using static FlightGearTestExec.Models.FlightDataContainer;

namespace FlightGearTestExec.Views
{
    public partial class GraphsView : UserControl
    {
        private GraphsViewModel vm;

        public GraphsView()
        {
            InitializeComponent();
            vm = DataContext as GraphsViewModel;
        }
    }
}

