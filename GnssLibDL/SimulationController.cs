using GnssLibCALC.Models;
using GnssLibCALC.Models.SatModels;
using GnssLibCALC.Models.BroadCastDataModels;
using System.IO.Ports;
using GnssLibCALC;
using System;
using GeoTiffElevationReader;
using MightyLittleGeodesy.Positions;
using GnssLibCALC.Models.Configuration;
using System.Reflection.Metadata.Ecma335;


namespace GnssLibDL
{
    public class SimulationController
    {

        private readonly double _speedForSimulation = 600;
        private static readonly double _MIN_ELEVATION = 0;
        private static readonly double _MAX_ELEVATION = 90;

        private double _continousSecGPS = 0;
        private double _continousSecGAL = 0;
        
        private int _boxHourGPS;
        private int _boxHourGAL;
        private int _boxMin;

        private Satellites _GNSS_Data;
        private string _geoFilePath;


        //Input & Output
        public ReceiverConfiguration _rConfig;
        public JammerConfiguration _jConfig;


        //Output
        private List<double[]> _satellitePositions;

        public List<string> _visibleSatellitesPRN_GPS;
        public List<string> _visibleSatellitesPRN_GL;
        public List<SatelliteElevationAndAzimuthInfo> _satListGPS;
        public List<SatelliteElevationAndAzimuthInfo> _satListGL;
        
        public double _PDOP;
        public double _HDOP;
        public double _VDOP;
        public double[] _receiverPos;
        
        public double _continousSecFromStart = 0;

        public SimulationController( ReceiverConfiguration rConfig, JammerConfiguration jConfig, String fileName) {
            this._rConfig = rConfig;
            this._jConfig = jConfig;

            _boxHourGPS = rConfig.ReceiverStartDT.Hour - (rConfig.ReceiverStartDT.Hour % 2);
            if(rConfig.ReceiverStartDT.Hour % 2 == 1)
            {
                _continousSecGPS = 3600;
            }
            _boxMin = rConfig.ReceiverStartDT.Minute - (rConfig.ReceiverStartDT.Minute % 10);
            if(rConfig.ReceiverStartDT.Minute % 10 != 0)
            {
                _continousSecGAL = (rConfig.ReceiverStartDT.Minute % 10) * 60;
            }

            //Read File and put in a Satellites variable
            string filePath = $"Resources/Broadcast/{fileName}"; 
            BroadCastDataReader bcdr = new BroadCastDataReader();
            _GNSS_Data = bcdr.ReadBroadcastData(filePath);

        }

        public void Tick()
        {

            //Reset Lists for new values at the new time
            _visibleSatellitesPRN_GPS = new List<string>();
            _satListGPS = new List<SatelliteElevationAndAzimuthInfo>();
            _visibleSatellitesPRN_GL = new List<string>();
            _satListGL = new List<SatelliteElevationAndAzimuthInfo>();

            _satellitePositions = new List<double[]>();
            _receiverPos = CoordinatesCalculator.GeodeticToECEF(_rConfig.ReceiverLatitude, _rConfig.ReceiverLongitude, _rConfig.ReceiverElevetion);
            
            

            //Check if GPS, Galileo, Glonass is to be Used
            if (_rConfig.IsUsingGPS)
            {
                MakeGps();
            }
            if (_rConfig.IsUsingGalileo)
            {
                MakeGalileo();
            }


            // Calculate DOP for Visible Satellites
            DOPCalulator.CalculateDOP(_receiverPos, _satellitePositions, out double PDOP, out double HDOP, out double VDOP);
            this._PDOP = PDOP;
            this._HDOP = HDOP;
            this._VDOP = VDOP;
            _continousSecGPS += _speedForSimulation;
            _continousSecGAL += _speedForSimulation;
            _continousSecFromStart += _speedForSimulation;
        }

