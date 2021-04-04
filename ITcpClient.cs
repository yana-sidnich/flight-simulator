using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace FlightGearTestExec
{
    interface ITcpClient
    {
        public void connect(string ip, int port);

        public void disconnect();

        public bool isConnected();

        public void send(byte[] data);

        public void recieve();

        public void flushStream();



    }
}
