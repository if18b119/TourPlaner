﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlaner.Models;

namespace TourPlaner.BusinessLayer
{
    public interface ITourItemFactory
    {

        IEnumerable<Tour> GetItems();
        IEnumerable<Tour> Search(string tourName, bool caseSensitive = false);
        public bool AddTour(string name, string from, string to, string description);
        public bool DeleteTour(Tour tour_to_delete);

    }
}
