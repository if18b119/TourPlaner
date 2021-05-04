using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlaner.Models;

namespace TourPlaner.BusinessLayer
{
    public interface ITourItemFactory
    {

        IEnumerable<Tour> GetItems();
        IEnumerable<Tour> Search(string tourName, bool caseSensitive = false);
        public bool AddTour(String UUID, string name, string from, string to, string description, string route_type);
        public bool SavePathAndDeleteTour(Tour tour_to_delete);
        public bool DeleteImages();
        public bool AddLog(Tour current_tour, string date_Time, double distance, double totalTime, string report);


    }
}
