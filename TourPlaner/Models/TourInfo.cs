using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlaner.Models
{
    public class TourInfo
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Distance { get; set; }
        public string Time { get; set; }
        public bool HasTunnels { get; set; }
        public bool HasHighways { get; set; }
        public bool HasBridge { get; set; }
        public bool HasAccesRestriction { get; set; }
    }
}
