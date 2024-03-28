using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GnssLibDL
{
    public class SimulationRunTime
    {
        private bool simulate;
        private SimulationController sc;
        public event EventHandler tickDone;

        public SimulationRunTime() {
            
        }

        public void RunSimulation(SimulationController scIN)
        {
            sc = scIN;

            simulate = true;
            long currentTimeMillis;

            Task.Run(() =>
            {    
                while (simulate)
                {
                    currentTimeMillis = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                    sc.Tick();

                    currentTimeMillis = DateTimeOffset.Now.ToUnixTimeMilliseconds() - currentTimeMillis;

                    Thread.Sleep(1000 - (int) currentTimeMillis);
                    
                    tickDone(this, EventArgs.Empty);  
                }
            });
        }

        public void StopSimulation()
        {
            simulate = false;
        }


    }
}
