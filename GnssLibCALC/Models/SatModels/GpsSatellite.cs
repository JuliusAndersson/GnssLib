using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GnssLibCALC.Models.BroadCastDataModels;

namespace GnssLibCALC.Models.SatModels
{
    /// <summary>
    /// Obejct for one Gps satellite. Contains a list of all it's broadcast messages that were parsed from a Rinex v.4 file. 
    /// </summary>
    public class GpsSatellite
    {
        public List<BroadCastDataLNAV> Data { get; set; }
        public string id { get; set; }
        
    }
}
