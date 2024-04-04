using GnssLibCALC.Models;
using GnssLibCALC.Models.SatModels;
using GnssLibCALC.Models.BroadCastDataModels;
using System.IO.Ports;
using GnssLibCALC;
using System;
using GeoTiffElevationReader;
using MightyLittleGeodesy.Positions;


namespace GnssLibDL
{
    public class SimulationController
    {
        private static readonly double _MIN_ELEVATION = 0;
        private static readonly double _MAX_ELEVATION = 90;

        private double _continousSecGPS = 0;
        private double _continousSecGAL = 0;
        private double _continousSecFromStart = 0;

        private bool _isNewHour = false;
        private int _boxHour;
        private bool _isNewQuart = false;
        private int _boxMin;

        private Satellites _GNSS_Data;
        private string _geoFilePath;


        //Input
        private bool _isUsingGPS;
        private bool _isUsingGalileo;
        private bool _isUsingGlonass;
        
        private double _latPos = 0;
        private double _longPos;

        private bool _isJamOn;
        private double _jamStr;
        private double _jamLat;
        private double _jamLong;


        //Input & Output
        private DateTime _dt;
        private double _elevation;


        //Output
        private List<string> _visibleSatellitesPRN;
        private List<string> _visibleSatellitesPRNGL;
        private List<double[]> _satellitePositions;
        private List<Satellite> _satList;
        private List<Satellite> _satListGL;
        
        private double _PDOP;
        private double _HDOP;
        private double _VDOP;
        private double[] _receiverPos;

        public SimulationController(bool gps, bool galileo, bool glonass, DateTime dateTime, String fileName,  
            double latPos, double longPos, bool jammer, double jamRad, double jamLat, double jamLong) {
            _isUsingGPS = gps;
            _isUsingGalileo = galileo;
            _isUsingGlonass = glonass;

            _dt = dateTime;
            this._latPos = latPos;
            this._longPos = longPos;

            _isJamOn = jammer;
            _jamStr = jamRad;
            this._jamLat = jamLat;
            this._jamLong = jamLong;

            _boxHour = dateTime.Hour - (dateTime.Hour % 2);

            //Read File and put in a Satellites variable
            string filePath = $"Resources/Broadcast/{fileName}"; 
            BroadCastDataReader bcdr = new BroadCastDataReader();
            _GNSS_Data = bcdr.ReadBroadcastData(filePath);

            //Check if elevetion map exists
            _geoFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources/ElevationMaps/62_3_2023.tif");
            _elevation = initElevation(latPos, longPos);



        }

        public void Tick()
        {

            //Reset Lists for new values at the new time
            _visibleSatellitesPRN = new List<string>();
            _satellitePositions = new List<double[]>();
            _satList = new List<Satellite>();
            _receiverPos = CoordinatesCalculator.GeodeticToECEF(_latPos, _longPos, _elevation);
            
            _visibleSatellitesPRNGL = new List<string>();
            _satListGL = new List<Satellite>();

            //Check if GPS, Galileo, Glonass is to be Used
            if (_isUsingGPS)
            {
                MakeGps();
            }
            if (_isUsingGalileo)
            {
                MakeGalileo();
            }


            // Calculate DOP for Visible Satellites
            DOPCalulator.CalculateDOP(_receiverPos, _satellitePositions, out double PDOP, out double HDOP, out double VDOP);
            this._PDOP = PDOP;
            this._HDOP = HDOP;
            this._VDOP = VDOP;
            _continousSecGPS += 1;
            _continousSecGAL += 1;
            _continousSecFromStart += 1;
        }

