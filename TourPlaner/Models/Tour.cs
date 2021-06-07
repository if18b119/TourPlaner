using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlaner.Models
{

    public class Tour
    {   
        public string UUID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Route_Type { get; set; }
        public string PicPath { get; set; }
        public TourInfo TourInfo { get; set; }
        public string TourInfoString { get; set; }
        public ObservableCollection<Log>LogItems { get; set; }

    }
}
