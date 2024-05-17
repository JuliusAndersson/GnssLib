using GeoTiffElevationReader;
using GnssLibCALC;
using GnssLibCALC.Models.Configuration;
using GnssLibDL;
using GnssLibNMEA_Writer;
using MightyLittleGeodesy.Positions;
using System.Globalization;
using System.IO.Ports;


namespace GnssLibGUI
{
    public partial class GUI_Window : Form
    {

        private SimulationController? _simulationController;
        private SimulationRunTime _simulationRunTIme;
        private bool _hasStopped = true;
        private SerialPort _serialPort;
        private List<string> _fileList = new List<string>();
        private bool _isRunning = false;
        private bool _isNmeaOn = false;
        private bool _hasRunnedOnce = false;
        private string _geoFilePath;
        private string _geoFolderPath;
        private AltitudeChecker _altitudeChecker;

        /// <summary>
        /// 
        /// </summary>
        public GUI_Window()
        {
            InitializeComponent();
            _serialPort = new SerialPort("COM1", 4800, Parity.None, 8, StopBits.One);
            _serialPort.Handshake = Handshake.None;

            //Check what files exist and creates a Dropdown
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources/Broadcast");
            if (Directory.Exists(folderPath))
            {
                string[] fileNames = Directory.GetFiles(folderPath);


                foreach (string filePath in fileNames)
                {
                    _fileList.Add(Path.GetFileName(filePath));
                    int dayIndexs = Path.GetFileName(filePath).IndexOf('_') + 7;
                    string days = Path.GetFileName(filePath).Substring(dayIndexs, 3);
                    int yearIndexs = Path.GetFileName(filePath).IndexOf('_') + 3;
                    string years = Path.GetFileName(filePath).Substring(yearIndexs, 4);
                    setFile.Items.Add("Day: " + days + " Year: " + years);
                }
            }
            else
            {
                Terminal.Text += "File does not exist.";
            }


            setFile.SelectedIndex = 0;



            //subscribe to event in RunTime
            _simulationRunTIme = new SimulationRunTime();
            _simulationRunTIme.tickDone += HandleTickEvent;

            Terminal.ForeColor = Color.Green;
            Terminal.Text += "Enter VALUES, then PRESS 'Simulate' to start the simulation." + Environment.NewLine;

            _geoFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources/ElevationMaps/62_3_2023.tif");
            _geoFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources/NasaElevationMaps/");

            _altitudeChecker = new AltitudeChecker(_geoFolderPath);

        }

