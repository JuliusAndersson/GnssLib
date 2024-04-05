using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GnssLibCALC.Models.Configuration
{
    internal class ReceiverConfiguration
    {
        public bool IsUsingGPS { get; set; }
        public bool IsUsingGalileo { get; set; }
        public bool IsUsingGlonass { get; set; }
        public DateTime ReceiverDT { get; set; }
        public double ReceiverLatitude { get; set; }
        public double ReceiverLongitude { get; set; }
        public double ReceiverElevetion { get; set; }
    }
}
