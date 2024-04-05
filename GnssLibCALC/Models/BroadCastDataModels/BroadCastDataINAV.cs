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
        public double SqtOfMajorAxis { get; set; }

        //Orbit3
        public double TimeOfEphemeris { get; set; } //toe?
        public double InclinationCorrectionCosinusComponent { get; set; }
        public double OmegaAngle0 { get; set; } //omega
        public double AngularVelocity { get; set; }

        //Orbit4
        public double InitialInclination { get; set; }
        public double RadiusCorrectionCosinusComponent { get; set; }
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


        public override string ToString()
        {
            return $"SatId: {SatId}\nDateTime: {DateTime}\nClockBias: {ClockBias}\nClockDrift: {ClockDrift}\nClockDriftRate: {ClockDriftRate}\n\n" +
                   $"// Orbit1\nIssueOfData: {IssueOfData}\nRadiusCorrectionSinusComponent: {RadiusCorrectionSinusComponent}\nDelta_n: {Delta_n}\nM0_Angle: {M0_Angle}\n\n" +
                   $"// Orbit2\nLatitudeCorrectionCosinusComponent: {LatitudeCorrectionCosinusComponent}\nOrbitEcentricity: {OrbitEcentricity}\n" +
                   $"LatitudeCorrectionSinusComponent: {LatitudeCorrectionSinusComponent}\nSqtOfMajorAxis: {SqtOfMajorAxis}\n\n" +
                   $"// Orbit3\nTimeOfEphemeris: {TimeOfEphemeris}\nInclinationCorrectionCosinusComponent: {InclinationCorrectionCosinusComponent}\n" +
                   $"OmegaAngle0: {OmegaAngle0}\nAngularVelocity: {AngularVelocity}\n\n" +
                   $"// Orbit4\nInitialInclination: {InitialInclination}\nRadiusCorrectionCosinusComponent: {RadiusCorrectionCosinusComponent}\n" +
                   $"OmegaAngle: {OmegaAngle}\nAngularVelocityPerSec: {AngularVelocityPerSec}\n\n" +
                   $"// Orbit5\nInclinationRate: {InclinationRate}\nDataSources: {DataSources}\nGalileoWeek: {GalileoWeek}\n\n" +
                   $"// Orbit6\nSignalInSpaceAccuarcy: {SignalInSpaceAccuarcy}\nSpaceVehicleHealth: {SpaceVehicleHealth}\nBGD_a: {BGD_a}\nBGD_b: {BGD_b} \n t: {TransmissionTime}";
        }




    }

}

