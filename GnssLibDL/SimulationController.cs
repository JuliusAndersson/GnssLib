using Exjobbv2.Models;
using Exjobbv2.Models.BroadCastDataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GnssLibDL
{
    internal class SimulationController
    {
        private bool useGPS;
        private bool useGalileo;
        private bool useGlonass;
        private DateTime dt;
        private double latPos;
        private double longPos;
        private bool useJammer;
        private double jamRad;
        private double jamLat;
        private double jamLong;
        private Satellites GNSS_Data;

        private double updateLat;
        private double updateLong;



        public SimulationController(bool gps, bool galileo, bool glonass, DateTime dateTime, double latPos, double longPos, bool jammer, double jamRad, double jamLat, double jamLong) {
            useGPS = gps;
            useGalileo = galileo;
            useGlonass = glonass;
            dt = dateTime;
            this.latPos = latPos;
            this.longPos = longPos;
            useJammer = jammer;
            this.jamRad = jamRad;
            this.jamLat = jamLat;
            this.jamLong = jamLong;

            string filePath = $"Resources/Broadcast/{dt.Year}_{ dt.Month}_{dt.Day}_BroadCastFile.rnx"; // 2024_01_01_BroadCastFile.rnx
            //BroadCastDataReader bcdr = new BroadCastDataReader();
            //GNSS_Data = bcdr.ReadBroadCastData(filePath);


        }

        public void Tick()
        {
            if(latPos != updateLat) { latPos = updateLat; } 
            if(longPos != updateLong) { longPos = updateLong; }
            





        }

        public void UpdatePos(double latPos, double longPos)
        {
            updateLat = latPos;
            updateLong = longPos;
        }


    }
}
