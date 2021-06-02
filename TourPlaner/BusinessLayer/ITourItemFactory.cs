using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlaner.Models;

namespace TourPlaner.BusinessLayer
{
    public interface ITourItemFactory
    {

        IEnumerable<Tour> GetItems();
        public ObservableCollection<Log> GetTourLogs(string UUID);
        IEnumerable<Tour> Search(string tourName, bool caseSensitive = false);
        public bool AddTour(String UUID, string name, string from, string to, string description, string route_type);
        public bool SavePathAndDeleteTour(Tour tour_to_delete);
        public bool DeleteImages();

        public bool Export();
        public bool MakePdf(Tour current_tour);
        public bool DeleteLog(string tour_id, string log_id);
        public Log GetNewLog(string tour_id, string log_id);
        public bool UpdateLogValue(string tour_id, string log_id, string to_update_column, string new_value);
        public bool AddLog(Tour current_tour, string date_Time, string distance, string totalTime, string report, string rating, string avarage_speed, string comment, string problems, string transport_modus, string recomended);
        public bool Import(string file_name);
        public bool ImportAndDelete(string file_name);
        public bool Paste(Tour to_copy);

    }
}
