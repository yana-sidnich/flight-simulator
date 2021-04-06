using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static FlightGearTestExec.FlightDataContainer;
using FlightGearTestExec.ViewModels;

namespace FlightGearTestExec.Views
{
    public partial class FeaturesListView : UserControl, INotifyPropertyChanged
    {
        private struct ListItem
        {
            public string FeatureName { get; set; }
            public string CorrelatedFeatures { get; set; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                NotifyPropertyChanged("Selected");
            }
        }

        private int _selected;
        // <src:UserControl DataContext="{Binding Path=DataContext.UserViewModel, RelativeSource={RelativeSource={RelativeSource Mode=FindAncestor, AncestorType={x:Type Window}}}">
        // <someNamespace:EricsUserControl DataContext="{Binding InstanceOfBindingObject}"/>
        public FeaturesListView()
        {
            InitializeComponent();
            // vm = new FeaturesListViewModel();
            Dictionary<string, FlightDataContainer> dataDictionary = get_should_be_data_context();
            foreach (var pair in dataDictionary)
            {
                ListItem temp = new ListItem { FeatureName = pair.Key, CorrelatedFeatures = pair.Value.correlatedFeatureName };
                FeaturesList.Items.Add(temp);
            }
        }

        private void List_Selected(object sender, RoutedEventArgs e)
        {
            ListView list = sender as ListView;
            Selected = list.SelectedIndex;
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}