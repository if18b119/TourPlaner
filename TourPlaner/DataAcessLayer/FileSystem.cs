using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TourPlaner.Models;
using ceTe.DynamicPDF;
using ceTe.DynamicPDF.PageElements;


namespace TourPlaner.DataAcessLayer
{
    class FileSystem : IDataAcess
    {
        //Logging -Instanz
        private static readonly log4net.ILog log = 
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public string save_path;
        public string delete_path;
        public string key;
        public string to_pdf;
        public string export_path;
        private ConfigFile config_file;
        public FileSystem()
        {
            //here we get the important information from the config file that we need in our filesystem
            string json_path = "C:\\Users\\titto\\Desktop\\Studium\\4.Semester\\Swe2\\TourPlaner\\TourPlaner\\config_file.json";
            string json = File.ReadAllText(json_path);
            this.config_file = JsonConvert.DeserializeObject<ConfigFile>(json);
            this.save_path = config_file.RouteImageSettings.Location;
            this.delete_path = config_file.ToDeletePath.Path;
            this.key = config_file.RequestKey.Key;
            this.to_pdf = config_file.GeneratePdf.Path;
            this.export_path = config_file.ExportTours.Path;
        }
        public void AddTourAsync(String UUID, string name,string from, string to, string pic_path, string description, string route_Type)
        {
            throw new NotImplementedException();
        }
        
        public string SaveImage(string from, string to)
        {   //HTTP CLient verwenden (singleton)
          
            try
            {
                string picName;
                string url = @"https://www.mapquestapi.com/staticmap/v5/map?start=" + from + "&end=" + to + "&size=600,400@2x&key=" + key;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;

                using (HttpWebResponse lxResponse = (HttpWebResponse)request.GetResponse())
                {
                    using (BinaryReader reader = new BinaryReader(lxResponse.GetResponseStream()))
                    {
                        //UUID
                        Byte[] lnByte = reader.ReadBytes(1 * 1024 * 1024 * 10);
                        Random rand2 = new Random();
                        //Thread.Sleep(200);
                        picName = Convert.ToString(rand2.Next(999999));
                        picName += ".jpg";
                        using (FileStream lxFS = new FileStream(save_path + picName, FileMode.Create))
                        {
                            lxFS.Write(lnByte, 0, lnByte.Length);
                        }
                    }

                }

                return save_path + picName;
            }
            catch(Exception e)
            {
                string exception = "{\"errorMessages\":[\"" + e.Message.ToString() + "\"],\"errors\":{}}";
                log.Error(exception, e);
                return string.Empty;
            }
            
        }

