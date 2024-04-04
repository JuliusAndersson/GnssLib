using System;

namespace GnssLibCALC.Models.BroadCastDataModels
{
    /// <summary>
    /// Object for all paramaters parsed from a Rinex v.4 File related to GPS in the LNAV format
    /// </summary>
    public class BroadCastDataLNAV 
    {
        
        public string SatId { get; set; }
        public DateTime DateTime { get; set; }
        public double ClockBias { get; set; } //Not Used
        public double ClockDrift { get; set; } //Not Used
        public double ClockDriftRate { get; set; } //Not Used

        //Orbit1
        public double IssueOfDataEphemeris { get; set; }
        public double RadiusCorrectionSinusComponent { get; set; }
        public double Delta_n0 { get; set; }
        public double M0_Angle { get; set; }

        //Orbit2
        public double LatitudeCorrectionCosinusComponent { get; set; }
        public double OrbitEcentricity { get; set; }
        public double LatitudeCorrectionSinusComponent { get; set; }
        public double SqrtOfMajorAxis { get; set; }

        //Orbit3
        public double TimeOfEphemeris { get; set; } 
        public double InclinationCorrectionCosinusComponent { get; set; }
        public double OmegaAngle0 { get; set; } 
        public double AngularVelocity { get; set; }

        //Orbit4
        public double InitialInclination { get; set; }
        public double RadiusCorrectionCosinusComponent { get; set; }
        public double OmegaAngle { get; set; }
        public double AngularVelocityPerSec { get; set; }

        //Orbit5
        public double InclinationRate { get; set; }
        public double CodesOnL2Channel { get; set; } //Not Used
        public double ContinousGpsWeek { get; set; } //Not Used
        public double L2PDataFlag { get; set; } //Not Used

        //Orbit6
        public double SpaceVehicleAccuarcy { get; set; } //Not Used
        public double SpaceVehicleHealth { get; set; } //Not Used
        public double TotalGroupDelay { get; set; } //Not Used
        public double IssueOfDataClock { get; set; } 

        //orbit7
        public double TransmissionTime { get; set; }

    }
}

