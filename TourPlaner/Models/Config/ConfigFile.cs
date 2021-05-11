using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlaner.Models.Config;

namespace TourPlaner.Models
{
    public class ConfigFile
    {
        public DBConfig DbSettings { get; set; }
        public ImageConfig RouteImageSettings { get; set; }
        public ReqKeyConfig RequestKey { get; set; }
        public DeleteConfig ToDeletePath { get; set; }
        public LogToPdf GeneratePdf { get; set; }
        public ExportFiles ExportTours { get; set; }

    }
}
