using System;
using GnssLibCALC.Models.SatModels;

namespace GnssLibCALC.Models.SatelliteSystemModels
{
    /// <summary>
    /// Contains a list of all Gallileo satellites that was parsed from the broadcast file.
    /// </summary>
    public class Galileo
    {
        public List<GalileoSatellite> ListOfSatellites { get; set; }
    }
}