        private void MakeGps()
        {
            foreach (var gps in _GNSS_Data.gps.satList)
            {
                foreach (var broadcast in gps.Data)
                {

                    //Check if it's another 2h mark so the next messege Block should be used
                    if (_dt.Hour % 2 == 0 && _isNewHour)
                    {
                        _isNewHour = false;
                        _boxHour = _dt.Hour;
                        _continousSecGPS = 0;
                    }
                    else if (_dt.Hour % 2 == 1 && !_isNewHour)
                    {
                        _isNewHour = true;
                    }

                    //if (broadcast.DateTime > new DateTime(2024, 01, 01, boxHour-1, 55, 00) && broadcast.DateTime < new DateTime(2024, 01, 01, boxHour, 05, 00))
                    if (broadcast.DateTime == new DateTime(_dt.Year, _dt.Month, _dt.Day, _boxHour, 00, 00))
                    {
                        //Check if a Satellite is visible
                        double[] satPos = CoordinatesCalculator.CalculatePosition(broadcast, _continousSecGPS);
                        VisibleSatCalulator.IsSatelliteVisible(_MIN_ELEVATION, _MAX_ELEVATION, _receiverPos, satPos, out bool isVisible, out double elevation, out double azimuth);
                        if (isVisible)
                        {
                            //Check if a Satellite is blocked by the jammer
                            if (!(InterferenceCalculator.DoesLineSegmentIntersectSphere(satPos, _receiverPos, CoordinatesCalculator.GeodeticToECEF(_jamLat, _jamLong, 0), _jamStr)) || !_isJamOn)
                            {
                                _satellitePositions.Add(satPos);
                                _visibleSatellitesPRN.Add(broadcast.SatId);
                                _satList.Add(new Satellite()
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
            foreach (var galileo in _GNSS_Data.galileo.satList)
            {
                foreach (var broadcast in galileo.Data)
                {

                    //Check if it's another 15min mark so the next messege Block should be used
                    if (_dt.Minute % 15 == 0 && _isNewQuart)
                    {
                        _isNewQuart = false;
                        _boxHour = _dt.Hour;
                        _boxMin = _dt.Minute;
                        _continousSecGAL = 0;
                    }
                    else if (_dt.Hour % 15 == 14 && !_isNewQuart)
                    {
                        _isNewQuart = true;
                    }

                    //if (broadcast.DateTime > new DateTime(2024, 01, 01, boxHour-1, 55, 00) && broadcast.DateTime < new DateTime(2024, 01, 01, boxHour, 05, 00))
                    if (broadcast.DateTime == new DateTime(_dt.Year, _dt.Month, _dt.Day, _boxHour, _boxMin, 00))
                    {
                        //Check if a Satellite is visible
                        double[] satPos = CoordinatesCalculator.CalculatePosition(broadcast, _continousSecGAL);
                        VisibleSatCalulator.IsSatelliteVisible(_MIN_ELEVATION, _MAX_ELEVATION, _receiverPos, satPos, out bool isVisible, 
                            out double elevation, out double azimuth);
                        if (isVisible)
                        {
                            //Check if a Satellite is blocked by the jammer
                            if (!(InterferenceCalculator.DoesLineSegmentIntersectSphere(satPos, _receiverPos, 
                                CoordinatesCalculator.GeodeticToECEF(_jamLat, _jamLong, 0), _jamStr)) || !_isJamOn)
                            {
                                _satellitePositions.Add(satPos);
                                _visibleSatellitesPRNGL.Add(broadcast.SatId);
                                _satListGL.Add(new Satellite()
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

        public void UpdatePos(double latPos, double longPos)
        {
            this._latPos = latPos;
            this._longPos = longPos;

            _elevation = initElevation(latPos, longPos);
        }

        public void UpdateJammerPos(bool jamOn, double jamLat, double jamLong, double jamStr)
        {
            this._isJamOn = jamOn;
            this._jamLat = jamLat;
            this._jamLong = jamLong;
            this._jamStr = jamStr;
        }

        public double GetValues()
        {
            
            return _elevation;
        }

        public void NmeaValues(out List<string> activeSatellites, out List<string> activeSatellitesGL, out double PDOP, out double HDOP, out double VDOP, 
                                out List<Satellite> satList, out List<Satellite> satListGL, out DateTime utcTime, out double latitude, out double longitude, out double elevation)
        {
            activeSatellites = _visibleSatellitesPRN;
            activeSatellitesGL = _visibleSatellitesPRNGL;
            PDOP = this._PDOP;
            HDOP = this._HDOP;
            VDOP = this._VDOP;
            satList = this._satList;
            satListGL = this._satListGL;
            utcTime = _dt.AddSeconds(_continousSecFromStart);
            latitude = _latPos;
            longitude = _longPos;
            elevation = this._elevation;
        }

        public double GetApproxPos()
        {
            return _PDOP * 3;
        }


        private double initElevation(double latitude, double longitude)
        {
            WGS84Position wgsPos = new WGS84Position();
            wgsPos.SetLatitudeFromString(CoordinatesCalculator.DoubleToDegreesMinutesSeconds(latitude, true), WGS84Position.WGS84Format.DegreesMinutesSeconds);
            wgsPos.SetLongitudeFromString(CoordinatesCalculator.DoubleToDegreesMinutesSeconds(longitude, false), WGS84Position.WGS84Format.DegreesMinutesSeconds);
            SWEREF99Position rtPos = new SWEREF99Position(wgsPos, SWEREF99Position.SWEREFProjection.sweref_99_tm);
            double elevation = 0;
            if (rtPos.Latitude > 6200000 &&  rtPos.Latitude < 6300000 && rtPos.Longitude <400000 && rtPos.Longitude > 300000)
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
