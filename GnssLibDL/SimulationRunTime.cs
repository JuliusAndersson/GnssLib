
namespace GnssLibDL
{
    /// <summary>
    /// The Class that decides when the program will run
    /// </summary>
    /// <param name="scIN">The SimulationController</param>
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
