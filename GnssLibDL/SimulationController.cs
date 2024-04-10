﻿using GnssLibCALC.Models;
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

        /// <summary>
        /// The init sets global var and reads the Epemeris file.
        /// </summary>
        /// <param name="rConfig"> Configurations for the reciever. </param>
        /// <param name="jConfig"> Configurations for the jammer. </param>
        /// <param name="fileName"> The name of the epemeris file to be used.</param>
        /// <param name="simulationStartDateTime"> The DateTime that the simulation starts at.</param>
        public SimulationController( ReceiverConfiguration rConfig, JammerConfiguration jConfig, String fileName, DateTime simulationStartDateTime) {
            this._rConfig = rConfig;
            this._jConfig = jConfig;
            this._simulationStartDateTime = simulationStartDateTime;

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
        /// <summary>
        /// Used to update the receiver position.
        /// </summary>
        public void UpdatePos(double latPos, double longPos, double elevation)
        {
            this._rConfig.ReceiverLatitude = latPos;
            this._rConfig.ReceiverLongitude = longPos;
            this._rConfig.ReceiverElevetion = elevation; 
        }
        /// <summary>
        /// Used to update the jammer position.
        /// </summary>
        public void UpdateJammerPos(bool jamOn, double jamLat, double jamLong, double jamRad)
        {
            this._jConfig.IsJammerOn = jamOn;
            this._jConfig.JammerLatitude = jamLat;
            this._jConfig.JammerLongitude = jamLong;
            this._jConfig.JammerRadius = jamRad;
        }

        /// <summary>
        /// Calculates position accuracy with PDOP and gps error.
        /// </summary>
        public double GetApproxPos()
        {
            return _PDOP * _rConfig.ReceiverGpsError;
        }


        /// <summary>
        /// 3 methods used to debug and write to the Terminal on the GUI.
        /// </summary>
        public DateTime DebugToTerminalGUIGetValues()
        {
            return new DateTime(_simulationStartDateTime.Year, _simulationStartDateTime.Month, _simulationStartDateTime.Day, 00, 00, 00);
        }

        public string DebugToTerminalGUIGetValuesDouble()
        {
            double[] jampos = CoordinatesCalculator.GeodeticToECEF(55.706094, 13.190011, 0);
            double[] recpos = CoordinatesCalculator.GeodeticToECEF(55.718279, 13.174734,0);
            return "";
        }

        public double DebugToTerminalGUIGetValuesBool()
        {
            return 0;
        }

    }
}
