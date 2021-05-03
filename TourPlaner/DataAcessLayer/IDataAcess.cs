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
        public void AddTourAsync(string name, string from, string to, string pic_path, string description,string route_Type);
        public bool DeleteTour(string name);
        public bool DeleteImages( );
        public bool AddLog(Tour current_tour, string date_Time, double distance, double totalTime, string report);

        public void SaveImagePath(string path);
        public string SaveImage(string from, string to);
    }
}
