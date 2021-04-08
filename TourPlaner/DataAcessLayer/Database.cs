using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlaner.Models;

namespace TourPlaner.DataAcessLayer
{
    class Database : IDataAcess
    {
        private List<Tour> tourItems = new List<Tour>()
        {
            new Tour() {Name = "Wien"},
            new Tour() {Name = "Salzbutg"},
            new Tour() {Name = "Kairo"},
            new Tour() {Name = "Atlanta"}
        };

        public Database()
        {
            //connection string to the database
        }

        public bool AddTour(string name)
        {
            tourItems.Add(new Tour() { Name = name });
            return true;
        }


        public bool DeleteTour(string name)
        {
            tourItems.RemoveAt(tourItems.FindIndex(a => a.Name == name));
            return true;
        }

        public List<Tour> GetTours()
        {
            return tourItems;
        }
    }
}
