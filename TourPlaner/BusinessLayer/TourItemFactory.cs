using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlaner.DataAcessLayer;
using TourPlaner.Models;

namespace TourPlaner.BusinessLayer
{
    public class TourItemFactory
    {
        private static ITourItemFactory tourItemFactory;
        
        public static ITourItemFactory GetInstance()
        {
            if(tourItemFactory == null)
            {
                tourItemFactory = new TourItemFactoryImpl();
            }
            return tourItemFactory;
        }

    }
}
