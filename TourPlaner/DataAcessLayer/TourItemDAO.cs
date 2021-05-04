using System;
using System.Collections.Generic;
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

        public bool DeleteImages( )
        {
            accesType.DeleteImages();
            return true;
        }

        public bool AddLog(Tour current_tour, string date_Time, double distance, double totalTime, string report)
        {
            accesType.AddLog(current_tour, date_Time, distance, totalTime, report);
            return true;

        }
    }
}
