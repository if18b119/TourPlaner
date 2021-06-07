using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlaner.Models.JsonObjects
{
    public class RouteInformation
    {
        public double distance { get; set; }
        public int time { get; set; }
        public bool hasAccessRestriction { get; set; }
        public bool hasBridge { get; set; }
        public bool hasTunnel { get; set; }
        public bool hasHighway { get; set; }
    }
}
