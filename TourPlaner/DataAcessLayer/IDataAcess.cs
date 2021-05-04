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
        public void AddTourAsync(String UUID, string name, string from, string to, string pic_path, string description,string route_Type);
        public bool DeleteTour(string name); //tour von der db löschen
        public bool DeleteImages( ); //fotos vom filesystem mittels pfad in json datei löschen
        public bool AddLog(Tour current_tour, string date_Time, double distance, double totalTime, string report);

        public void SaveImagePath(string path); //das speichern des pfads in json datei um es später löschen zu können
        public string SaveImage(string from, string to);
    }
}
