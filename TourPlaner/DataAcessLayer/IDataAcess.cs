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
        public bool AddTour(string name);
        public bool DeleteTour(string name);

    }
}
