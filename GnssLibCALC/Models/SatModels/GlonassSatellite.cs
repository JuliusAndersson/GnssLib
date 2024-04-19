using GnssLibCALC.Models.BroadCastDataModels;

namespace GnssLibCALC.Models.SatModels
{
    public class GlonassSatellite
    {
        public List<BroadCastDataFDMA> Data { get; set; }
        public string id { get; set; }
    }
}

