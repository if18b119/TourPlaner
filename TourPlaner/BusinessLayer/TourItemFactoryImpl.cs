using System.Collections.Generic;
using System.Linq;
using TourPlaner.DataAcessLayer;
using TourPlaner.Models;

namespace TourPlaner.BusinessLayer
{
    internal class TourItemFactoryImpl : ITourItemFactory
    {

        private TourItemDAO tourItemDatabase;
        private TourItemDAO tourItemFileSystem;

        public TourItemFactoryImpl()
        {
            tourItemDatabase = new TourItemDAO(DataType.Database);
            tourItemFileSystem = new TourItemDAO(DataType.Filesystem);
        }
              


        public bool AddTour(string name, string from, string to)
        {
            string pic_path = tourItemFileSystem.SaveImage(from, to);
            tourItemDatabase.AddTour(name,  from,  to, pic_path);

            return true;
        }

        public bool DeleteTour(string name)
        {
            tourItemDatabase.DeleteTour(name);

            return true;
        }


        public IEnumerable<Tour> GetItems()
        {
            return tourItemDatabase.GetTours();
             
        }

        public IEnumerable<Tour> Search(string tourName, bool caseSensitive = false)
        {
            IEnumerable<Tour> items = GetItems();

            if (caseSensitive)
            {
                return items.Where(x => x.Name.Contains(tourName));
            }
            return items.Where(x => x.Name.ToLower().Contains(tourName.ToLower()));
        }
    }
}