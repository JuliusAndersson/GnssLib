using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GnssLibDL.Models.BroadCastDataModels;

namespace GnssLibDL.Models.SatModels
{
    internal class GPS_Sat
    {
        public List<BroadCastDataLNAV> Data { get; set; }
        public string id { get; set; }
        
    }
}
