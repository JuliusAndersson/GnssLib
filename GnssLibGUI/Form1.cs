using GeoTiffElevationReader;
using GnssLibCALC;
using GnssLibCALC.Models.BroadCastDataModels;
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
        System.Windows.Forms.Timer _timerRunTime = new System.Windows.Forms.Timer();
        private SimulationController? _sc;
        private SimulationRunTime _srt;
        private bool _hasStopped = true;
        private SerialPort _serialPort;
        private List<String> _fileList = new List<string>();
        private bool _isRunning = false;
        private bool _isNmeaOn = false;

        private double _elevation;
        private string _geoFilePath;
        public GUI_Window()
        {
            InitializeComponent();
            _serialPort = new SerialPort("COM1", 4800, Parity.None, 8, StopBits.One);
            _serialPort.Handshake = Handshake.None;

            //Check what files exist
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

            //Event that repeats ever 1s
            _timerRunTime.Tick += HandleRunTime;
            _timerRunTime.Interval = 1000;

            //subscribe to event in RunTime
            _srt = new SimulationRunTime();
            _srt._tickDone += HandleTickEvent;

            Terminal.ForeColor = Color.Green;
            Terminal.Text += "Enter VALUES, then PRESS 'Simulate' to start the simulation." + Environment.NewLine;

            _geoFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources/ElevationMaps/62_3_2023.tif");
           

        }

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

                        ReceiverConfiguration rConfig = new ReceiverConfiguration()
                        {
                            IsUsingGPS = setGps.Checked,
                            IsUsingGalileo = setGalileo.Checked,
                            IsUsingGlonass = setGlonass.Checked,
                            ReceiverDT = dt,
                            ReceiverLatitude = double.Parse(setLat.Text.Replace(',', '.'), CultureInfo.InvariantCulture),
                            ReceiverLongitude = double.Parse(setLong.Text.Replace(',', '.'), CultureInfo.InvariantCulture),
                            ReceiverElevetion = initElevation(double.Parse(setLat.Text.Replace(',', '.'), CultureInfo.InvariantCulture), double.Parse(setLong.Text.Replace(',', '.'), CultureInfo.InvariantCulture))
                        };

                        JammerConfiguration jConfig = new JammerConfiguration()
                        {
                            IsJammerOn = setIntOn.Checked,
                            JammerRadius = double.Parse(setRadR.Value.ToString()),
                            JammerLatitude = double.Parse(setJammerLat.Text.Replace(',', '.'), CultureInfo.InvariantCulture),
                            JammerLongitude = double.Parse(setJammerLong.Text.Replace(',', '.'), CultureInfo.InvariantCulture),
                        };


                            _sc = new SimulationController(rConfig, jConfig, fileName);

                        _timerRunTime.Start();
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

        public void HandleRunTime(object sender, EventArgs e)
        {
            //When event happen in this file run SimulationRunTime once
            _srt.RunSimulation(_sc);
        }

        public void HandleTickEvent(object sender, EventArgs e)
        {
            //Test values from Controller
            Terminal.Text += _sc.GetValues() + Environment.NewLine;
            Terminal.SelectionStart = Terminal.Text.Length;
            Terminal.ScrollToCaret();

            if (_sc.GetApproxPos() > 0)
            {
                labelPosAcc.Text = _sc.GetApproxPos().ToString("F1") + " m";
            }
            else
            {
                labelPosAcc.Text = "-,- m";
            }
            //When event happen at the end of RunTime write to NMEA if its on
            if (_isNmeaOn) {
                NmeaStringsGenerator.NmeaGenerator(_serialPort, _sc);
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
            labelPosAcc.Text = "-,- m";
            Terminal.SelectionStart = Terminal.Text.Length;
            Terminal.ScrollToCaret();
            _timerRunTime.Stop();
            _isRunning = false;
            _isNmeaOn = false;

            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
            }

            if (_hasStopped)
            {
                Terminal.Clear();
            }
            _hasStopped = true;

        }

        private void updatePos_Click(object sender, EventArgs e)
        {
            //When you update the Position of Reciver check so Simulation is on and that values are correct
            if (_sc != null)
            {
                double value;
                if (double.TryParse(setLat.Text.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out value)
                  && double.TryParse(setLong.Text.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out value)) { 

                    _sc.UpdatePos(double.Parse(setLat.Text.Replace(',', '.'), CultureInfo.InvariantCulture), double.Parse(setLong.Text.Replace(',', '.'), CultureInfo.InvariantCulture),
                       initElevation(double.Parse(setLat.Text.Replace(',', '.'), CultureInfo.InvariantCulture), double.Parse(setLong.Text.Replace(',', '.'), CultureInfo.InvariantCulture)));
                    Terminal.ForeColor = Color.White;
                    Terminal.Text += "New Position Value Set:  Lat: " + setLat.Text + " Long: " + setLong.Text + Environment.NewLine;
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
            if (_sc != null)
            {
                double value;
                if (double.TryParse(setJammerLat.Text.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out value)
                  && double.TryParse(setJammerLong.Text.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out value)) { 

                    _sc.UpdateJammerPos(setIntOn.Checked, double.Parse(setJammerLat.Text.Replace(',', '.'), CultureInfo.InvariantCulture), 
                        double.Parse(setJammerLong.Text.Replace(',', '.'), CultureInfo.InvariantCulture), double.Parse(setRadR.Value.ToString()));

                    Terminal.ForeColor = Color.White;
                    Terminal.Text += "New Jammer Value Set:  Lat: " + setJammerLat.Text + " Long: " + setJammerLong.Text + Environment.NewLine;
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
        private double initElevation(double latitude, double longitude)
        {
            WGS84Position wgsPos = new WGS84Position();
            wgsPos.SetLatitudeFromString(CoordinatesCalculator.DoubleToDegreesMinutesSeconds(latitude, true), WGS84Position.WGS84Format.DegreesMinutesSeconds);
            wgsPos.SetLongitudeFromString(CoordinatesCalculator.DoubleToDegreesMinutesSeconds(longitude, false), WGS84Position.WGS84Format.DegreesMinutesSeconds);
            SWEREF99Position rtPos = new SWEREF99Position(wgsPos, SWEREF99Position.SWEREFProjection.sweref_99_tm);
            double elevation = 0;
            if (rtPos.Latitude > 6200000 && rtPos.Latitude < 6300000 && rtPos.Longitude < 400000 && rtPos.Longitude > 300000)
            {
                if (File.Exists(_geoFilePath))
                {
                    GeoTiff elevationtiff = new GeoTiff(_geoFilePath);
                    elevation = elevationtiff.GetElevationAtLatLon(rtPos.Latitude, rtPos.Longitude);
                }
            }
            return elevation;
        }

    }
}