using System;
using System.Collections.Generic;
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
    class TourItemDAO
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

        public List<Tour> GetTours()
        {
            return accesType.GetTours();
        }

        public bool AddTour(string name, string from, string to, string pic_path)
        {
            accesType.AddTour(name, from, to, pic_path);
            return true;
        }

        public bool DeleteTour(string name)
        {
            accesType.DeleteTour(name);
            return true;
        }
        
        
    }
}
