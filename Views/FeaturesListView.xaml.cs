using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using FlightGearTestExec.Models;
using static FlightGearTestExec.Models.FlightDataContainer;
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

        private readonly FeaturesListViewModel vm;

        public FeaturesListView()
        {
            InitializeComponent();

            vm = DataContext as FeaturesListViewModel;

            vm.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {
                    if (e.PropertyName == "VM_FeatureList_CorrelatedFeatures")
                    {
                        UpdateListItems();
                    }
                };

            UpdateListItems();

            // set default value
            FeaturesList.SelectedIndex = 0;
        }

        private void List_Selected(object sender, RoutedEventArgs e)
        {
            if (sender is ListView list && list.SelectedValue != null)
            {
                if (list.SelectedValue is not ListItem item || vm == null) return;

                vm.VM_FeaturesList_SelectedString = item.FeatureName;
                vm.VM_FeaturesList_CorrelatedString = item.CorrelatedName;
            }
        }

        private void UpdateListItems()
        {
            Dictionary<string, FlightDataContainer> dataDictionary = vm?.getDictionary();
            List<ListItem> tempItems = new List<ListItem>();
            FeaturesList.Items.Clear();
            if (dataDictionary != null)
            {
                foreach (var pair in dataDictionary)
                {
                    ListItem temp = new ListItem
                    { FeatureName = pair.Key ?? "", CorrelatedName = pair.Value.correlatedFeatureName ?? "" };
                    tempItems.Add(temp);
                }
            }
            tempItems = tempItems.OrderBy(x => x.FeatureName).ToList();
            foreach (var item in tempItems)
            {
                FeaturesList.Items.Add(item);
            }
        }
    }
}