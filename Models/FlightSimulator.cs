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

namespace FlightGearTestExec
{
    public class FlightSimulator : IFlightSimulator
    {
        private const int DEFAULT_MILLIS_PER_TICK = 100;

        private ITcpClient myClient;
        Process simulatorExec;
        private SimulatorConf conf;
        private DataHandler dataHandler;

        public event PropertyChangedEventHandler PropertyChanged;

        public Dictionary<string, FlightDataContainer> dataDictionary;
        // can be configure from multiple threads.
        private volatile int currentLine;
        private double speed;
        private volatile bool forward;
        private volatile bool stopped;
        private volatile bool paused;
        private string _selectedString;
        private string _correlatedString;

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

        public FlightSimulator()
        {
            get { return numOfRow; }
            set
            {
                numOfRow = value;
                this.NotifyPropertyChanged("CurrentLineNumber");
            }
        }

            this.conf = new SimulatorConf();
        public string SelectedString
        {
            get { return _selectedString; }
            set
            {
                _selectedString = value;
                this.NotifyPropertyChanged("SelectedString");
            }
        }

        public FlightSimulator()
        {
            this.myClient = new MyTcpClient();

            this.conf = new SimulatorConf();

            this.currentLine = 0;
            this.speed = 1.0f;
            this.stopped = true;
            this.paused = false;
            this.forward = true;

            // this.dataHandler = new DataHandler(this.conf.FlightCSVPath, this.conf.FlightXMLPath);
            dataDictionary = FlightDataContainer.get_should_be_data_context();
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        private void notifyAll()
        {
            foreach(string prop in this.dataHandler.DataByColumn.Keys)
            {
                this.NotifyPropertyChanged(prop);
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

        private void WaitForNextInput(int timeDiff = 0)
        {
            // in correct the time taken to notify all changes, and wait only differnce needed.
            int sleep = (int)(DEFAULT_MILLIS_PER_TICK / this.speed);
            Trace.WriteLine($"speed: {this.speed}");
            if (timeDiff < sleep)
            {
                Thread.Sleep(sleep - timeDiff);
            }
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
        public void Start()
        {
            Trace.Write(this.isConnected());
            this.dataHandler = new DataHandler(this.conf.FlightTestCSVPath, this.conf.FlightXMLPath);
            Trace.Write(this.dataHandler.DataByRow.Count);
            Thread t = new Thread (new ThreadStart((delegate()
            {
                this.stopped = false;

                //foreach (string line in this.dataHandler.DataByRow)
                while (!this.stopped)
                {
                    if (paused)
                    {
                        // in case we are pasued - we do busy waiting, stopping for the same time between frames.
                        // THat way we can lose a maximum if a frame when unpaused/ stopped.
                        WaitForNextInput();
                        continue;
                    }
                    Trace.WriteLine("raw number is");
                    Trace.WriteLine(currentLine);
                    string line = this.dataHandler.DataByRow[currentLine];
                    byte[] byteLine = Encoding.ASCII.GetBytes(line + System.Environment.NewLine);
                    this.myClient.send(byteLine);
                    
                    int before = DateTime.Now.Millisecond;
                    this.notifyAll();
                    int after = DateTime.Now.Millisecond;
                    // check if not stopped
                    if (stopped)
                    {
                        break;
                    }
                    // iterate to next line, please note that this variable can be configured from an outside source. 

                    WaitForNextInput(after - before);
                    // check if after waiting
                    this.currentLine = this.forward ? Math.Min(this.currentLine + 1, GetNumLines() - 1) : Math.Max(this.currentLine -1, 0);

                    this.NotifyPropertyChanged("current_line");
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
            this.Start();
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

        public SimulatorConf configuration()
        {
            return this.conf;
        }

        public double GetSpeed()
        {
            return this.speed;
        }
        public void SetSpeed(double value)
        {
            this.speed = Math.Round(value, 1);
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
            return this.dataHandler.DataByColumn.First().Value.Count; ;
        }

        public void pauseRun()
        {
            this.paused = true;
        }
        public void unPauseRun()
        {
            this.paused = false;

        }
        public float DefaultTicksPerSec()
        {

            return (float)(1000 / DEFAULT_MILLIS_PER_TICK);
        }

    }
}
