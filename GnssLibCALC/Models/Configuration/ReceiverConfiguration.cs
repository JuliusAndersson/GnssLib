
namespace GnssLibCALC.Models.Configuration
{
    public class ReceiverConfiguration
    {
        public bool IsUsingGPS { get; set; }
        public bool IsUsingGalileo { get; set; }
        public bool IsUsingGlonass { get; set; }
        public double ReceiverLatitude { get; set; }
        public double ReceiverLongitude { get; set; }
        public double ReceiverAltitude { get; set; }
        public double ReceiverGpsError { get; set; }
    }
}
