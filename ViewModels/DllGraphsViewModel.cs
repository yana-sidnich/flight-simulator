using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace FlightGearTestExec.ViewModels
{
    class DllGraphViewModel : BaseViewModel
    {
        private dynamic _dllInterface;

        public dynamic dllGraphOutput;

        private readonly IFlightSimulator _model;

        private double _threshold = -1.0f;

        private string _selectedDll = "";

        private Visibility _isGraphVisible = Visibility.Visible;

        private bool isDllRunning = false;

        public Visibility IsGraphVisible
        {
            get => _isGraphVisible;
            set
            {
                if (value == _isGraphVisible) return;
                _isGraphVisible = value;
                NotifyPropertyChanged("IsGraphVisible");
                NotifyPropertyChanged("IsErrorVisible");
            }
        }

        public Visibility IsErrorVisible => IsGraphVisible == Visibility.Hidden ? Visibility.Visible : Visibility.Hidden;

        public class ComboBoxItem
        {
            public string Path { get; set; }
            public string Basename { get; set; }
        }

        public void ThresholdDragDone()
        {
            try
            {
                NotifyPropertyChanged("VM_DllGraph_CurrentThreshold");
                UpdateThreshold();
            }
            catch (Exception e)
            {
                Debug.Print("Can't change dll threshold");
            }
        }

        public DllGraphViewModel()
        {
            _model = simulator;
            _model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {
                    this.NotifyPropertyChanged("VM_DllGraph_" + e.PropertyName);
                };
        }


        private void loadDll()
        {
            Assembly dll = Assembly.LoadFile(_selectedDll);
            Type[] t = dll.GetExportedTypes();
            _dllInterface = null;
            foreach (Type type in t)
            {
                if (type.Name == "UCExported")
                    _dllInterface = Activator.CreateInstance(type);
            }
            if (_dllInterface == null)
                throw new Exception("Class UCExported Was Not Implemented In DLL");


            string test = _model.Configuration.FlightTestCSVPath_WithColumns;
            string train = _model.Configuration.FlightTrainCSVPath_WithColumns;
            // string test = "C:\\Users\\ItayYaakov\\OneDrive-BIU\\Desktop\\plugins\\reg_flight_col.csv";
            // string train = "C:\\Users\\ItayYaakov\\OneDrive-BIU\\Desktop\\plugins\\anomaly_flight_col.csv";
            dllGraphOutput = _dllInterface.CreateUC(test, train);
            if (dllGraphOutput != null)
            {
                isDllRunning = true;
                GetCorrelatedFeatures();
            }
        }

        public void UpdatePoints(string name)
        {
            if (name == null)
                return;

            switch (name)
            {
                case "VM_DllGraph_SelectedString":
                    if (string.IsNullOrEmpty(VM_DllGraph_SelectedString))
                    {
                        return;
                    }

                    if (isDllRunning)
                    {
                        _dllInterface.UpdateChosenFeature(VM_DllGraph_SelectedString);
                    }

                    break;
                case "VM_DllGraph_CorrelatedString":
                    if (string.IsNullOrEmpty(VM_DllGraph_CorrelatedString))
                    {
                        IsGraphVisible = Visibility.Hidden;
                        return;
                    }
                    IsGraphVisible = Visibility.Visible;
                    break;
                default:
                    return;
            }
        }

        public void UpdateThreshold()
        {
            if (isDllRunning)
            {
                isDllRunning = false;
                Dictionary<String, String> correlatedFeatures = _dllInterface.UpdateThreshold((float)_threshold);
                _model.CorrelatedFeatures = correlatedFeatures;
                isDllRunning = true;
            }
        }

        public void GetCorrelatedFeatures()
        {
            if (isDllRunning)
            {
                Dictionary<String, String> correlatedFeatures = _dllInterface.GetCorrelatedFeatures();
                _model.CorrelatedFeatures = correlatedFeatures;
            }
        }

        public void UpdateFrame(long timeStep)
        {
            if (isDllRunning)
            {
                if (Application.Current != null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        _dllInterface.UpdateFrame(timeStep);
                    });
                }
            }
        }

        public List<ComboBoxItem> VM_DllGraph_DllNames
        {
            get
            {
                DirectoryInfo dirInfo = new DirectoryInfo(_model.Configuration.AnomalyAlgorithmDLL);
                List<ComboBoxItem> dllFiles = new List<ComboBoxItem>();
                FileInfo[] info = dirInfo.GetFiles("*.dll");
                foreach (FileInfo f in info)
                {
                    ComboBoxItem c = new ComboBoxItem();
                    c.Path = f.FullName;
                    c.Basename = f.Name;
                    dllFiles.Add(c);
                }

                return dllFiles;
            }
        }

        public string VM_DllGraph_SelectedString => _model.SelectedString;

        public string VM_DllGraph_CorrelatedString => _model.CorrelatedString;

        public int VM_DllGraph_CurrentLineNumber => _model.GetCurrentLine();

        public double VM_DllGraph_CurrentThreshold
        {
            get => _threshold;
            set => _threshold = value;
        }

        private ComboBoxItem _selectedComboBoxItem;
        public ComboBoxItem VM_DllGraph_SelectedDll
        {
            get
            {
                return _selectedComboBoxItem;
            }
            set
            {
                ComboBoxItem item = value as ComboBoxItem;
                _selectedDll = item?.Path;
                isDllRunning = false;
                loadDll();
                NotifyPropertyChanged("VM_DllGraph_SelectedDll");
            }
        }

    }
}