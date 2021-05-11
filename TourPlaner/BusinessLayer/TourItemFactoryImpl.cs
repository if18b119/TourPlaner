
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public bool AddLog(Tour current_tour, string date_Time, string distance, string totalTime, string report, string rating, string avarage_speed, string comment, string problems, string transport_modus, string recomended)
        {
            tourItemDatabase.AddLog(current_tour, date_Time, distance, totalTime, report, rating, avarage_speed, comment, problems, transport_modus, recomended);
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
                return items.Where(x => x.Name.Contains(tourName));// || x => x.From.Contains(tourName));
            }
            return items.Where(x => x.Name.ToLower().Contains(tourName.ToLower()));
        }

        public ObservableCollection<Log> GetTourLogs(string UUID)
        {
           return tourItemDatabase.GetTourLogs(UUID);
        }

        public bool UpdateLogValue(string tour_id, string log_id, string to_update_column, string new_value)
        {
            
            return tourItemDatabase.UpdateLogValue(tour_id, log_id, to_update_column, new_value);
        }

        public Log GetNewLog(string tour_id, string log_id)
        {
            return tourItemDatabase.GetNewLog(tour_id, log_id);
        }

        public bool DeleteLog(string tour_id, string log_id)
        {
            return tourItemDatabase.DeleteLog(tour_id, log_id);
        }

        public bool MakePdf(Tour current_tour)
        {
            return tourItemFileSystem.MakePdf(current_tour);
        }

        public bool Export()
        {
            List<Tour> current_tours_in_DB = new List<Tour>();
            current_tours_in_DB = tourItemDatabase.GetTours();
            foreach (Tour t in current_tours_in_DB)
            {
                t.LogItems = tourItemDatabase.GetTourLogs(t.UUID);
            }
            return tourItemFileSystem.Export(current_tours_in_DB);
        }

        public bool Import(string file_name)
        {
            //Die liste an tours von dem json file deserializen
            List<Tour> tours_from_json = new List<Tour>();
            string json_path = file_name;
            string json = File.ReadAllText(json_path);
            tours_from_json = JsonConvert.DeserializeObject<List<Tour>>(json);

            string image_path = string.Empty;
            try
            {
                foreach (Tour t in tours_from_json)
                {
                    //überprüfen ob die tour schon in der Datenbank exisitert
                    if (!tourItemDatabase.DoesTourExistInDb(t.UUID))
                    {
                        //Request schicken um Foto zu bekommen.
                        image_path = tourItemFileSystem.SaveImage(t.From, t.To);
                        //image path in externe txt datei speichern für das spätere löschen
                        tourItemFileSystem.SaveImagePath(image_path);

                        //tour in die Datenbankeinfügen
                        if (!tourItemDatabase.AddTour(t.UUID, t.Name, t.From, t.To, image_path, t.Description, t.Route_Type))
                        {
                            return false;
                        }

                        //logs in die db einfügen
                        foreach (Log l in t.LogItems)
                        {
                            if (!tourItemDatabase.AddLog(t, l.Date_Time, l.Distance, l.TotalTime, l.Report, l.Rating, l.AvarageSpeed, l.Comment, l.Problems, l.TransportModus, l.Recomended))
                            {
                                return false;
                            }

                        }

                    }
                }
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
           
        }
    }
}