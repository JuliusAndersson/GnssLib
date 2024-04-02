using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GnssLibCALC.Models.BroadCastDataModels;

namespace GnssLibCALC.Models.SatModels
{
    public class GPS_Sat
    {
        public List<BroadCastDataLNAV> Data { get; set; }
        public string id { get; set; }
        
    }
}
