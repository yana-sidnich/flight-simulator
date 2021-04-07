using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.ComponentModel;
using System.Net;

namespace FlightGearTestExec
{
    class FlightSimulator : IFlightSimulator
    {
       
        private ITcpClient myClient;
        Process simulatorExec;
        private SimulatorConf conf;
        private DataHandler dataHandler;


        public event PropertyChangedEventHandler PropertyChanged;
        // can be configure from multiple threads.
        private volatile int numOfRow;
        private volatile int speed;
        private volatile bool stopped;

        public FlightSimulator()
        {
            this.myClient = new MyTcpClient();
            string jsonString = File.ReadAllText("FlightConf.json");

            this.conf = new SimulatorConf();

            this.numOfRow = 0;
            this.speed = 100;
            this.stopped = true;

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
                return this.dataHandler.DataByColumn[propName][this.numOfRow];
            }
            return 0.0;
        }

        private void WaitForNextInput(int timeDiff)
        {
            // in correct the time taken to notify all changes, and wait only differnce needed.
            if (timeDiff < this.speed)
            {
                Thread.Sleep(this.speed - timeDiff);
            }
        }
        public void Start()
        {

            Trace.Write(this.isConnected());
            this.dataHandler = new DataHandler(this.conf.FlightTestCSVPath, this.conf.FlightXMLPath);
            Trace.Write(this.dataHandler.DataByRow.Count);
            Thread t = new Thread (new ThreadStart((delegate()
            {
                this.stopped = false;

                foreach (string line in this.dataHandler.DataByRow)
                {
                    Trace.WriteLine(line.Length);
                    Trace.WriteLine("raw number is");
                    Trace.WriteLine(numOfRow);
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
                    WaitForNextInput(after - before);
                    // check if after waiting
                    if (stopped)
                    {
                        break;
                    }
                    // iterate to next line, please note that this variable can be configured from an outside source. 
                    this.numOfRow++;
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
        public void executeSimulator()
        {
            ProcessStartInfo info = new ProcessStartInfo(conf.SimulatorPath);
            string playback_loc = "playback_small";
            info.ArgumentList.Add($"--fg-root={conf.FgRoot}");
            info.ArgumentList.Add($"--fdm=null");
            info.ArgumentList.Add($"--generic=socket,in,10,127.0.0.1,5400,tcp,{playback_loc}");
            info.UseShellExecute = false;

            simulatorExec = Process.Start(info);

        }

        public bool isRunning()
        {
            return !(this.stopped);
        }

        public SimulatorConf configuration()
        {
            return this.conf;
        }
    }
}
