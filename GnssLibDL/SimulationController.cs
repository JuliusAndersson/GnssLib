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
using GnssLibCALC.Models.SatelliteSystemModels;


namespace GnssLibDL
{
    public class SimulationController
    {

        private readonly double _speedForSimulation = 1;
        private static readonly double _MIN_ELEVATION = 0;
        private static readonly double _MAX_ELEVATION = 90;
        
        private Satellites _GNSS_Data;

        //Input & Output
        public ReceiverConfiguration _rConfig;
        public JammerConfiguration _jConfig;
        public DateTime _simulationStartDateTime;

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

        public SimulationController( ReceiverConfiguration rConfig, JammerConfiguration jConfig, String fileName, DateTime simulationStartDateTime) {
            this._rConfig = rConfig;
            this._jConfig = jConfig;
            this._simulationStartDateTime = simulationStartDateTime;

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
            _continousSecFromStart += _speedForSimulation;
        }

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
                        if (!(InterferenceCalculator.DoesLineSegmentIntersectSphere(satPosXYZ, _receiverPos, CoordinatesCalculator.GeodeticToECEF(_jConfig.JammerLatitude, _jConfig.JammerLongitude, 0), _jConfig.JammerRadius)) || !_jConfig.IsJammerOn)
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
                        if (!(InterferenceCalculator.DoesLineSegmentIntersectSphere(satPosXYZ, _receiverPos,
                            CoordinatesCalculator.GeodeticToECEF(_jConfig.JammerLatitude, _jConfig.JammerLongitude, 0), _jConfig.JammerRadius)) || !_jConfig.IsJammerOn)
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
            return new DateTime(_simulationStartDateTime.Year, _simulationStartDateTime.Month, _simulationStartDateTime.Day, 00, 00, 00);
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
