using GnssLibCALC.Models;
using GnssLibCALC.Models.SatModels;
using GnssLibCALC;
using GnssLibCALC.Models.Configuration;

namespace GnssLibDL
{
    public class SimulationController
    {

        private readonly double _speedForSimulation = 1;
        private static readonly double _MIN_ELEVATION = 0;
        private static readonly double _MAX_ELEVATION = 90;

        private GNSS _GNSS_Data;
        private bool _onDomeMode = false;
        private bool _isJammed = false;

        //Input & Output
        private ReceiverConfiguration _rConfig;
        private JammerConfiguration _jConfig;
        private DateTime _simulationStartDateTime;

        //Output
        private List<double[]> _satellitePositions;
        private List<string> _visibleSatellitesPRN_GPS;
        private List<string> _visibleSatellitesPRN_GL;
        private List<SatelliteElevationAndAzimuthInfo> _satListGPS;
        private List<SatelliteElevationAndAzimuthInfo> _satListGL;

        private double _PDOP;
        private double _HDOP;
        private double _VDOP;
        private double[] _receiverPos;
        private double[] _jammerPos;
        private double _continousSecFromStart = 0;

        public bool isJammed { get { return _isJammed; } }
        public ReceiverConfiguration rConfig { get { return _rConfig; } }
        public DateTime simulationStartDateTime { get { return _simulationStartDateTime; } }
        public List<string> visibleSatellitesPRN_GPS { get { return _visibleSatellitesPRN_GPS; } }
        public List<string> visibleSatellitesPRN_GL { get { return _visibleSatellitesPRN_GL; } }
        public List<SatelliteElevationAndAzimuthInfo> satListGPS { get { return _satListGPS; } }
        public List<SatelliteElevationAndAzimuthInfo> satListGL { get { return _satListGL; } }
        public double PDOP { get { return _PDOP; } }
        public double HDOP { get { return _HDOP; } }
        public double VDOP { get { return _VDOP; } }
        public double continousSecFromStart { get { return _continousSecFromStart; } }



        /// <summary>
        /// The init sets global var and reads the Epemeris file.
        /// </summary>
        /// <param name="rConfig"> Configurations for the reciever. </param>
        /// <param name="jConfig"> Configurations for the jammer. </param>
        /// <param name="fileName"> The name of the epemeris file to be used.</param>
        /// <param name="simulationStartDateTime"> The DateTime that the simulation starts at.</param>
        public SimulationController(ReceiverConfiguration rConfig, JammerConfiguration jConfig, string fileName, DateTime simulationStartDateTime, 
            bool domeMode)
        {
            this._rConfig = rConfig;
            this._jConfig = jConfig;
            this._simulationStartDateTime = simulationStartDateTime;
            this._onDomeMode = domeMode;
            //Read File and put in a Satellites variable
            string filePath = $"Resources/Broadcast/{fileName}";
            BroadCastDataReader bcdr = new BroadCastDataReader();
            _GNSS_Data = bcdr.ReadBroadcastData(filePath);

        }

        /// <summary>
        /// The Controller of the program, calls for CALC to calculate and update global var.
        /// </summary>
        public void Tick()
        {

            //Reset Lists for new values at the new time
            _visibleSatellitesPRN_GPS = new List<string>();
            _satListGPS = new List<SatelliteElevationAndAzimuthInfo>();
            _visibleSatellitesPRN_GL = new List<string>();
            _satListGL = new List<SatelliteElevationAndAzimuthInfo>();

            _satellitePositions = new List<double[]>();
            _receiverPos = CoordinatesCalculator.GeodeticToECEF(_rConfig.ReceiverLatitude, _rConfig.ReceiverLongitude, _rConfig.ReceiverAltitude);
            _jammerPos = CoordinatesCalculator.GeodeticToECEF(_jConfig.JammerLatitude, _jConfig.JammerLongitude, 0);

            if (!_onDomeMode && _jConfig.IsJammerOn)
            {
                if(DOPCalulator.CalculateDistance(_receiverPos, _jammerPos) < InterferenceCalculator.LineOfSightCalculation(_rConfig.ReceiverAltitude, _jConfig.JammerRange))
                {
                    _isJammed = true;
                }
                else
                {
                    _isJammed = false;
                }

            }

            //Check if GPS, Galileo, Glonass is to be Used
            if (!_isJammed || _onDomeMode || !_jConfig.IsJammerOn)
            {
                if (_rConfig.IsUsingGPS)
                {
                    MakeGps();
                }
                if (_rConfig.IsUsingGalileo)
                {
                    MakeGalileo();
                }
            }

            // Calculate DOP for Visible Satellites
            DOPCalulator.CalculateDOP(_receiverPos, _satellitePositions, out double PDOP, out double HDOP, out double VDOP);
            this._PDOP = PDOP;
            this._HDOP = HDOP;
            this._VDOP = VDOP;
            _continousSecFromStart += _speedForSimulation;
        }

