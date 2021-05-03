using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlaner.Models
{
    public class ConfigFile
    {
        public DBConfig DbSettings { get; set; }
        public ImageConfig RouteImageSettings { get; set; }
        public ReqKeyConfig RequestKey { get; set; }
        public DeleteConfig ToDeletePath { get; set; }

   
    }
}
