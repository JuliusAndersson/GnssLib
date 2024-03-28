using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exjobbv2.Models.BroadCastDataModels;

namespace Exjobbv2.Models.SatModels
{
    internal class GPS_Sat
    {
        public List<BroadCastDataLNAV> Data { get; set; }
        public string id { get; set; }
        
    }
}
