
namespace GnssLibDL
{
    /// <summary>
    /// The Class that decides when the program will run.
    /// </summary>
    
    public class SimulationRunTime
    {
        private event EventHandler _tickDone;
        public event EventHandler tickDone
        {
            add { _tickDone += value; }
            remove { _tickDone -= value; }
        }
        /// <summary>
        /// Method that runs the simulation.
        /// </summary>
        /// <param name="scIN">The simulationController used to run the simulaton.</param>
        public void RunSimulation(SimulationController scIN)
        {
            scIN.Tick();     
            _tickDone(this, EventArgs.Empty);
        }

    }
}
