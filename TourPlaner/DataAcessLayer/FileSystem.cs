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

namespace TourPlaner.DataAcessLayer
{
    class FileSystem : IDataAcess
    {
        public string save_path;
        public string delete_path;
        public string key;
        private ConfigFile config_file;
        public FileSystem()
        {
            
            string json_path = "C:\\Users\\titto\\Desktop\\Studium\\4.Semester\\Swe2\\TourPlaner\\TourPlaner\\config_file.json";
            string json = File.ReadAllText(json_path);
            this.config_file = JsonConvert.DeserializeObject<ConfigFile>(json);
            this.save_path = config_file.RouteImageSettings.Location;
            this.delete_path = config_file.ToDeletePath.Path;
            this.key = config_file.RequestKey.Key;
        }
        public void AddTourAsync(String UUID, string name,string from, string to, string pic_path, string description, string route_Type)
        {
            throw new NotImplementedException();
        }
        
        public string SaveImage(string from, string to)
        {   //HTTP CLient verwenden (singleton)
          
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

        public bool DeleteImages( )
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
    }
}
