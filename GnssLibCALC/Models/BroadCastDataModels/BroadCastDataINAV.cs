using System;

namespace GnssLibCALC.Models.BroadCastDataModels
{
    public class BroadCastDataINAV
    {
        public string SatId { get; set; }
        public DateTime DateTime { get; set; }
        public double ClockBias { get; set; }
        public double ClockDrift { get; set; }
        public double ClockDriftRate { get; set; }

        //Orbit1
        public double IssueOfData { get; set; }
        public double RadiusCorrectionSinusComponent { get; set; }
        public double Delta_n { get; set; }
        public double M0_Angle { get; set; }

        //Orbit2
        public double LatitudeCorrectionCosinusComponent { get; set; }
        public double OrbitEcentricity { get; set; }
        public double LatitudeCorrectionSinusComponent { get; set; }
        public double SrqtOfMajorAxis { get; set; }

        //Orbit3
        public double TimeOfEphemeris { get; set; } //toe?
        public double nclinationCorrectionCosinusComponent { get; set; }
        public double OmegaAngle0 { get; set; } //omega
        public double AngularVelocity { get; set; }

        //Orbit4
        public double InitialInclination { get; set; }
        public double RandiusCorrectionCosinusComponent { get; set; }
        public double OmegaAngle { get; set; }
        public double AngularVelocityPerSec { get; set; }

        //Orbit5
        public double InclinationRate { get; set; }
        public double DataSources { get; set; }
        public double GalileoWeek { get; set; }


        //Orbit6
        public double SignalInSpaceAccuarcy { get; set; }
        public double SpaceVehicleHealth { get; set; }
        public double BGD_a { get; set; } //tgd
        public double BGD_b { get; set; }

        //orbit7
        public double TransmissionTime { get; set; }
    }
}

