using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Text.Json;
using System.Diagnostics;
using System.Net;

namespace FlightGearTestExec
{
    class MyTcpClient : ITcpClient
    {
        private TcpClient myClient;
       //private IPEndPoint endPoint;

        //private string filePath = @"C:\Users\yanaStudy\source\repos\FlighyGearConnection\reg_flight.csv";

        public MyTcpClient()
        {
            myClient = new TcpClient();
            string jsonString = File.ReadAllText("test.json");
            SimulatorConf conf = JsonSerializer.Deserialize<SimulatorConf>(jsonString);
            Trace.WriteLine(conf.FlightCSVPath);
            Trace.WriteLine(conf.SimulatorPath);


        }
        public void connect(string ip, int port)
        {
            try
            {
                this.myClient.Connect(ip, port);
            }
            catch (ArgumentNullException e)
            {
                Trace.WriteLine($"IP address given is null: '{e}'");
            }
            catch (ArgumentOutOfRangeException e)
            {
                Trace.WriteLine($"Port number is invalid: '{e}'");
            }
            catch (SocketException e)
            {
                Trace.WriteLine($"An error occurred when accessing the socket: '{e}'");
            }
            catch (ObjectDisposedException e)
            {
                Trace.WriteLine($"TcpClient is closed.: '{e}'");
            }
        }

        public void disconnect()
        {
            if (this.isConnected())
            {
                this.myClient.Close();
            }

            this.myClient = new TcpClient();
        }


        public bool isConnected()
        {
            return this.myClient.Connected;
        }

        public void recieve()
        {
            throw new NotImplementedException();
        }

        public void send(byte[] data)
        {
            NetworkStream sendStream = this.myClient.GetStream();
            sendStream.Write(data);
            sendStream.Flush();
            Trace.WriteLine("sending");
        }

        void ITcpClient.flushStream()
        {
            this.myClient.GetStream().Flush();
        }
    }
}