        private void MakeGps()
        {
            foreach (var gps in _GNSS_Data.Gps.satList)
            {

                //Check if it's another 2h mark so the next messege Block should be used
                if (_rConfig.ReceiverStartDT.AddSeconds(_continousSecFromStart).Hour % 2 == 0 && _rConfig.ReceiverStartDT.AddSeconds(_continousSecGPS).Minute == 0 && _rConfig.ReceiverStartDT.AddSeconds(_continousSecGPS).Second == 0)
                {
                    _boxHourGPS = _rConfig.ReceiverStartDT.AddSeconds(_continousSecFromStart).Hour;
                    _continousSecGPS = 0;
                }

                foreach (var broadcast in gps.Data)
                {
                    //if (broadcast.DateTime > new DateTime(2024, 01, 01, boxHour-1, 55, 00) && broadcast.DateTime < new DateTime(2024, 01, 01, boxHour, 05, 00))
                    if (broadcast.DateTime == new DateTime(_rConfig.ReceiverStartDT.Year, _rConfig.ReceiverStartDT.Month, _rConfig.ReceiverStartDT.Day, _boxHourGPS, 00, 00))
                    {
                        //Check if a Satellite is visible
                        double[] satPosXYZ = CoordinatesCalculator.CalculatePosition(broadcast, _continousSecGPS);

                        VisibleSatCalulator.IsSatelliteVisible(_MIN_ELEVATION, _MAX_ELEVATION, _receiverPos, satPosXYZ, out bool isVisible, out double elevation, out double azimuth);
                        if (isVisible)
                        {
                            //Check if a Satellite is blocked by the jammer
                            if (!(InterferenceCalculator.DoesLineSegmentIntersectSphere(satPosXYZ, _receiverPos, CoordinatesCalculator.GeodeticToECEF(_jConfig.JammerLatitude, _jConfig.JammerLongitude, 0), _jConfig.JammerRadius)) || !_jConfig.IsJammerOn)
                            {
                                _satellitePositions.Add(satPosXYZ);
                                _visibleSatellitesPRN_GPS.Add(broadcast.SatId);
                                _satListGPS.Add(new SatelliteElevationAndAzimuthInfo()
                                {
                                    SatId = broadcast.SatId.Substring(1),
                                    Azimuth = Math.Round(azimuth, 0),
                                    Elevation = Math.Round(elevation, 0)
                                });
                            }
                        }
                    }
                }
            }
        }

        private void MakeGalileo()
        {
            foreach (var galileo in _GNSS_Data.Galileo.ListOfSatellites)
            {
                //Check if it's another 15min mark so the next messege Block should be used
                if (_rConfig.ReceiverStartDT.AddSeconds(_continousSecFromStart).Minute % 10 == 0 && _rConfig.ReceiverStartDT.AddSeconds(_continousSecFromStart).Second == 0)
                {
                    _boxHourGAL = _rConfig.ReceiverStartDT.AddSeconds(_continousSecFromStart).Hour;
                    _boxMin = _rConfig.ReceiverStartDT.AddSeconds(_continousSecFromStart).Minute;
                    _continousSecGAL = 0;
                }

                foreach (var broadcast in galileo.Data)
                {

                    //if (broadcast.DateTime > new DateTime(2024, 01, 01, boxHour-1, 55, 00) && broadcast.DateTime < new DateTime(2024, 01, 01, boxHour, 05, 00))
                    if (broadcast.DateTime == new DateTime(_rConfig.ReceiverStartDT.Year, _rConfig.ReceiverStartDT.Month, _rConfig.ReceiverStartDT.Day, _boxHourGAL, _boxMin, 00))
                    {

                        //Check if a Satellite is visible
                        double[] satPosXYZ = CoordinatesCalculator.CalculatePosition(broadcast, _continousSecGAL);
                        VisibleSatCalulator.IsSatelliteVisible(_MIN_ELEVATION, _MAX_ELEVATION, _receiverPos, satPosXYZ, out bool isVisible, 
                            out double elevation, out double azimuth);
                        if (isVisible)
                        {
                            //Check if a Satellite is blocked by the jammer
                            if (!(InterferenceCalculator.DoesLineSegmentIntersectSphere(satPosXYZ, _receiverPos, 
                                CoordinatesCalculator.GeodeticToECEF(_jConfig.JammerLatitude, _jConfig.JammerLongitude, 0), _jConfig.JammerRadius)) || !_jConfig.IsJammerOn)
                            {
                               
                                _satellitePositions.Add(satPosXYZ);
                                _visibleSatellitesPRN_GL.Add(broadcast.SatId);
                                _satListGL.Add(new SatelliteElevationAndAzimuthInfo()
                                {
                                    SatId = broadcast.SatId.Substring(1),
                                    Azimuth = Math.Round(azimuth, 0),
                                    Elevation = Math.Round(elevation, 0)
                                });
                            }
                        }
                    }
                }
            }
        }

        public void UpdatePos(double latPos, double longPos, double elevation)
        {
            this._rConfig.ReceiverLatitude = latPos;
            this._rConfig.ReceiverLongitude = longPos;
            this._rConfig.ReceiverElevetion = elevation; 
        }

        public void UpdateJammerPos(bool jamOn, double jamLat, double jamLong, double jamRad)
        {
            this._jConfig.IsJammerOn = jamOn;
            this._jConfig.JammerLatitude = jamLat;
            this._jConfig.JammerLongitude = jamLong;
            this._jConfig.JammerRadius = jamRad;
        }

        public DateTime DebugToTerminalGUIGetValues()
        {
            return new DateTime(_rConfig.ReceiverStartDT.Year, _rConfig.ReceiverStartDT.Month, _rConfig.ReceiverStartDT.Day, _boxHourGPS, _boxMin, 00);
        }

        public string DebugToTerminalGUIGetValuesDouble()
        {
            return "";
        }

        public double DebugToTerminalGUIGetValuesBool()
        {
            return 0;
        }
        public double GetApproxPos()
        {
            return _PDOP * _rConfig.ReceiverGpsError;
        }

    }
}
