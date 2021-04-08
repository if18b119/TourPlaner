using System.Collections.Generic;
using System.Linq;
using TourPlaner.DataAcessLayer;
using TourPlaner.Models;

namespace TourPlaner.BusinessLayer
{
    internal class TourItemFactoryImpl : ITourItemFactory
    {

        private TourItemDAO tourItemDAO;

        public TourItemFactoryImpl(DataType dt)
        {
            tourItemDAO = new TourItemDAO(dt);
        }
              


        public bool AddTour(string name)
        {
            tourItemDAO.AddTour(name);
            return true;
        }

        public bool DeleteTour(string name)
        {
            tourItemDAO.DeleteTour(name);
            return true;
        }


        public IEnumerable<Tour> GetItems()
        {
            return tourItemDAO.GetTours();
             
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