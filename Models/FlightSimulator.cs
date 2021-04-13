using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.ComponentModel;
using System.Net;
using FlightGearTestExec.Models;

namespace FlightGearTestExec
{
    class FlightSimulator : IFlightSimulator
    {
        // we set 10 hz play rate
        private const float DEFAULT_SLEEP_TIME = 100.0f;
        private int sleepTime = (int) DEFAULT_SLEEP_TIME;

        private ITcpClient myClient;
        Process simulatorExec;
        private SimulatorConf conf;
        private DataHandler dataHandler;


        public event PropertyChangedEventHandler PropertyChanged;
        // can be configure from multiple threads.
        private volatile int currentLine;
        private double speed;
        private volatile bool forward;
        private volatile bool stopped;
        private volatile bool paused;
        private string _selectedString;
        private string _correlatedString;
        private Dictionary<string, FlightDataContainer> _dataDictionary;
        private Dictionary<string, string> _correlatedFeatures;

        // PROPERTIES
        public double Speed => this.speed;

        public string SelectedString
        {
            get { return _selectedString; }
            set
            {
                _selectedString = value;
                this.NotifyPropertyChanged("SelectedString");
            }
        }
        public string CorrelatedString
        {
            get { return _correlatedString; }
            set
            {
                _correlatedString = value;
                this.NotifyPropertyChanged("CorrelatedString");
            }
        }

        public Dictionary<string, string> CorrelatedFeatures
        {
            get
            {
                _correlatedFeatures.Clear();
                foreach (var pair in DataDictionary)
                {
                    _correlatedFeatures[pair.Key] = pair.Value.correlatedFeatureName;
                }

                return _correlatedFeatures;
            }
            set
            {
                foreach (var pair in value)
                {
                    _dataDictionary[pair.Key].correlatedFeatureName = pair.Value;
                }

                this.NotifyPropertyChanged("CorrelatedFeatures");
            }
        }

        public Dictionary<string, FlightDataContainer> DataDictionary
        {
            get
            {
                return _dataDictionary;
            }
            set
            {
                this.NotifyPropertyChanged("DataDictionary");
            }
        }

        public FlightSimulator()
        {
            this.myClient = new MyTcpClient();
            this.conf = new SimulatorConf();
            this._correlatedFeatures = new Dictionary<string, string>();
            this._dataDictionary = new Dictionary<string, FlightDataContainer>();
            this.currentLine = 0;
            this.speed = 1.0f;
            this.stopped = true;
            this.paused = false;
            this.forward = true;

        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public double getRequetedProp(string propName)
        {
            if (this.dataHandler != null)
            {
                return this.dataHandler.DataByColumn[propName][this.currentLine];
            }
            return 0.0;
        }

        public void executeSimulator(string ip, string port)
        {
            ProcessStartInfo info = new ProcessStartInfo(conf.simulatorBinaryPath);
            string playback_loc = "playback_small";
            info.ArgumentList.Add($"--fg-root={conf.FgRoot}");
            info.ArgumentList.Add($"--fdm=null");
            info.ArgumentList.Add($"--generic=socket,in,10,{ip},{port},tcp,{playback_loc}");
            info.UseShellExecute = false;
            Trace.WriteLine($"executing: --fg-root={conf.FgRoot}, --generic=socket,in,10,{ip},{port},tcp,{playback_loc}");

            this.simulatorExec = Process.Start(info);

        }

        private static string GetTempFile(string baseFileName, string extension)
        {
            var path = Path.GetTempPath();
            var fileName = baseFileName + Guid.NewGuid().ToString() + extension;
            return Path.Combine(path, fileName);
        }

        private void CreateTempFileNames()
        {
            this.conf.FlightTestCSVPath_WithColumns = GetTempFile("test_columns", ".csv");
            this.conf.FlightTrainCSVPath_WithColumns = GetTempFile("train_columns", ".csv");
        }

        public void Start()
        {
            // create temporal csv files with columns names in first row
            CreateTempFileNames();
            this.dataHandler = new DataHandler(this.conf.FlightTestCSVPath, this.conf.FlightTrainCSVPath,
                                        this.conf.FlightTestCSVPath_WithColumns, this.conf.FlightTrainCSVPath_WithColumns,
                                        this.conf.FlightXMLPath);
            // Trace.Write(this.dataHandler.DataByRow.Count);
            Thread t = new Thread(new ThreadStart((delegate ()
           {
               this.stopped = false;

               //foreach (string line in this.dataHandler.DataByRow)
               while (!this.stopped)
               {
                   if (paused)
                   {
                       // in case we are paused - we do busy waiting, stopping for the same time between frames.
                       // THat way we can lose a maximum if a frame when unpaused/ stopped.
                       Thread.Sleep(sleepTime);
                       // TODO change how you wait on pause
                       continue;
                   }
                   string line = this.dataHandler.DataByRow[currentLine];
                   byte[] byteLine = Encoding.ASCII.GetBytes(line + System.Environment.NewLine);
                   this.myClient.send(byteLine);
                   // check if not stopped
                   if (stopped)
                   {
                       break;
                   }
                   // iterate to next line, please note that this variable can be configured from an outside source. 

                   Thread.Sleep(sleepTime);
                   // check if after waiting
                   this.currentLine = this.forward ? Math.Min(this.currentLine + 1, GetNumLines() - 1) : Math.Max(this.currentLine - 1, 0);
                   if ((this.forward && this.currentLine == GetNumLines() - 1) || (!this.forward && this.currentLine == 0))
                   {
                       this.paused = true;
                   }

                   this.NotifyPropertyChanged("CurrentLineNumber");
                   if (stopped)
                   {
                       break;
                   }
               }
           })));
            t.Start();
        }

        public void Connect(string ip, int port)
        {
            this.myClient.connect(ip, port);
        }

        public void StartAfterConnect()
        {
            this.Start();
            if (this.dataHandler != null)
            {
                this._dataDictionary = this.dataHandler.DataDictionary;
            }
        }

        public void Disconnect()
        {
            this.myClient.disconnect();

        }

        public bool isConnected()
        {
            return this.myClient.isConnected();
        }
        public void Stop()
        {
            this.stopped = true;
        }

        public bool isRunning()
        {
            return !(this.stopped);
        }

        public SimulatorConf Configuration => this.conf;

        public void SetSpeed(double value)
        {
            this.speed = Math.Round(value, 1);
            this.sleepTime = (int) (DEFAULT_SLEEP_TIME / value);
            this.NotifyPropertyChanged("speed");
        }
        public void SetForward(bool forward)
        {
            this.forward = forward;
        }
        public bool GetForward()
        {
            return forward;
        }
        public int GetCurrentLine()
        {
            return currentLine;
        }
        public void SetCurrentLine(int value)
        {
            currentLine = value;
        }
        public int GetNumLines()
        {
            return this.dataHandler.DataByColumn.First().Value.Count;
        }

        public void pauseRun()
        {
            this.paused = true;
        }
        public void unPauseRun()
        {
            this.paused = false;

        }

    }
}
