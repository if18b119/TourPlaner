
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
        //Logging -Instanz
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

        public bool UpdateTourValue(string tour_id, string to_update_column, string new_value)
        {
            try
            {

                ObservableCollection<Tour> tmp_tours = new ObservableCollection<Tour>();
                //Die gesamten informationen der Tour aus der db holen
                foreach (Tour t in tourItemDatabase.GetTours())
                {
                   tmp_tours.Add(t);
                }

                Tour tour_to_change = tmp_tours.Where(x => x.UUID == tour_id).First();
                if (tour_to_change != null)
                {
                    switch (to_update_column)
                    {
                        case "b0":
                            tour_to_change.Name = new_value;
                            break;
                        case "b1":
                            tour_to_change.From = new_value;
                            break;
                        case "b2":
                            tour_to_change.To = new_value;
                            break;
                        case "b4":
                            tour_to_change.Description = new_value;
                            break;
                        default:
                            return false;

                    }
                    //Values speichern um einen neuen Request zu schicken
                    string uuid = tour_to_change.UUID;
                    string name = tour_to_change.Name;
                    string from = tour_to_change.From;
                    string to = tour_to_change.To;
                    string route_type = tour_to_change.Route_Type;
                    string desc = tour_to_change.Description;

                    //Die Tour aus der Datenbank löschen und das Foto
                    SavePathAndDeleteTour(tour_to_change);

                    //Neuen Request schicken und tour speichern.
                    AddTour(uuid, name, from, to, desc, route_type);
                    return true;
                }
                else
                {
                    throw new Exception("Keine Tour gefunden-[uPdate]");
                }
                    
                    

                
            }
            catch(Exception e)
            {
                string exception = "{\"errorMessages\":[\"" + e.Message.ToString() + "\"],\"errors\":{}}";
                log.Error(exception, e);
                return false;
            }

        }

        public Log GetNewLog(string tour_id, string log_id)
        {
            return tourItemDatabase.GetNewLog(tour_id, log_id);
        }
        public Tour GetNewTour(string tour_id)
        {
            return tourItemDatabase.GetNewTour(tour_id);
        }

        public bool DeleteLog(string tour_id, string log_id)
        {
            return tourItemDatabase.DeleteLog(tour_id, log_id);
        }

        public bool MakePdf(Tour current_tour)
        {
            return tourItemFileSystem.MakePdf(current_tour);
        }

        public bool MakeReport()
        {
            List<Tour> tours = tourItemDatabase.GetTours();
            return tourItemFileSystem.MakeReport(tours);
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

        public bool Paste(Tour to_copy)
        {
            try
            {
                if(to_copy != null)
                {
                    string pic_path = tourItemFileSystem.SaveImage(to_copy.From, to_copy.To);

                    //Um sie vom originalen zu unterscheiden.
                    to_copy.UUID += "-Copy";
                    to_copy.Name += " Copy";

                    //Dadurch dass beim adden einer tour auf die id geprüft werden kann nicht eine tour mehrmals kopiert werden, aber die kopie kann geupdatet werden
                    //wenn man versucht eine bereits kopierte tour zu kopieren, wobei die tour neue daten enthält (logs);
                    tourItemDatabase.AddTour(to_copy.UUID, to_copy.Name, to_copy.From, to_copy.To, pic_path, to_copy.Description, to_copy.Route_Type);


                    if (to_copy.LogItems.Count > 0)
                    {
                        foreach (Log l in to_copy.LogItems)
                        {
                            tourItemDatabase.AddLog(to_copy, l.Date_Time, l.Distance, l.TotalTime, l.Report, l.Rating, l.AvarageSpeed, l.Comment, l.Problems, l.TransportModus, l.Recomended);
                        }
                    }
                    //für die logs


                    return true;
                }
                else
                {
                    throw new Exception("No Tour to copy!");
                }
               
            }
            catch(Exception e)
            {
                string exception = "{\"errorMessages\":[\"" + e.Message.ToString() + "\"],\"errors\":{}}";
                log.Error(exception, e);
                return false;
            }
            
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

                        if(t.LogItems != null && t.LogItems.Count != 0)
                        {
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
                }
                return true;
            }
            catch(Exception e)
            {
                string exception = "{\"errorMessages\":[\"" + e.Message.ToString() + "\"],\"errors\":{}}";
                log.Error(exception, e);
                return false;
            }
           
        }

        public bool ImportAndDelete(string file_name)
        {
            //Die liste an tours von dem json file deserializen
            List<Tour> tours_from_json = new List<Tour>();
            string json_path = file_name;
            string json = File.ReadAllText(json_path);
            tours_from_json = JsonConvert.DeserializeObject<List<Tour>>(json);

            string image_path = string.Empty;
            try
            {
                
                while(true)
                {
                    
                   if(tourItemDatabase.GetTours().Count != 0)
                    {
                        Tour t = tourItemDatabase.GetTours()[0];
                        SavePathAndDeleteTour(t);
                    }
                    else
                    {
                        break;
                    }
                   
                }
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

                        if (t.LogItems != null && t.LogItems.Count != 0)
                        {
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
                }
                return true;
            }
            catch (Exception e)
            {
                string exception = "{\"errorMessages\":[\"" + e.Message.ToString() + "\"],\"errors\":{}}";
                log.Error(exception, e);
                return false;
            }
        }
    }
}