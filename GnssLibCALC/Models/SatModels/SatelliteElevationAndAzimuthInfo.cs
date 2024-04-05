using System;




namespace GnssLibCALC.Models.SatModels
{
	/// <summary>
	/// Object contains SatId, elevation and azimuth for a visible satellite. Used when constructing NMEA-messages
	/// </summary>
	public class SatelliteElevationAndAzimuthInfo
	{
		public string SatId { get; set; }
		public double Elevation { get; set; }
		public double Azimuth { get; set; }
	}
}

