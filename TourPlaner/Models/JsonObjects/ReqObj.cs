using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlaner.Models.JsonObjects
{
    public class ReqObj
    {
        //"distance", "formattedTime", "hasAccessRestriction",  "hasBridge", "hasTunnel", "hasHighway"
        public RouteInformation route { get; set; }
    }
}
