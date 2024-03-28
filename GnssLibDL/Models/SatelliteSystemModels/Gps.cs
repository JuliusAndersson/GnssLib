using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GnssLibDL.Models.SatModels;
namespace GnssLibDL.Models.SatelliteSystemModels
{
    internal class Gps
    {
        public List<GPS_Sat> satList { get; set; }
    }
}

//Broadcastdata -> Gps-sat -> gps -> sattelit
//Broadcastdata -> gal-sat -> gal -> sattelit