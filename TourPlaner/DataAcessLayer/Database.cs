using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TourPlaner.Models;
using TourPlaner.Models.JsonObjects;

namespace TourPlaner.DataAcessLayer
{
    class Database : IDataAcess
    {

        private ConfigFile config_file;

        private string key;

        private List<Tour> tourItems = new List<Tour>();/*
        {
            new Tour() {Name = "Wien"},
            new Tour() {Name = "Salzbutg"},
            new Tour() {Name = "Kairo"},
            new Tour() {Name = "Atlanta"}
        };*/

        public Database()
        {
            //connection string to the database
            string json_path = "C:\\Users\\titto\\Desktop\\Studium\\4.Semester\\Swe2\\TourPlaner\\TourPlaner\\config_file.json";
            string json = File.ReadAllText(json_path);
            this.config_file = JsonConvert.DeserializeObject<ConfigFile>(json);
            this.key = config_file.RequestKey.Key;
        }

        public bool AddLog(Tour current_tour, string date_Time, double distance, double totalTime, string report)
        {
            int index = tourItems.FindIndex(a => a == current_tour);
            Log tmp = new Log()
            {
                Date_Time = date_Time,
                Distance = distance,
                TotalTime = totalTime,
                Report = report
            };
            tourItems[index].SetLog(tmp);
            return true;
            
        }

        private string PutInfosInDescription(string description,string from, string to, RouteInformation route_info)
        {
            description += "Route Description: \n";
            
            TimeSpan time = TimeSpan.FromSeconds(route_info.time);
            //here backslash is must to tell that colon is
            //not the part of format, it just a character that we want in output
            string str = time.ToString(@"dd\:hh\:mm\:ss");
            description += "    From: " + from + "\n";
            description += "    To: " + to + "\n";
            description += "    Distance: " + route_info.distance + " km\n";
            description += "    Time: " + str +"\n";
            description += "    Has Tunnels: " + route_info.hasTunnel + "\n";
            description += "    Has Highways: " + route_info.hasHighway + "\n";
            description += "    Has Bridge: " + route_info.hasBridge + "\n";
            description += "    Has Acces Restriction: " + route_info.hasAccessRestriction + "\n";
            return description;
        }

        public void AddTourAsync(string name,string from, string to, string pic_path, string description, string route_type)
        {
            string url = @"http://www.mapquestapi.com/directions/v2/route?key=" + key + "&from=" + from + "&to=" + to + "&routeType=" + route_type;
            
            //damit am anfang steht user description
            string tmp = "User Description: \n";
            tmp += description + "\n\n\n\n";
            description = tmp;
            ///

            try
            {
                using (var webClient = new System.Net.WebClient())
                {
                    var json = webClient.DownloadString(url);
                    ReqObj req_obj = JsonConvert.DeserializeObject<ReqObj>(json);


                    req_obj.route.distance = Math.Round(req_obj.route.distance * 1.60934, 2);

                    description = PutInfosInDescription(description, from, to, req_obj.route);

                    tourItems.Add(new Tour()
                    {
                        Name = name,
                        From = from,
                        To = to,
                        PicPath = pic_path,
                        Description = description

                    }
                    );
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            };
                      
        }

        public bool DeleteImages(string name)
        {
            throw new NotImplementedException();
        }

        public bool DeleteImages()
        {
            throw new NotImplementedException();
        }

        public bool DeleteTour(string name)
        {
            tourItems.RemoveAt(tourItems.FindIndex(a => a.Name == name));
            return true;
        }

        public List<Tour> GetTours()
        {
            return tourItems;
        }

        public string SaveImage(string from, string to)
        {
            throw new NotImplementedException();
        }

        public void SaveImagePath(string path)
        {
            throw new NotImplementedException();
        }
    }
}
