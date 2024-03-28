using GnssLibDL.Models;
using GnssLibDL.Models.BroadCastDataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GnssLibDL
{
    public class SimulationController
    {
        private bool useGPS;
        private bool useGalileo;
        private bool useGlonass;
        private DateTime dt;
        private double latPos;
        private double longPos;

        private bool jamOn;
        private double jamStr;
        private double jamLat;
        private double jamLong;

        private bool NMEA_On;

        private Satellites GNSS_Data;

        private double updateLat;
        private double updateLong;
        private bool updateJamOn;
        private double updateJamLat;
        private double updateJamLong;
        private double updateJamStr;


        public SimulationController(bool gps, bool galileo, bool glonass, DateTime dateTime, double latPos, double longPos, bool jammer, double jamRad, double jamLat, double jamLong, bool NMEA_On) {
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
            this.NMEA_On = NMEA_On;
            

            string filePath = $"Resources/Broadcast/{dt.Year}_{ dt.Month}_{dt.Day}_BroadCastFile.rnx"; // 2024_01_01_BroadCastFile.rnx
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





            if (NMEA_On)
            {
                //skicka till NMEA Generator
            }


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

        public void GetValues()
        {

        }

    }
}
