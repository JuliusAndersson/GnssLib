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