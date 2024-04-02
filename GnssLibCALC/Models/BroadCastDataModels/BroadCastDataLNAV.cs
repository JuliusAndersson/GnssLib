using System;

namespace GnssLibCALC.Models.BroadCastDataModels
{
    public class BroadCastDataLNAV 
    {
        //SV/Epoch/SvClock
        public string SatId { get; set; }
        public DateTime DateTime { get; set; }
        public double ClockBias { get; set; }
        public double ClockDrift { get; set; }
        public double ClockDriftRate { get; set; }

        //Orbit1
        public double A_DOT { get; set; }
        public double C_rs { get; set; }
        public double Delta_n0 { get; set; }
        public double M0 { get; set; }

        //Orbit2
        public double C_uc { get; set; }
        public double e_Ecentricity { get; set; }
        public double C_us { get; set; }
        public double sqrtA { get; set; }

        //Orbit3
        public double t_oe { get; set; } //toe?
        public double C_ic { get; set; }
        public double OmegaA0 { get; set; } //omega
        public double C_is { get; set; }

        //Orbit4
        public double i0 { get; set; }
        public double C_rc { get; set; }
        public double omega { get; set; }
        public double OMEGA_DOT { get; set; }

        //Orbit5
        public double IDOT { get; set; }
        public double L2_code { get; set; }
        public double GPS_WEEK { get; set; }
        public double L2_P { get; set; }

        //Orbit6
        public double SV_acc { get; set; }
        public double SV_health { get; set; }
        public double TGD { get; set; } //tgd
        public double IODC { get; set; }

        //orbit7
        public double t_tm { get; set; }

    }
}

