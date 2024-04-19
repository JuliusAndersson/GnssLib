using GnssLibCALC.Models.SatModels;
namespace GnssLibCALC.Models.SatelliteSystemModels
{

    public class GPS
    {
        public List<GPSSatellite> satList { get; set; }
    }
}

//Broadcastdata -> Gps-sat -> gps -> sattelit
//Broadcastdata -> gal-sat -> gal -> sattelit