using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GnssLibDL
{
    internal class SimulationRunTime
    {
        private bool simulate;
        private SimulationController sc;
        public SimulationRunTime(SimulationController scIN) {
            sc = scIN;
            simulate = true;
        }

        public void RunSimulation()
        {
            long currentTimeMillis;
            while (simulate)
            {
                currentTimeMillis = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                sc.Tick();

                currentTimeMillis = DateTimeOffset.Now.ToUnixTimeMilliseconds() - currentTimeMillis;

                Thread.Sleep(1000 - (int) currentTimeMillis);

            }
        }

        public void StopSimulation()
        {
            simulate = false;
        }


    }
}
