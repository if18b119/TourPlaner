using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlaner.Models;

namespace TourPlaner.DataAcessLayer
{
    public enum DataType
    {
        Database,
        Filesystem
    }
    public class TourItemDAO
    {
        private IDataAcess accesType;
        public TourItemDAO(DataType dt)
        {
            if (dt == DataType.Database)
                accesType = new Database();
            else if (dt == DataType.Filesystem)
                accesType = new FileSystem();
        }

        public bool UpdateLogValue(string tour_id, string log_id, string to_update_column, string new_value)
        {
            return accesType.UpdateLogValue(tour_id, log_id, to_update_column, new_value);
        }

        public string SaveImage(string from, string to)
        {
            return accesType.SaveImage(from, to);
        }

        public void SaveImagePath(string path)
        {
            accesType.SaveImagePath(path);
        }

        public List<Tour> GetTours()
        {
            return accesType.GetTours();
        }

        public bool  MakeReport(List<Tour> tours)
        {
            return accesType.MakeReport(tours);
        }

        public bool AddTour(String UUID, string name, string from, string to, string pic_path, string description, string route_type)
        {
            accesType.AddTourAsync(UUID, name, from, to, pic_path, description, route_type);
            return true;
        }

        public bool DeleteTour(string name)
        {
            accesType.DeleteTour(name);
            return true;
        }

        
        public bool DeleteAllTour()
        {
            accesType.DeleteAllTour();
            return true;
        }

        public bool DeleteImages( )
        {
            accesType.DeleteImages();
            return true;
        }

        public bool AddLog(Tour current_tour, string date_Time, string distance, string totalTime, string report, string rating, string avarage_speed, string comment, string problems, string transport_modus, string recomended)
        {
            accesType.AddLog(current_tour, date_Time, distance, totalTime, report, rating, avarage_speed, comment, problems, transport_modus, recomended);
            return true;

        }
        public ObservableCollection<Log> GetTourLogs(String UUID)
        {
            return accesType.GetTourLogs(UUID);
            
        }
        public Log GetNewLog(string tour_id, string log_id)
        {
            return accesType.GetNewLog(tour_id, log_id);
        }
        public Tour GetNewTour(string tour_id)
        {
           return accesType.GetNewTour(tour_id);
        }

        public bool DeleteLog(string tour_id, string log_id)
        {
            return accesType.DeleteLog(tour_id, log_id);
        }

        public bool MakePdf(Tour current_tour)
        {
            return accesType.MakePdf(current_tour);
        }

        public bool Export(List<Tour> current_tours_in_DB)
        {
            return accesType.Export(current_tours_in_DB);
        }
        public bool DoesTourExistInDb(string tour_id)
        {
            return accesType.DoesTourExistInDb(tour_id);
        }
    }
}
