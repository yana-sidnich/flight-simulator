using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightGearTestExec
{
    class FlightSimuatorSingleton
    {
        private static readonly FlightSimulator instance = new FlightSimulator();

        private FlightSimuatorSingleton() { }

        public static IFlightSimulator simulator
        {
            get
            {
                return instance;
            }
        }
    }
}
