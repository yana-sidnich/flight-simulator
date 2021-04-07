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
using static FlightGearTestExec.FlightDataContainer;

namespace FlightGearTestExec.Views
{
    /// <summary>
    /// Interaction logic for GraphsView.xaml
    /// </summary>
    public partial class GraphsView : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public GraphsView()
        {
            InitializeComponent();
            dictionarrrrrrry = get_should_be_data_context();
            featuresNamesList = new List<string>(this.dictionarrrrrrry?.Keys);
            featuresNamesList.Sort();
            featuresNamesList = featuresNamesList.Select(s => new { Str = s, Split = s.Split('_') })
                .OrderBy(x => int.Parse(x.Split[1]))
                .ThenBy(x => x.Split[0])
                .Select(x => x.Str)
                .ToList();
        }

        private int _selected_index;
        private int _correlated_index;
        private string _selected_string;
        private string _correlated_string;
        private Dictionary<string, FlightDataContainer> dictionarrrrrrry;
        private List<string> featuresNamesList;

        public int Selected
        {
            get { return _selected_index; }
            set
            {
                _selected_index = value;
                _selected_string = featuresNamesList?[_selected_index];
                // _correlated_index = List[_selected_index];
                // _correlated_string = featuresNamesList?[_correlated_index];
                _correlated_string = dictionarrrrrrry?[_selected_string].correlatedFeatureName;
            }
        }

        // public int Correlated_index
        // {
        //     get { return _correlated_index; }
        // }        
        public string Selected_string
        {
            get { return _selected_string; }
        }
        public string Correlated_string
        {
            get { return _correlated_string; }
        }


        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}

