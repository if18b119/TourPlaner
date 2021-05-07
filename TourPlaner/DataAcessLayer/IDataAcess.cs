using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlaner.Models;

namespace TourPlaner.DataAcessLayer
{
    interface IDataAcess 
    {
        public List<Tour> GetTours();
        public void AddTourAsync(String UUID, string name, string from, string to, string pic_path, string description,string route_Type);
        public bool DeleteTour(string name); //tour von der db löschen
        public bool DeleteImages( ); //fotos vom filesystem mittels pfad in json datei löschen
        public bool AddLog(Tour current_tour, string date_Time, string distance, string totalTime, string report, string rating, string avarage_speed, string comment, string problems, string transport_modus, string recomended);
        public ObservableCollection<Log> GetTourLogs(String UUID);
        public bool UpdateLogValue(string tour_id, string log_id, string to_update_column, string new_value);
        public void SaveImagePath(string path); //das speichern des pfads in json datei um es später löschen zu können
        public string SaveImage(string from, string to);
        public Log GetNewLog(string tour_id, string log_id);
        public bool DeleteLog(string tour_id, string log_id);
    }
}
