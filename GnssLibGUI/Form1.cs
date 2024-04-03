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
        private List<String> fileList = new List<string>();
        private bool running = false;

        public GUI_Window()
        {
            InitializeComponent();
            serialPort = new SerialPort("COM1", 4800, Parity.None, 8, StopBits.One);
            serialPort.Handshake = Handshake.None;

            
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources/Broadcast");

            if (Directory.Exists(folderPath))
            {
                string[] fileNames = Directory.GetFiles(folderPath);

                Terminal.Text += "Files in the folder:" + Environment.NewLine;
                foreach (string fileName in fileNames)
                {
                    fileList.Add(Path.GetFileName(fileName));
                    int dayIndexs = fileName.IndexOf('_') + 7;
                    string days = fileName.Substring(dayIndexs, 3);
                    int yearIndexs = fileName.IndexOf('_') + 3;
                    string years = fileName.Substring(yearIndexs, 4);
                    setFile.Items.Add(Path.GetFileName("Day: " + days + " Year: " + years));
                }
            }
            else
            {
                Terminal.Text += "File does not exist.";
            }


            setFile.SelectedIndex = 0;

            timerRunTime.Tick += HandleRunTime;
            timerRunTime.Interval = 1000;

            srt = new SimulationRunTime();
            srt.tickDone += HandleTickEvent;

        }

        private void Simulate_Click(object sender, EventArgs e)
        {
            if (!running)
            {
                try
                {
                    serialPort.Open();
                    running = true;
                    DateTime dt;
                    Stop.Text = "Stop";
                    stopClear = false;

                    //Get DateTime from file
                    string fileName = fileList[setFile.SelectedIndex];
                    int dayIndex = fileName.IndexOf('_') + 7;
                    string day = fileName.Substring(dayIndex, 3);
                    int yearIndex = fileName.IndexOf('_') + 3;
                    string year = fileName.Substring(yearIndex, 4);
                    DateTime firstDayOfYear = new DateTime(int.Parse(year), 1, 1);
                    DateTime targetDate = firstDayOfYear.AddDays(int.Parse(day) - 1);
                    int month = targetDate.Month;
                    int days = targetDate.Day;
                    dt = new DateTime(int.Parse(year), month, days);

                    //Check so correct values are in then start the simulation
                    double value;
                    if (double.TryParse(setLat.Text.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out value)
                        && double.TryParse(setLong.Text.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out value)
                        && double.TryParse(setJammerLat.Text.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out value)
                        && double.TryParse(setJammerLong.Text.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out value))
                    {
                        sc = new SimulationController(setGps.Checked, setGalileo.Checked, setGlonass.Checked, dt, fileName,
                            double.Parse(setLat.Text.Replace(',', '.'), CultureInfo.InvariantCulture),
                            double.Parse(setLong.Text.Replace(',', '.'), CultureInfo.InvariantCulture),
                            setIntOn.Checked,
                            double.Parse(setRadR.Value.ToString()),
                            double.Parse(setJammerLat.Text.Replace(',', '.'), CultureInfo.InvariantCulture),
                            double.Parse(setJammerLong.Text.Replace(',', '.'), CultureInfo.InvariantCulture));

                        timerRunTime.Start();
                        Terminal.ForeColor = Color.White;
                        Terminal.Text += "Simulation Started: " + dt + Environment.NewLine;
                        Terminal.SelectionStart = Terminal.Text.Length;
                        Terminal.ScrollToCaret();
                    }
                    else
                    {
                        Terminal.ForeColor = Color.Red;
                        Terminal.Text += "# Invalid inputs (Check if you use , or . in Coordinates) #" + Environment.NewLine;
                        Terminal.SelectionStart = Terminal.Text.Length;
                        Terminal.ScrollToCaret();
                        
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            else
            {
                Terminal.ForeColor = Color.Red;
                Terminal.Text += "# Stop the Simulation before pressing Simulate again #" + Environment.NewLine;
                Terminal.SelectionStart = Terminal.Text.Length;
                Terminal.ScrollToCaret();
                
            }
        }

        public void HandleRunTime(object sender, EventArgs e)
        {
            //When event happen in this file run SimulationRunTime once
            srt.RunSimulation(sc);
        }

        public void HandleTickEvent(object sender, EventArgs e)
        {
            //When event happen at the end of RunTime write to NMEA if its on
            if (setNMEA.Checked) {
                NmeaStringsGenerator.NmeaGenerator(serialPort, sc);
            }
        }

        private void setRadR_Scroll(object sender, EventArgs e)
        {
            //set the label when Jammmer str slider is changed
            labelRadR.Text = setRadR.Value.ToString() + " km";
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            //When Stop is pressed once stop the simulation and change the name to Clear, 2nd time clear the Terminal
            Stop.Text = "Clear";
            Terminal.ForeColor = Color.White;
            Terminal.Text += "Simulation Stopped" + Environment.NewLine;
            Terminal.SelectionStart = Terminal.Text.Length;
            Terminal.ScrollToCaret();
            timerRunTime.Stop();
            running = false;

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
            //When you update the Position of Reciver check so Simulation is on and that values are correct
            if (sc != null)
            {
                double value;
                if (double.TryParse(setLat.Text.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out value)
                  && double.TryParse(setLong.Text.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out value)) { 

                    sc.UpdatePos(double.Parse(setLong.Text.Replace(',', '.'), CultureInfo.InvariantCulture), double.Parse(setLong.Text.Replace(',', '.'), CultureInfo.InvariantCulture));
                    Terminal.ForeColor = Color.White;
                    Terminal.Text += "New Position Value Set: " + setLat.Text + " " + setLong.Text + Environment.NewLine;
                    Terminal.SelectionStart = Terminal.Text.Length;
                    Terminal.ScrollToCaret();
                }
                else
                {
                    Terminal.ForeColor = Color.Red;
                    Terminal.Text += "# Invalid inputs (Check if you use , or . in Coordinates) #" + Environment.NewLine;
                    Terminal.SelectionStart = Terminal.Text.Length;
                    Terminal.ScrollToCaret();
                }
            }
            
        }

        private void updateJammerPos_Click(object sender, EventArgs e)
        {
            //When you update the Position of Jammer check so Simulation is on and that values are correct
            if (sc != null)
            {
                double value;
                if (double.TryParse(setJammerLat.Text.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out value)
                  && double.TryParse(setJammerLong.Text.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out value)) { 

                    sc.UpdateJammerPos(setIntOn.Checked, double.Parse(setJammerLat.Text.Replace(',', '.'), CultureInfo.InvariantCulture), double.Parse(setJammerLong.Text.Replace(',', '.'), CultureInfo.InvariantCulture), double.Parse(setRadR.Value.ToString()));
                    Terminal.ForeColor = Color.White;
                    Terminal.Text += "New Jammer Value Set: " + setJammerLat.Text + " " + setJammerLong.Text + Environment.NewLine;
                    Terminal.SelectionStart = Terminal.Text.Length;
                    Terminal.ScrollToCaret();
                }
                else
                {
                    Terminal.ForeColor = Color.Red;
                    Terminal.Text += "# Invalid inputs (Check if you use , or . in Coordinates) #" + Environment.NewLine;
                    Terminal.SelectionStart = Terminal.Text.Length;
                    Terminal.ScrollToCaret();
                }
            }
             

        }

    }
}