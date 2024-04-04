using System;
using GnssLibCALC.Models.BroadCastDataModels;

namespace GnssLibCALC.Models.SatModels
{
    public class GalileoSatellite
    {
        public List<BroadCastDataINAV> Data { get; set; }
        public string Id { get; set; }
    }
}

