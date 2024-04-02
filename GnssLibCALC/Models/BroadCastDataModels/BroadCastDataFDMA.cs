using System;

namespace GnssLibCALC.Models.BroadCastDataModels
{
    public class BroadCastDataFDMA
    {

        public string SatId { get; set; }
        public DateTime DateTime { get; set; }
        public double ClockBias { get; set; }
        public double ClockDrift { get; set; }
        public double Message_frame_time { get; set; }

        //Orbit1
        public double Pos_X { get; set; }
        public double Velocity_X { get; set; }
        public double Acc_X { get; set; }
        public double Health { get; set; }

        //Orbit2
        public double Pos_Y { get; set; }
        public double Velocity_Y { get; set; }
        public double Acc_Y { get; set; }
        public double Frequency_number { get; set; }

        //Orbit3
        public double Pos_Z { get; set; } //toe?
        public double Velocity_Z { get; set; }
        public double Acc_Z { get; set; } //omega
        public double Age_of_opereration { get; set; }

        //Orbit4
        public double Status_Flags { get; set; }
        public double L1_L2_delay_diff { get; set; }
        public double URAI { get; set; }
        public double Health_Flags { get; set; }

    }
}

