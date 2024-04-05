using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GnssLibCALC.Models.SatModels;
namespace GnssLibCALC.Models.SatelliteSystemModels
{
    public class Gps
    {
        public List<GpsSatellite> satList { get; set; }
    }
}

//Broadcastdata -> Gps-sat -> gps -> sattelit
//Broadcastdata -> gal-sat -> gal -> sattelit