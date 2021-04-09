using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlaner.Models
{
    public enum RouteType
    {
        Fastest,
        Shortest,
        Pedestrian,
        Multimodal,
        Bicycle
    }
    public class Tour
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public RouteType Route_Type { get; set; }
        public string PicPath { get; set; }
    }
}