        public bool DeleteImages( )
        {
            try
            {
                string json = File.ReadAllText(delete_path);
                List<ImageToBeDeleted> images = JsonConvert.DeserializeObject<List<ImageToBeDeleted>>(json);
                if (images != null)
                {
                    foreach (ImageToBeDeleted image in images)
                    {
                        File.Delete(image.path);
                    }
                    //Jason file wird entleert
                    File.WriteAllText(delete_path, String.Empty);
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

        public List<Tour> GetTours()
        {
            throw new NotImplementedException();
        }

        public bool AddLog(Tour current_tour, string date_Time, string distance, string totalTime, string report, string rating, string avarage_speed, string comment, string problems, string transport_modus, string recomended)
        {
            throw new NotImplementedException();
        }

        public void SaveImagePath(string _path)
        {
            try
            {
                string initialJson = File.ReadAllText(delete_path);
                var list = JsonConvert.DeserializeObject<List<ImageToBeDeleted>>(initialJson);
                ImageToBeDeleted image = new ImageToBeDeleted() { path = _path };
                if (list == null)
                {
                    list = new List<ImageToBeDeleted>();
                }
                list.Add(image);

                string output = JsonConvert.SerializeObject(list, Formatting.Indented);
                File.WriteAllText(delete_path, output);
            }
            catch(Exception e)
            {
                string exception = "{\"errorMessages\":[\"" + e.Message.ToString() + "\"],\"errors\":{}}";
                log.Error(exception, e);
            }
           

        }

        public bool DeleteTour(string name)
        {
            throw new NotImplementedException();
        }

        public List<Tour> GetTourLogs(string UUID)
        {
            throw new NotImplementedException();
        }

        ObservableCollection<Log> IDataAcess.GetTourLogs(string UUID)
        {
            throw new NotImplementedException();
        }

        public bool UpdateLogValue(string tour_id, string log_id, string to_update_column, string new_value)
        {
            throw new NotImplementedException();
        }

        public Log GetNewLog(string tour_id, string log_id)
        {
            throw new NotImplementedException();
        }

        public bool DeleteLog(string tour_id, string log_id)
        {
            throw new NotImplementedException();
        }

        public bool MakePdf(Tour current_tour)
        {
            try
            {
                //PDF generator
                Document document = new Document();

                Page page = new Page(PageSize.Letter, PageOrientation.Portrait, 54.0f);
                document.Pages.Add(page);

                string labelText = " " + current_tour.Name;
                Label label = new Label(labelText, 0, 0, 504, 100, Font.Helvetica, 18, TextAlign.Center);
                page.Elements.Add(label);


                string labelText2 = "#" + current_tour.UUID;
                Label label2 = new Label(labelText2, 0, 25, 504, 100, Font.Helvetica, 18, TextAlign.Center);
                page.Elements.Add(label2);

                string labelText3 = "_________________________________________________";
                Label label3 = new Label(labelText3, 0, 50, 504, 100, Font.Helvetica, 18, TextAlign.Center);
                page.Elements.Add(label3);


                string labelText4 = "From " + current_tour.From;
                Label label4 = new Label(labelText4, 0, 75, 504, 100, Font.Helvetica, 14, TextAlign.Center);
                page.Elements.Add(label4);


                string labelText5 = "to " + current_tour.To;
                Label label5 = new Label(labelText5, 0, 100, 504, 100, Font.Helvetica, 14, TextAlign.Center);
                page.Elements.Add(label5);


                string labelText6 = "____________________";
                Label label6 = new Label(labelText6, 0, 110, 504, 100, Font.Helvetica, 18, TextAlign.Center);
                page.Elements.Add(label6);


                int i = 1;
                string string_schleife = "";
                Label label_schleife;

                foreach (Log log in current_tour.LogItems)
                {
                    int y = 130;

                    Page page2 = new Page(PageSize.Letter, PageOrientation.Portrait, 54.0f);
                    document.Pages.Add(page2);

                    string_schleife = "Log " + i;
                    label_schleife = new Label(string_schleife, 0, y, 504, 100, Font.Helvetica, 14, TextAlign.Left);
                    page2.Elements.Add(label_schleife);
                    y += 10;

                    string_schleife = "_________";
                    label_schleife = new Label(string_schleife, 0, y, 504, 100, Font.Helvetica, 14, TextAlign.Left);
                    page2.Elements.Add(label_schleife);
                    y += 20;

                    string_schleife = "UUID: " + log.UUID;
                    label_schleife = new Label(string_schleife, 0, y, 504, 100, Font.Helvetica, 14, TextAlign.Left);
                    page2.Elements.Add(label_schleife);
                    y += 20;

                    string_schleife = "Date: " + log.Date_Time;
                    label_schleife = new Label(string_schleife, 0, y, 504, 100, Font.Helvetica, 14, TextAlign.Left);
                    page2.Elements.Add(label_schleife);
                    y += 20;

                    string_schleife = "Distance: " + log.Distance;
                    label_schleife = new Label(string_schleife, 0, y, 504, 100, Font.Helvetica, 14, TextAlign.Left);
                    page2.Elements.Add(label_schleife);
                    y += 20;

                    string_schleife = "Total Time: " + log.TotalTime;
                    label_schleife = new Label(string_schleife, 0, y, 504, 100, Font.Helvetica, 14, TextAlign.Left);
                    page2.Elements.Add(label_schleife);
                    y += 20;

                    string_schleife = "Report: " + log.Report;
                    label_schleife = new Label(string_schleife, 0, y, 504, 100, Font.Helvetica, 14, TextAlign.Left);
                    page2.Elements.Add(label_schleife);
                    y += 20;

                    string_schleife = "Rating: " + log.Rating;
                    label_schleife = new Label(string_schleife, 0, y, 504, 100, Font.Helvetica, 14, TextAlign.Left);
                    page2.Elements.Add(label_schleife);
                    y += 20;

                    string_schleife = "Avarage Speed: " + log.AvarageSpeed;
                    label_schleife = new Label(string_schleife, 0, y, 504, 100, Font.Helvetica, 14, TextAlign.Left);
                    page2.Elements.Add(label_schleife);
                    y += 20;

                    string_schleife = "Comment: " + log.Comment;
                    label_schleife = new Label(string_schleife, 0, y, 504, 100, Font.Helvetica, 14, TextAlign.Left);
                    page2.Elements.Add(label_schleife);
                    y += 20;

                    string_schleife = "Problems: " + log.Problems;
                    label_schleife = new Label(string_schleife, 0, y, 504, 100, Font.Helvetica, 14, TextAlign.Left);
                    page2.Elements.Add(label_schleife);
                    y += 20;

                    string_schleife = "Transport Mode: " + log.TransportModus;
                    label_schleife = new Label(string_schleife, 0, y, 504, 100, Font.Helvetica, 14, TextAlign.Left);
                    page2.Elements.Add(label_schleife);
                    y += 20;

                    string_schleife = "Recomendation " + log.Recomended;
                    label_schleife = new Label(string_schleife, 0, y, 504, 100, Font.Helvetica, 14, TextAlign.Left);
                    page2.Elements.Add(label_schleife);
                    y += 20;

                    string_schleife = "______________________________________________________________";
                    label_schleife = new Label(string_schleife, 0, y, 504, 100, Font.Helvetica, 14, TextAlign.Left);
                    page2.Elements.Add(label_schleife);
                    y += 20;

                    i++;
                }


                document.Draw(this.to_pdf + current_tour.Name + "_" + current_tour.UUID + ".pdf");
                return true;
            }
            catch (Exception e)
            {
                string exception = "{\"errorMessages\":[\"" + e.Message.ToString() + "\"],\"errors\":{}}";
                log.Error(exception, e);
                return false;
            }
        }

        public bool Export(List<Tour> current_tours_in_DB)
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.NullValueHandling = NullValueHandling.Ignore;
                String UUID = Guid.NewGuid().ToString();
                using (StreamWriter sw = new StreamWriter(export_path + "export_" + UUID + ".json"))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {

                    serializer.Serialize(writer, current_tours_in_DB);
                }
                return true;
            }
            catch(Exception e )
            {
                string exception = "{\"errorMessages\":[\"" + e.Message.ToString() + "\"],\"errors\":{}}";
                log.Error(exception, e);
                return false;
            }
           
        }

        public bool DoesTourExistInDb(string tour_id)
        {
            throw new NotImplementedException();
        }

        public bool DeleteAllTour()
        {
            throw new NotImplementedException();
        }
    }
}
