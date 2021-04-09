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
    public partial class FeaturesListView : UserControl
    {
        private struct ListItem
        {
            public string FeatureName { get; set; }
            public string CorrelatedName { get; set; }
        }

        private FeaturesListViewModel vm;
        private int _selected;
        
        public FeaturesListView()
        {
            InitializeComponent();
            Dictionary<string, FlightDataContainer> dataDictionary = get_should_be_data_context();
            foreach (var pair in dataDictionary)
            {
                ListItem temp = new ListItem { FeatureName = pair.Key, CorrelatedName = pair.Value.correlatedFeatureName };
                FeaturesList.Items.Add(temp);
            }

            vm = DataContext as FeaturesListViewModel;
        }

        private void List_Selected(object sender, RoutedEventArgs e)
        {
            ListView list = sender as ListView;
            if (list != null && list.SelectedValue != null)
            {
                ListItem? item = list.SelectedValue as ListItem?;
                if (item != null && vm != null)
                {
                    vm.VM_FeaturesList_SelectedString = item.Value.FeatureName;
                    vm.VM_FeaturesList_CorrelatedString = item.Value.CorrelatedName;
                }
            }
        }
    }
}