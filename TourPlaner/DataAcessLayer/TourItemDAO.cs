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

        public List<Tour> GetTours()
        {
            return accesType.GetTours();
        }

        public bool AddTour(string name)
        {
            accesType.AddTour(name);
            return true;
        }

        public bool DeleteTour(string name)
        {
            accesType.DeleteTour(name);
            return true;
        }
    }
}