        /// <summary>
        /// When Simulate button is pressed SimulationController is created and the SimulationRunTime event is started.
        /// </summary>
        private void Simulate_Click(object sender, EventArgs e)
        {
            if (!_isRunning)
            {
                try
                {

                    if (setNMEA.Checked)
                    {
                        _serialPort.Open();
                        _isNmeaOn = true;
                    }


                    _hasRunnedOnce = true;
                    _isRunning = true;
                    _hasStopped = false;
                    DateTime dt;
                    Stop.Text = "Stop";


                    //Get DateTime from file
                    string fileName = _fileList[setFile.SelectedIndex];
                    int dayIndex = fileName.IndexOf('_') + 7;
                    string day = fileName.Substring(dayIndex, 3);
                    int yearIndex = fileName.IndexOf('_') + 3;
                    string year = fileName.Substring(yearIndex, 4);
                    DateTime firstDayOfYear = new DateTime(int.Parse(year), 1, 1);
                    DateTime targetDate = firstDayOfYear.AddDays(int.Parse(day) - 1);
                    int month = targetDate.Month;
                    int days = targetDate.Day;
                    dt = new DateTime(int.Parse(year), month, days, int.Parse(setHour.Text), int.Parse(setMinute.Text), int.Parse(setSecond.Text));

                    //Check so correct values are in then start the simulation
                    double value;
                    if (double.TryParse(setLat.Text.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out value)
                        && double.TryParse(setLong.Text.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out value)
                        && double.TryParse(setJammerLat.Text.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out value)
                        && double.TryParse(setJammerLong.Text.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out value)
                        && double.Parse(setLat.Text.Replace(',', '.'), CultureInfo.InvariantCulture) > -90
                        && double.Parse(setLat.Text.Replace(',', '.'), CultureInfo.InvariantCulture) < 90
                        && double.Parse(setLong.Text.Replace(',', '.'), CultureInfo.InvariantCulture) > -180
                        && double.Parse(setLong.Text.Replace(',', '.'), CultureInfo.InvariantCulture) < 180)
                    {

                        ReceiverConfiguration receiverConfig = new ReceiverConfiguration()
                        {
                            IsUsingGPS = setGps.Checked,
                            IsUsingGalileo = setGalileo.Checked,
                            IsUsingGlonass = setGlonass.Checked,
                            ReceiverLatitude = double.Parse(setLat.Text.Replace(',', '.'), CultureInfo.InvariantCulture),
                            ReceiverLongitude = double.Parse(setLong.Text.Replace(',', '.'), CultureInfo.InvariantCulture),
                            ReceiverAltitude = _altitudeChecker.GetAltitudeAtCoordinates(double.Parse(setLat.Text.Replace(',', '.'), CultureInfo.InvariantCulture), double.Parse(setLong.Text.Replace(',', '.'), CultureInfo.InvariantCulture)),
                            ReceiverGpsError = 3
                        };

                        JammerConfiguration jammerConfig = new JammerConfiguration()
                        {
                            IsJammerOn = setIntOn.Checked,
                            JammerRange = double.Parse(setRadR.Value.ToString()),
                            JammerLatitude = double.Parse(setJammerLat.Text.Replace(',', '.'), CultureInfo.InvariantCulture),
                            JammerLongitude = double.Parse(setJammerLong.Text.Replace(',', '.'), CultureInfo.InvariantCulture),
                        };


                        _simulationController = new SimulationController(receiverConfig, jammerConfig, fileName, dt, DomeMode.Checked);

                        _simulationRunTIme.RunSimulation(_simulationController);
                        Terminal.ForeColor = Color.White;
                        Terminal.Text += "Simulation Started: " + dt + Environment.NewLine;
                        Terminal.SelectionStart = Terminal.Text.Length;
                        Terminal.ScrollToCaret();
                    }
                    else
                    {
                        Terminal.ForeColor = Color.Red;
                        Terminal.Text += "# Invalid inputs (Check if you use , or . in Coordinates) ( Lat: -90 -> 90 ) ( Long: -180 -> 180 ) #" + Environment.NewLine;
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

        /// <summary>
        /// When event happen at the end of RunTime write to NMEA if its on and update Position accuracy.
        /// </summary>
        public void HandleTickEvent(object sender, EventArgs e)
        {
            //Test values from Controller
            //Terminal.Text += _simulationController.DebugToTerminalGUIGetValues() + Environment.NewLine;
            //Terminal.SelectionStart = Terminal.Text.Length;
            //Terminal.ScrollToCaret();

            //Terminal.Text += _simulationController.DebugToTerminalGUIGetValuesDouble() + Environment.NewLine;
            //Terminal.SelectionStart = Terminal.Text.Length;
            //Terminal.ScrollToCaret();

            //foreach (var val in _simulationController.DebugToTerminalGUIGetValuesBool())
            //{
            //    Terminal.Text += val + Environment.NewLine;
            //    Terminal.SelectionStart = Terminal.Text.Length;
            //    Terminal.ScrollToCaret();
            //}

            if (_simulationController.GetApproxPos() > 0)
            {
                labelPosAcc.Text = _simulationController.GetApproxPos().ToString("F1") + " m";
            }
            else
            {
                labelPosAcc.Text = "-,- m";
            }

            if (_isNmeaOn)
            {
                if (_simulationController.IsNoSatVisible())
                {
                    NmeaStringsGenerator.ClearGPSView();
                }
                else
                {
                    NmeaStringsGenerator.NmeaGenerator(_serialPort, _simulationController);
                }
            }
        }

        /// <summary>
        /// Set the label when Jammmer str slider is changed.
        /// </summary>
        private void SetRadR_Scroll(object sender, EventArgs e)
        {
            double radInt = 0;
            if (DomeMode.Checked)
            {
                radInt = setRadR.Value;
                labelRadR.Text = ((int)radInt).ToString() + " km";
            }
            else
            {
                double alts = _altitudeChecker.GetAltitudeAtCoordinates(double.Parse(setLat.Text.Replace(',', '.'), CultureInfo.InvariantCulture), double.Parse(setLong.Text.Replace(',', '.'), CultureInfo.InvariantCulture));
                radInt = InterferenceCalculator.LineOfSightCalculation(alts, setRadR.Value);

                jamRadBox.Text = "Jammer Height     (Approx Range: " + (int)radInt + " km)";
                labelRadR.Text = setRadR.Value + " m";
            }

        }

        /// <summary>
        /// When Stop is pressed once stop the simulation and change the name to Clear, 2nd time clear the Terminal.
        /// </summary>
        private void Stop_Click(object sender, EventArgs e)
        {
            Stop.Text = "Clear";
            Terminal.ForeColor = Color.White;
            Terminal.Text += "Simulation Stopped" + Environment.NewLine;
            labelPosAcc.Text = "-,- m";
            Terminal.SelectionStart = Terminal.Text.Length;
            Terminal.ScrollToCaret();
            _isRunning = false;
            _isNmeaOn = false;

            if (_hasRunnedOnce)
            {
                _simulationRunTIme.StopSimulation();
            }


            if (_hasStopped)
            {
                Terminal.Clear();

                if (_hasRunnedOnce)
                {
                    _serialPort.Open();
                    NmeaStringsGenerator.ClearGPSView();
                    _serialPort.Close();
                }
            }
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
            }
            _hasStopped = true;

        }

        /// <summary>
        /// When you update the Position of Reciver check so Simulation is on and that values are correct.
        /// </summary>
        private void UpdatePos_Click(object sender, EventArgs e)
        {
            if (_simulationController != null)
            {
                double value;
                if (double.TryParse(setLat.Text.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out value)
                  && double.TryParse(setLong.Text.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out value)
                  && double.Parse(setLat.Text.Replace(',', '.'), CultureInfo.InvariantCulture) > -90
                  && double.Parse(setLat.Text.Replace(',', '.'), CultureInfo.InvariantCulture) < 90
                  && double.Parse(setLong.Text.Replace(',', '.'), CultureInfo.InvariantCulture) > -180
                  && double.Parse(setLong.Text.Replace(',', '.'), CultureInfo.InvariantCulture) < 180)
                {

                    _simulationController.UpdatePos(double.Parse(setLat.Text.Replace(',', '.'), CultureInfo.InvariantCulture), double.Parse(setLong.Text.Replace(',', '.'), CultureInfo.InvariantCulture),
                       _altitudeChecker.GetAltitudeAtCoordinates(double.Parse(setLat.Text.Replace(',', '.'), CultureInfo.InvariantCulture), double.Parse(setLong.Text.Replace(',', '.'), CultureInfo.InvariantCulture)));
                    Terminal.ForeColor = Color.White;
                    Terminal.Text += "New Position Value Set:  Lat: " + setLat.Text + " Long: " + setLong.Text + Environment.NewLine;
                    Terminal.SelectionStart = Terminal.Text.Length;
                    Terminal.ScrollToCaret();
                }
                else
                {
                    Terminal.ForeColor = Color.Red;
                    Terminal.Text += "# Invalid inputs (Check if you use , or . in Coordinates) ( Lat: -90 -> 90 ) ( Long: -180 -> 180 ) #" + Environment.NewLine;
                    Terminal.SelectionStart = Terminal.Text.Length;
                    Terminal.ScrollToCaret();
                }
            }
        }

        /// <summary>
        /// When you update the Position of Jammer check so Simulation is on and that values are correct.
        /// </summary>
        private void UpdateJammerPos_Click(object sender, EventArgs e)
        {
            if (_simulationController != null)
            {
                double value;
                if (double.TryParse(setJammerLat.Text.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out value)
                  && double.TryParse(setJammerLong.Text.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out value)
                  && double.Parse(setJammerLat.Text.Replace(',', '.'), CultureInfo.InvariantCulture) > -90
                  && double.Parse(setJammerLat.Text.Replace(',', '.'), CultureInfo.InvariantCulture) < 90
                  && double.Parse(setJammerLong.Text.Replace(',', '.'), CultureInfo.InvariantCulture) > -180
                  && double.Parse(setJammerLong.Text.Replace(',', '.'), CultureInfo.InvariantCulture) < 180)
                {

                    _simulationController.UpdateJammerPos(setIntOn.Checked, double.Parse(setJammerLat.Text.Replace(',', '.'), CultureInfo.InvariantCulture),
                        double.Parse(setJammerLong.Text.Replace(',', '.'), CultureInfo.InvariantCulture), double.Parse(setRadR.Value.ToString()),
                        DomeMode.Checked);

                    Terminal.ForeColor = Color.White;
                    Terminal.Text += "New Jammer Value Set:  Lat: " + setJammerLat.Text + " Long: " + setJammerLong.Text + Environment.NewLine;
                    Terminal.SelectionStart = Terminal.Text.Length;
                    Terminal.ScrollToCaret();
                }
                else
                {
                    Terminal.ForeColor = Color.Red;
                    Terminal.Text += "# Invalid inputs (Check if you use , or . in Coordinates) ( Lat: -90 -> 90 ) ( Long: -180 -> 180 ) #" + Environment.NewLine;
                    Terminal.SelectionStart = Terminal.Text.Length;
                    Terminal.ScrollToCaret();
                }
            }


        }

        private void DomeMode_CheckedChanged(object sender, EventArgs e)
        {
            if (DomeMode.Checked == true)
            {
                setRadR.Maximum = 500;
                setRadR.TickFrequency = 10;
                labelRadR.Text = setRadR.Value + " km";
                jamBox.Text = "Interference Dome Mode";
                jamRadBox.Text = "Jammer Radius";
            }
            else
            {
                if (setRadR.Value > 200)
                {
                    labelRadR.Text = "200" + " m";
                }
                setRadR.Maximum = 200;
                setRadR.TickFrequency = 5;
                jamBox.Text = "Interference";
                jamRadBox.Text = "Jammer Height     (Approx Range: " + "~" + " km)";
            }
        }
    }
}