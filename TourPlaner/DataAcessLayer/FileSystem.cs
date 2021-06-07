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
using Aspose.Pdf.Text;
using Aspose.Pdf;
using Page = ceTe.DynamicPDF.Page;
using Image = ceTe.DynamicPDF.PageElements.Image;

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
            string json_path = "config_file.json";
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
                ceTe.DynamicPDF.Document document = new ceTe.DynamicPDF.Document();

                Page page = new Page(ceTe.DynamicPDF.PageSize.Letter, PageOrientation.Portrait, 54.0f);
                document.Pages.Add(page);

                string labelText = " " + current_tour.Name;
                Label label = new Label(labelText, 0, 0, 504, 100, ceTe.DynamicPDF.Font.Helvetica, 18, TextAlign.Center);
                page.Elements.Add(label);


                string labelText2 = "#" + current_tour.UUID;
                Label label2 = new Label(labelText2, 0, 25, 504, 100, ceTe.DynamicPDF.Font.Helvetica, 18, TextAlign.Center);
                page.Elements.Add(label2);

                string labelText3 = "_________________________________________________";
                Label label3 = new Label(labelText3, 0, 50, 504, 100, ceTe.DynamicPDF.Font.Helvetica, 18, TextAlign.Center);
                page.Elements.Add(label3);


                string labelText4 = "From " + current_tour.From;
                Label label4 = new Label(labelText4, 0, 75, 504, 100, ceTe.DynamicPDF.Font.Helvetica, 14, TextAlign.Center);
                page.Elements.Add(label4);


                string labelText5 = "to " + current_tour.To;
                Label label5 = new Label(labelText5, 0, 100, 504, 100, ceTe.DynamicPDF.Font.Helvetica, 14, TextAlign.Center);
                page.Elements.Add(label5);


                string labelText6 = "____________________";
                Label label6 = new Label(labelText6, 0, 110, 504, 100, ceTe.DynamicPDF.Font.Helvetica, 18, TextAlign.Center);
                page.Elements.Add(label6);


                Image image = new Image(current_tour.PicPath, 115, 504);
                page.Elements.Add(image);

                int i = 1;
                string string_schleife = "";
                Label label_schleife;

                if(current_tour.LogItems != null)
                {
                    foreach (Log log in current_tour.LogItems)
                    {
                        int y = 130;

                        Page page2 = new Page(ceTe.DynamicPDF.PageSize.Letter, PageOrientation.Portrait, 54.0f);
                        document.Pages.Add(page2);

                        string_schleife = "Log " + i;
                        label_schleife = new Label(string_schleife, 0, y, 504, 100, ceTe.DynamicPDF.Font.Helvetica, 14, TextAlign.Left);
                        page2.Elements.Add(label_schleife);
                        y += 10;

                        string_schleife = "_________";
                        label_schleife = new Label(string_schleife, 0, y, 504, 100, ceTe.DynamicPDF.Font.Helvetica, 14, TextAlign.Left);
                        page2.Elements.Add(label_schleife);
                        y += 20;

                        string_schleife = "UUID: " + log.UUID;
                        label_schleife = new Label(string_schleife, 0, y, 504, 100, ceTe.DynamicPDF.Font.Helvetica, 14, TextAlign.Left);
                        page2.Elements.Add(label_schleife);
                        y += 20;

                        string_schleife = "Date: " + log.Date_Time;
                        label_schleife = new Label(string_schleife, 0, y, 504, 100, ceTe.DynamicPDF.Font.Helvetica, 14, TextAlign.Left);
                        page2.Elements.Add(label_schleife);
                        y += 20;

                        string_schleife = "Distance: " + log.Distance;
                        label_schleife = new Label(string_schleife, 0, y, 504, 100, ceTe.DynamicPDF.Font.Helvetica, 14, TextAlign.Left);
                        page2.Elements.Add(label_schleife);
                        y += 20;

                        string_schleife = "Total Time: " + log.TotalTime;
                        label_schleife = new Label(string_schleife, 0, y, 504, 100, ceTe.DynamicPDF.Font.Helvetica, 14, TextAlign.Left);
                        page2.Elements.Add(label_schleife);
                        y += 20;

                        string_schleife = "Report: " + log.Report;
                        label_schleife = new Label(string_schleife, 0, y, 504, 100, ceTe.DynamicPDF.Font.Helvetica, 14, TextAlign.Left);
                        page2.Elements.Add(label_schleife);
                        y += 20;

                        string_schleife = "Rating: " + log.Rating;
                        label_schleife = new Label(string_schleife, 0, y, 504, 100, ceTe.DynamicPDF.Font.Helvetica, 14, TextAlign.Left);
                        page2.Elements.Add(label_schleife);
                        y += 20;

                        string_schleife = "Avarage Speed: " + log.AvarageSpeed;
                        label_schleife = new Label(string_schleife, 0, y, 504, 100, ceTe.DynamicPDF.Font.Helvetica, 14, TextAlign.Left);
                        page2.Elements.Add(label_schleife);
                        y += 20;

                        string_schleife = "Comment: " + log.Comment;
                        label_schleife = new Label(string_schleife, 0, y, 504, 100, ceTe.DynamicPDF.Font.Helvetica, 14, TextAlign.Left);
                        page2.Elements.Add(label_schleife);
                        y += 20;

                        string_schleife = "Problems: " + log.Problems;
                        label_schleife = new Label(string_schleife, 0, y, 504, 100, ceTe.DynamicPDF.Font.Helvetica, 14, TextAlign.Left);
                        page2.Elements.Add(label_schleife);
                        y += 20;

                        string_schleife = "Transport Mode: " + log.TransportModus;
                        label_schleife = new Label(string_schleife, 0, y, 504, 100, ceTe.DynamicPDF.Font.Helvetica, 14, TextAlign.Left);
                        page2.Elements.Add(label_schleife);
                        y += 20;

                        string_schleife = "Recomendation " + log.Recomended;
                        label_schleife = new Label(string_schleife, 0, y, 504, 100, ceTe.DynamicPDF.Font.Helvetica, 14, TextAlign.Left);
                        page2.Elements.Add(label_schleife);
                        y += 20;

                        string_schleife = "______________________________________________________________";
                        label_schleife = new Label(string_schleife, 0, y, 504, 100, ceTe.DynamicPDF.Font.Helvetica, 14, TextAlign.Left);
                        page2.Elements.Add(label_schleife);
                        y += 20;

                        i++;
                    }
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

        public bool MakeReport(List <Tour> tours)
        {
            try
            {
                double total_tours = 0;
                double total_logs = 0;

                double total_needed_time = 0;
                double total_distance = 0;

                int has_tunnels = 0;
                int has_no_tunnels = 0;

                int has_highway = 0;
                int has_no_highway = 0;

                int has_bridge = 0;
                int has_no_bridge = 0;

                int has_access_restriction = 0;
                int has_no_restriction = 0;


                foreach(Tour t in tours)
                {
                    total_tours += 1;

                    //die logs der tour zum totalen hinzufügen
                    if(t.LogItems != null)
                    {
                        foreach (Log l in t.LogItems)
                        {
                            total_logs += 1;
                        }
                    }
                    



                    //die zeit aus der db splitten um sekunden,minuten,stunden und tage zu erhalten
                    string[] time = t.TourInfo.Time.Split(':');

                    double secondes = Convert.ToDouble(time[3]);

                    double minutes = Convert.ToDouble(time[2]);

                    double hours = Convert.ToDouble(time[1]);

                    double days = Convert.ToDouble(time[0]);

                    //die angegebene zeit in sekunden umrechnen
                    double total_seconds = secondes + minutes * 60;
                    total_seconds += hours * 60 * 60;
                    total_seconds += days * 24 * 60 * 60;


                    // die sekunden in stunden umberechnen
                    double needed_time = total_seconds / 60 / 60;

                    //auf das totale dazu addieren
                    total_needed_time += needed_time;

                    //distanz jeder tour auf die totale dazu addieren
                    string distance = t.TourInfo.Distance.Replace("km", "");

                    total_distance =Convert.ToDouble(distance);

                    //Has tunnels
                    if(t.TourInfo.HasTunnels)
                    {
                        has_tunnels += 1;
                    }
                    else
                    {
                        has_no_tunnels += 1;
                    }

                    //Has Highway
                    if (t.TourInfo.HasHighways)
                    {
                        has_highway += 1;
                    }
                    else
                    {
                        has_no_highway += 1;
                    }

                    //Has Bridge
                    if (t.TourInfo.HasBridge)
                    {
                        has_bridge += 1;
                    }
                    else
                    {
                        has_no_bridge += 1;
                    }

                    //Has Acces Restriction
                    if(t.TourInfo.HasAccesRestriction)
                    {
                        has_access_restriction += 1;
                    }
                    else
                    {
                        has_no_restriction += 1;
                    }
                }

                //PDF generator
                Aspose.Pdf.Document document = new Aspose.Pdf.Document();

                Aspose.Pdf.Page page = document.Pages.Add();

                // Add Header
                var header = new TextFragment("Tours Summary");
                header.TextState.Font = FontRepository.FindFont("Arial");
                header.TextState.FontSize = 24;
                header.HorizontalAlignment = HorizontalAlignment.Center;
                header.Position = new Position(130, 720);
                page.Paragraphs.Add(header);

                // Add description
                var descriptionText = "This document serves to provide a rough overview of a total of important information of the tours";
                var description = new TextFragment(descriptionText);
                description.TextState.Font = FontRepository.FindFont("Times New Roman");
                description.TextState.FontSize = 14;
                description.HorizontalAlignment = HorizontalAlignment.Center;
                page.Paragraphs.Add(description);


                //Labels of the information 
                var info_text = "\n__________________________________________________________________________________________________________________________________________\n\n" +
                    "Total number of Tours is " + total_tours +"\nTotal number of Logs is " + total_logs
                    + "\nThe total distance covered is " + total_distance + " km\nThe total past time is "+total_needed_time +
                    " hours\n\n";
                var info = new TextFragment(info_text);
                info.TextState.Font = FontRepository.FindFont("Times New Roman");
                info.TextState.FontSize = 12;
                info.HorizontalAlignment = HorizontalAlignment.Left;
                info.TextState.ForegroundColor = Aspose.Pdf.Color.Black;
                page.Paragraphs.Add(info);


                //Tabelle erstellen
                var table = new Aspose.Pdf.Table
                {
                    ColumnWidths = "100",
                    Border = new BorderInfo(BorderSide.Box, 1f, Aspose.Pdf.Color.DarkSlateGray),
                    DefaultCellBorder = new BorderInfo(BorderSide.Box, 0.5f, Aspose.Pdf.Color.Black),
                    DefaultCellPadding = new MarginInfo(4.5, 4.5, 4.5, 4.5),
                    Margin =
                {
                    Bottom = 10
                },
                    DefaultCellTextState =
                {
                    Font =  FontRepository.FindFont("Helvetica")
                }
                };

                var headerRow = table.Rows.Add();
                headerRow.Cells.Add("");
                headerRow.Cells.Add("Yes");
                headerRow.Cells.Add("No");
                

                foreach (Aspose.Pdf.Cell headerRowCell in headerRow.Cells)
                {
                    headerRowCell.BackgroundColor = Aspose.Pdf.Color.Gray;
                    headerRowCell.DefaultCellTextState.ForegroundColor = Aspose.Pdf.Color.WhiteSmoke;
                }



              
                var dataRow = table.Rows.Add();               
                dataRow.Cells.Add("Has Tunnels");
                dataRow.Cells.Add(Convert.ToString(has_tunnels));
                dataRow.Cells.Add(Convert.ToString(has_no_tunnels));

                var dataRow2 = table.Rows.Add();
                dataRow2.Cells.Add("Has Highways");
                dataRow2.Cells.Add(Convert.ToString(has_highway));
                dataRow2.Cells.Add(Convert.ToString(has_no_highway));

                var dataRow3 = table.Rows.Add();
                dataRow3.Cells.Add("Has Bridge");
                dataRow3.Cells.Add(Convert.ToString(has_bridge));
                dataRow3.Cells.Add(Convert.ToString(has_no_bridge));

                var dataRow4 = table.Rows.Add();
                dataRow4.Cells.Add("Has Access restriction");
                dataRow4.Cells.Add(Convert.ToString(has_access_restriction));
                dataRow4.Cells.Add(Convert.ToString(has_no_restriction));


                page.Paragraphs.Add(table);

                document.Save(this.to_pdf + "Summary.pdf");

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

        public Tour GetNewTour(string tour_id)
        {
            throw new NotImplementedException();
        }
    }
}
