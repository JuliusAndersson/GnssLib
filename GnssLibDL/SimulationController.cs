using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GnssLibDL
{
    internal class SimulationController
    {
        private bool useGPS { get; set; }
        private bool useGalileo { get; set; }
        private bool useGalileo { get; set; }

        public SimulationController(bool gps, bool galileo, bool glonass, DateTime dateTime, double latPos, double longPos, bool jammer, double jamRad, double jamLat, double jamLong) { 
            
        }

        public void Tick()
        {

        }


    }
}
