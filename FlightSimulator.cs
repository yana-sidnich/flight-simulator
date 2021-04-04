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

namespace FlightGearTestExec
{
    class FlightSimulator
    {
        public ITcpClient myClient;
        Process simulatorExec;
        public SimulatorConf conf;
        private double throttle;
        private double rudder;
        private double elevator;
        private double aileron;

        public event PropertyChangedEventHandler PropertyChanged;

        private int numOfRow;
        private int speed = 100;

        public FlightSimulator()
        {
            this.myClient = new MyTcpClient();
            string jsonString = File.ReadAllText("test.json");
            this.conf = JsonSerializer.Deserialize<SimulatorConf>(jsonString);
        }

        public void connectAndTransmit()
        {
            this.myClient.connect("localhost", 5400);
            StreamReader reader = new StreamReader(File.OpenRead(this.conf.FlightCSVPath));
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                Trace.WriteLine(line.Length);
                line += System.Environment.NewLine;
                Trace.WriteLine(line.Length);
                byte[] byteLine = Encoding.ASCII.GetBytes(line);
                this.myClient.send(byteLine);
                Thread.Sleep(this.speed);
            }
        }

        public void executeSimulator()
        {
            ProcessStartInfo info = new ProcessStartInfo(conf.SimulatorPath);
            //info.Environment.Add("FG_ROOT", conf.FG_ROOT);
            //info.Environment.Add("FG_HOME", conf.FG_HOME);
            //info.Environment.Add("FG_SCENERY",conf.FG_SCENERY);
            //string playback_loc = "C:\\Program Files\\FlightGear 2019.1.2\\data\\Protocol\\playback_small";
            string playback_loc = "playback_small";
            info.ArgumentList.Add($"--fg-root={conf.FG_ROOT}");
            info.ArgumentList.Add($"--fdm=null");
            info.ArgumentList.Add($"--generic=socket,in,10,127.0.0.1,5400,tcp,{playback_loc}");
            info.UseShellExecute = false;

            simulatorExec = Process.Start(info);

        }

    }
}
