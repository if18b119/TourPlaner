using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

        public bool AddLog(Tour current_tour, string date_Time, double distance, double totalTime, string report)
        {
            tourItemDatabase.AddLog(current_tour, date_Time, distance, totalTime, report);
            return true;
        }

        public bool AddTour(String UUID, string name, string from, string to, string description, string route_type)
        {
            //Um das foto aus dem request zu holen und um FIlesystem zu speichern
            string pic_path = tourItemFileSystem.SaveImage(from, to);

            tourItemDatabase.AddTour(UUID, name,  from,  to, pic_path, description, route_type);

            return true;
        }

        public bool DeleteImages()
        {
            tourItemFileSystem.DeleteImages();
            return true;
        }

        public bool SavePathAndDeleteTour(Tour tour_to_delete)
        {

            string picPath = tour_to_delete.PicPath;
            tour_to_delete.PicPath = string.Empty;   
            
            //string speichern um beim beenden des programms das foto aus dem filesystem zu löschen
            tourItemFileSystem.SaveImagePath(picPath);
            
            //das foto entfernen
            tourItemDatabase.DeleteTour(tour_to_delete.UUID);
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