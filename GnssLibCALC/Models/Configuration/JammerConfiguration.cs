using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GnssLibCALC.Models.Configuration
{
    public class JammerConfiguration
    {
        public bool IsJammerOn { get; set; }
        public double JammerRadius { get; set; }
        public double JammerLatitude { get; set; }
        public double JammerLongitude { get; set; }
    }
}
