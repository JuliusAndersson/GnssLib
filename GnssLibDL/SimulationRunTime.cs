using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GnssLibDL
{
    public class SimulationRunTime
    {
        public event EventHandler _tickDone;
        public void RunSimulation(SimulationController scIN)
        {
            scIN.Tick();     
            _tickDone(this, EventArgs.Empty);
        }

    }
}
