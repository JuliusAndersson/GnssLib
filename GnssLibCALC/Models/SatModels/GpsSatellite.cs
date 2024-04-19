using GnssLibCALC.Models.BroadCastDataModels;

namespace GnssLibCALC.Models.SatModels
{
    /// <summary>
    /// Obejct for one Gps satellite. Contains a list of all it's broadcast messages that were parsed from a Rinex v.4 file. 
    /// </summary>
    public class GPSSatellite
    {
        public List<BroadCastDataLNAV> Data { get; set; }
        public string Id { get; set; }
    }
}
