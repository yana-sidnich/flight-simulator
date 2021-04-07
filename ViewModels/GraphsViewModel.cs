using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// using FlightGearTestExec.Models;

namespace FlightGearTestExec.ViewModels
{
    class GraphsViewModel: INotifyPropertyChanged
    {
        private readonly IFlightSimulator _model;
        public event PropertyChangedEventHandler PropertyChanged;

        public GraphsViewModel(IFlightSimulator model)
        {
            this._model = model;
            this._model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {
                    this.NotifyPropertyChanged("VM_Graphs_" + e.PropertyName);
                };
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}