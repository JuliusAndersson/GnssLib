using GnssLibCALC.Models;
using GnssLibCALC.Models.SatModels;
using GnssLibCALC.Models.BroadCastDataModels;
using System.IO.Ports;
using GnssLibCALC;
using System;


namespace GnssLibDL
{
    public class SimulationController
    {
        private static readonly double MIN_ELEVATION = 10;
        private static readonly double MAX_ELEVATION = 80;

        private bool useGPS;
        private bool useGalileo;
        private bool useGlonass;
        private DateTime dt;
        private double latPos = 0;
        private double longPos;

        private bool jamOn;
        private double jamStr;
        private double jamLat;
        private double jamLong;

        private Satellites GNSS_Data;

        private double updateLat;
        private double updateLong;
        private bool updateJamOn;
        private double updateJamLat;
        private double updateJamLong;
        private double updateJamStr;
        private double continousSec = 0; //var to change for time simulations

        private bool newHour = false;
        private int boxHour;

        private List<string> visibleSatellitesPRN;
        private List<double[]> satellitePositions;
        private List<Satellite> satList;
        private double PDOP;
        private double HDOP;
        private double VDOP;

        public SimulationController(bool gps, bool galileo, bool glonass, DateTime dateTime, String fileName,  
            double latPos, double longPos, bool jammer, double jamRad, double jamLat, double jamLong) {
            useGPS = gps;
            useGalileo = galileo;
            useGlonass = glonass;

            dt = dateTime;
            this.latPos = latPos;
            this.longPos = longPos;
            updateLat = latPos;
            updateLong = longPos;

            jamOn = jammer;
            this.jamStr = jamRad;
            this.jamLat = jamLat;
            this.jamLong = jamLong;
            boxHour = dateTime.Hour - (dateTime.Hour % 2);

            string filePath = $"Resources/Broadcast/{fileName}"; 
            BroadCastDataReader bcdr = new BroadCastDataReader();
            GNSS_Data = bcdr.ReadBroadcastData(filePath);
            

        }

        public void Tick()
        {
            if(latPos != updateLat) { latPos = updateLat; } 
            if(longPos != updateLong) { longPos = updateLong; }
            if(jamLat != updateJamLat) {  jamLat = updateJamLat; }
            if(jamLong != updateJamLong) {  jamLong = updateJamLong; }
            if(jamOn != updateJamOn) { jamOn = updateJamOn; }
            if(jamStr != updateJamStr) {  jamStr = updateJamStr; }

            visibleSatellitesPRN = new List<string>();
            satellitePositions = new List<double[]>();
            satList = new List<Satellite>();
            double[] receiverPos = CoordinatesCalculator.GeodeticToECEF(latPos, longPos, 0);
            
            foreach (var gps in GNSS_Data.gps.satList)
            {
                foreach (var broadcast in gps.Data)
                {
                    

                    if (dt.Hour % 2 == 0 && newHour)
                    {
                        newHour = false;
                        boxHour = dt.Hour;
                        continousSec = 0;
                    }
                    else if (dt.Hour % 2 == 1 && !newHour)
                    {
                        newHour = true;
                    }

                    //if (broadcast.DateTime > new DateTime(2024, 01, 01, boxHour-1, 55, 00) && broadcast.DateTime < new DateTime(2024, 01, 01, boxHour, 05, 00))
                    if (broadcast.DateTime == new DateTime(dt.Year, dt.Month, dt.Day, boxHour, 00, 00))
                    {

                        double[] satPos = CoordinatesCalculator.CalculatePosition(broadcast, continousSec);
                        VisibleSatCalulator.IsSatelliteVisible(MIN_ELEVATION, MAX_ELEVATION, receiverPos, satPos, out bool isVisible, out double elevation, out double azimuth);
                        if (isVisible)
                        {
                            if (!(InterferenceCalculator.DoesLineSegmentIntersectSphere(satPos, receiverPos, CoordinatesCalculator.GeodeticToECEF(jamLat, jamLong, 0), jamStr)) || !jamOn)
                            {
                                satellitePositions.Add(satPos);
                                visibleSatellitesPRN.Add(broadcast.SatId);
                                satList.Add(new Satellite()
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

            DOPCalulator.CalculateDOP(receiverPos, satellitePositions, out double PDOP, out double HDOP, out double VDOP);
            this.PDOP = PDOP;
            this.HDOP = HDOP;
            this.VDOP = VDOP;
            continousSec += 1;
        }

        public void UpdatePos(double latPos, double longPos)
        {
            updateLat = latPos;
            updateLong = longPos;
        }

        public void UpdateJammerPos(bool jamOn, double jamLat, double jamLong, double jamStr)
        {
            updateJamOn = jamOn;
            updateJamLat = jamLat;
            updateJamLong = jamLong;
            updateJamStr = jamStr;
        }

        public List<Satellite> GetValues()
        {
            
            return satList;
        }

        public void NmeaValues(out List<string> activeSatellites, out double PDOP, out double HDOP, out double VDOP, 
                                out List<Satellite> satList, out DateTime utcTime, out double latitude, out double longitude)
        {
            activeSatellites = visibleSatellitesPRN;
            PDOP = this.PDOP;
            HDOP = this.HDOP;
            VDOP = this.VDOP;
            satList = this.satList;
            utcTime = dt;
            latitude = latPos;
            longitude = longPos;
        }


    }
}
