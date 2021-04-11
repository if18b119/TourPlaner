using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlaner.Models;

namespace TourPlaner.DataAcessLayer
{
    interface IDataAcess 
    {
        public List<Tour> GetTours();
        public bool AddTour(string name, string from, string to, string pic_path, string description);
        public bool DeleteTour(string name);

        public string SaveImage(string from, string to);
    }
}
