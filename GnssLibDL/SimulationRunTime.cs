using System.Timers;

namespace GnssLibDL
{
    /// <summary>
    /// The Class that decides when the program will run.
    /// </summary>
    public class SimulationRunTime
    {
        SimulationController _simulationController;
        private System.Timers.Timer _timerRunTime;
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
            
            _timerRunTime = new System.Timers.Timer();
            // Event that repeats every 1s
            _timerRunTime.Elapsed += RunTick;
            _timerRunTime.Interval = 1000;
            _timerRunTime.AutoReset = true;
            _timerRunTime.Start();
            _simulationController = scIN;
            _simulationController.Tick();
            _tickDone?.Invoke(this, EventArgs.Empty);
        }

        public void StopSimulation()
        {
            _timerRunTime.Stop();
            _timerRunTime.Dispose();
        }

        private void RunTick(object sender, ElapsedEventArgs e)
        {
            _simulationController.Tick();
            _tickDone?.Invoke(this, EventArgs.Empty);
        }
    }
}