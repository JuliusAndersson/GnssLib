using GnssLibDL;
using GnssLibNMEA_Writer;
using System.Diagnostics;
using System.Globalization;
using System.IO.Ports;



namespace GnssLibGUI
{
    public partial class GUI_Window : Form
    {
        System.Windows.Forms.Timer timerRunTime = new System.Windows.Forms.Timer();
        private SimulationController? sc;
        private SimulationRunTime srt;
        private bool stopClear = true;
        private SerialPort serialPort;

        public GUI_Window()
        {
            InitializeComponent();
            serialPort = new SerialPort("COM1", 4800, Parity.None, 8, StopBits.One);
            serialPort.Handshake = Handshake.None;

            setFile.Items.Add("File 1");
            setFile.Items.Add("File 2");
            setFile.Items.Add("File 3");
            setFile.SelectedIndex = 0;

            timerRunTime.Tick += HandleRunTime;
            timerRunTime.Interval = 1000;

            srt = new SimulationRunTime();
            srt.tickDone += HandleTickEvent;

        }

        private void Simulate_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort.Open();

                DateTime dt;
                Stop.Text = "Stop";
                stopClear = false;
                if (setFile.SelectedIndex == 0)
                {
                    dt = new DateTime(2024, 01, 01, (int)setHour.Value, (int)setMinute.Value, (int)setSecond.Value);
                }
                else if (setFile.SelectedIndex == 1)
                {
                    dt = new DateTime(2024, 01, 01, (int)setHour.Value, (int)setMinute.Value, (int)setSecond.Value);
                }
                else
                {
                    dt = new DateTime(2024, 01, 01, (int)setHour.Value, (int)setMinute.Value, (int)setSecond.Value);
                }

                double value;
                if (double.TryParse(setLat.Text.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out value)
                    && double.TryParse(setLong.Text.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out value)
                    && double.TryParse(setJammerLat.Text.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out value)
                    && double.TryParse(setJammerLong.Text.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out value))
                {
                    sc = new SimulationController(setGps.Checked, setGalileo.Checked, setGlonass.Checked, dt,
                        double.Parse(setLat.Text.Replace(',', '.'), CultureInfo.InvariantCulture),
                        double.Parse(setLong.Text.Replace(',', '.'), CultureInfo.InvariantCulture),
                        setIntOn.Checked,
                        double.Parse(setRadR.Value.ToString()),
                        double.Parse(setJammerLat.Text.Replace(',', '.'), CultureInfo.InvariantCulture),
                        double.Parse(setJammerLong.Text.Replace(',', '.'), CultureInfo.InvariantCulture));

                    timerRunTime.Start();

                    Terminal.Text += "Simulation Started" + Environment.NewLine;
                    Terminal.SelectionStart = Terminal.Text.Length;
                    Terminal.ScrollToCaret();
                }
                else
                {
                    Terminal.Text = "Invalid inputs (Check if you use , or . in Coordinates)" + Environment.NewLine;
                    Terminal.SelectionStart = Terminal.Text.Length;
                    Terminal.ScrollToCaret();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        public void HandleRunTime(object sender, EventArgs e)
        {
            srt.RunSimulation(sc);
            //Thread simulationThread = new Thread(() => srt.RunSimulation(sc));
            //simulationThread.Start();
            //Terminal.Text += "Tock" + Environment.NewLine;
            Terminal.SelectionStart = Terminal.Text.Length;
            Terminal.ScrollToCaret();
        }

        public void HandleTickEvent(object sender, EventArgs e)
        {




            //Terminal.Text += sc.GetValues().ElementAt(0).Azimuth + Environment.NewLine;


            Terminal.SelectionStart = Terminal.Text.Length;
            Terminal.ScrollToCaret();
            if (setNMEA.Checked) {
                NmeaStringsGenerator.NmeaGenerator(serialPort, sc);
            }
        }

        private void setRadR_Scroll(object sender, EventArgs e)
        {
            labelRadR.Text = setRadR.Value.ToString() + " km";
            
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            Stop.Text = "Clear";
            Terminal.Text += "Simulation Stopped" + Environment.NewLine;
            Terminal.SelectionStart = Terminal.Text.Length;
            Terminal.ScrollToCaret();
            timerRunTime.Stop();


            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }

            if (stopClear)
            {
                Terminal.Clear();
            }
            stopClear = true;
        }

        private void updatePos_Click(object sender, EventArgs e)
        {
            if (sc != null) {
                sc.UpdatePos(double.Parse(setLong.Text.Replace(',', '.'), CultureInfo.InvariantCulture), double.Parse(setLong.Text.Replace(',', '.'), CultureInfo.InvariantCulture));
                Terminal.Text += "New Position Value Set: " + setLat.Text + " " + setLong.Text + Environment.NewLine;
                Terminal.SelectionStart = Terminal.Text.Length;
                Terminal.ScrollToCaret();
            }
            
        }

        private void updateJammerPos_Click(object sender, EventArgs e)
        {
            if (sc != null)
            {
                sc.UpdateJammerPos(setIntOn.Checked, double.Parse(setJammerLat.Text.Replace(',', '.'), CultureInfo.InvariantCulture), double.Parse(setJammerLong.Text.Replace(',', '.'), CultureInfo.InvariantCulture), double.Parse(setRadR.Value.ToString()));
                Terminal.Text += "New Jammer Value Set: " + setJammerLat.Text + " " + setJammerLong.Text + Environment.NewLine;
                Terminal.SelectionStart = Terminal.Text.Length;
                Terminal.ScrollToCaret();
            }
             

        }

    }
}