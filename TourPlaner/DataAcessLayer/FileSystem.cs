using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlaner.Models;

namespace TourPlaner.DataAcessLayer
{
    class FileSystem : IDataAcess
    {
        private readonly string filePath;

        public FileSystem()
        {
            this.filePath = "...";
        }
        public bool AddTour(string name)
        {
            throw new NotImplementedException();
        }
   

        public bool DeleteTour(string name)
        {
            throw new NotImplementedException();
        }

        public List<Tour> GetTours()
        {
            throw new NotImplementedException();
        }
    }
}