        /// <summary>
        /// If gps is on, Calculate and update lists of satellites with gps.
        /// </summary>
        private void MakeGps()
        {

            var currentDatetime = _simulationStartDateTime.AddSeconds(_continousSecFromStart);
            
            foreach (var gps in _GNSS_Data.Gps.satList)
            {
               
                var lastBroadcast = gps.Data.LastOrDefault(b => b.DateTime <= currentDatetime);

                if (lastBroadcast != null)
                {
                    TimeSpan difference = currentDatetime - lastBroadcast.DateTime;
                    double timeDiffSec = difference.TotalSeconds;


                    //Check if a Satellite is visible
                    double[] satPosXYZ = CoordinatesCalculator.CalculatePosition(lastBroadcast, timeDiffSec);

                    VisibleSatCalulator.IsSatelliteVisible(_MIN_ELEVATION, _MAX_ELEVATION, _receiverPos, satPosXYZ, out bool isVisible, out double elevation, out double azimuth);
                    if (isVisible)
                    {
                        //Check if a Satellite is blocked by the jammer
                        if (!(InterferenceCalculator.DoesLineSegmentIntersectSphere(satPosXYZ, _receiverPos, _jammerPos, _jConfig.JammerRange)) || 
                            !_jConfig.IsJammerOn || !_onDomeMode)
                        {
                            _satellitePositions.Add(satPosXYZ);
                            _visibleSatellitesPRN_GPS.Add(lastBroadcast.SatId);
                            _satListGPS.Add(new SatelliteElevationAndAzimuthInfo()
                            {
                                SatId = lastBroadcast.SatId.Substring(1),
                                Azimuth = Math.Round(azimuth, 0),
                                Elevation = Math.Round(elevation, 0)
                            });
                        }
                    }

                }
            }
        }

        /// <summary>
        /// If galileo is on, Calculate and update lists of satellites with galileo.
        /// </summary>
        private void MakeGalileo()
        {
            var currentDatetime = _simulationStartDateTime.AddSeconds(_continousSecFromStart);

            foreach (var galileo in _GNSS_Data.Galileo.ListOfSatellites)
            {

                var lastBroadcast = galileo.Data.LastOrDefault(b => b.DateTime <= currentDatetime);

                if (lastBroadcast != null)
                {
                    TimeSpan difference = currentDatetime - lastBroadcast.DateTime;
                    double timeDiffSec = difference.TotalSeconds;


                    //Check if a Satellite is visible
                    double[] satPosXYZ = CoordinatesCalculator.CalculatePosition(lastBroadcast, timeDiffSec);
                    VisibleSatCalulator.IsSatelliteVisible(_MIN_ELEVATION, _MAX_ELEVATION, _receiverPos, satPosXYZ, out bool isVisible,
                        out double elevation, out double azimuth);
                    if (isVisible)
                    {
                        //Check if a Satellite is blocked by the jammer
                        if (!(InterferenceCalculator.DoesLineSegmentIntersectSphere(satPosXYZ, _receiverPos, _jammerPos, _jConfig.JammerRange)) || 
                            !_jConfig.IsJammerOn || !_onDomeMode)
                        {

                            _satellitePositions.Add(satPosXYZ);
                            _visibleSatellitesPRN_GL.Add(lastBroadcast.SatId);
                            _satListGL.Add(new SatelliteElevationAndAzimuthInfo()
                            {
                                SatId = lastBroadcast.SatId.Substring(1),
                                Azimuth = Math.Round(azimuth, 0),
                                Elevation = Math.Round(elevation, 0)
                            });
                        }
                    }
                }


            }
        }
        /// <summary>
        /// Used to update the receiver position.
        /// </summary>
        public void UpdatePos(double latPos, double longPos, double elevation)
        {
            this._rConfig.ReceiverLatitude = latPos;
            this._rConfig.ReceiverLongitude = longPos;
            this._rConfig.ReceiverAltitude = elevation;
        }
        /// <summary>
        /// Used to update the jammer position.
        /// </summary>
        public void UpdateJammerPos(bool jamOn, double jamLat, double jamLong, double jamRad, bool domeMode)
        {
            this._jConfig.IsJammerOn = jamOn;
            this._jConfig.JammerLatitude = jamLat;
            this._jConfig.JammerLongitude = jamLong;
            this._jConfig.JammerRange = jamRad;
            this._onDomeMode = domeMode;
        }

        /// <summary>
        /// Calculates position accuracy with PDOP and gps error.
        /// </summary>
        public double GetApproxPos()
        {
            return _PDOP * _rConfig.ReceiverGpsError;
        }

        public bool IsNoSatVisible()
        {
            if (visibleSatellitesPRN_GL.Count + visibleSatellitesPRN_GPS.Count < 1) return true;
            return false;
        }

        /// <summary>
        /// 3 methods used to write to the Terminal on the GUI. (DELETE! NOT USED IN FINISHED PRODUCT)
        /// </summary>
        public DateTime DebugToTerminalGUIGetValues()
        {
            return new DateTime(_simulationStartDateTime.Year, _simulationStartDateTime.Month, _simulationStartDateTime.Day, 00, 00, 00);
        }

        public string DebugToTerminalGUIGetValuesDouble()
        {
            return "";
        }

        public bool DebugToTerminalGUIGetValuesBool()
        {
            if(visibleSatellitesPRN_GL.Count+visibleSatellitesPRN_GPS.Count < 1) return true;
            return false;
        }

    }
}
