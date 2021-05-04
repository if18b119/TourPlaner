using Newtonsoft.Json;
using Npgsql;
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
        private string connection_string;

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
            string json = File.ReadAllText(json_path); //den text in der json datei auslesen
            this.config_file = JsonConvert.DeserializeObject<ConfigFile>(json);  //aus dem text ein Object erstellén
            this.key = config_file.RequestKey.Key; //MapQuest Key
            this.connection_string = "Host=" + this.config_file.DbSettings.Host + ";Username=" + this.config_file.DbSettings.Username +"; Password="+
                         this.config_file.DbSettings.Password + ";Database=" + this.config_file.DbSettings.Database + ";Port=" + this.config_file.DbSettings.Port;
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

        //um das tour info string aus dem tour info objekt zubasteln
        private string CreateTourInfoString(Tour t)
        {
            return t.TourInfoString = "From: " + t.TourInfo.From + "\nTo: " + t.TourInfo.To + "\nDistance: " + t.TourInfo.Distance
                   + "\nTime: " + t.TourInfo.Time + "\nHas Tunnels: " + t.TourInfo.HasTunnels + "\nHas Highways: " + t.TourInfo.HasHighways
                   + "\nHas Bridges: " + t.TourInfo.HasBridge + "\nHas Acces Restriction: " + t.TourInfo.HasAccesRestriction;
        }
       
        //erstellt das TourInfo objekt aus der db tabelle und gibt es dem Tour objekt
        private TourInfo PutInfosInDescription(string tourid, string from, string to, Tour t)
        {
            string distance = string.Empty;
            string time = string.Empty;
            bool has_tunnels;
            bool has_highways;
            bool has_bridge;
            bool has_acces_restriction;


            NpgsqlConnection conn = new NpgsqlConnection(connection_string);
            conn.Open();
            string strtours = "Select * from tourinfos where tour_id = @tour_id";
            NpgsqlCommand sqlcomm = new NpgsqlCommand(strtours, conn);
            sqlcomm.Parameters.AddWithValue("tour_id", tourid);
            sqlcomm.Prepare();

            NpgsqlDataReader reader = sqlcomm.ExecuteReader();
            reader.Read();
            distance = reader.GetString(1);
            time = reader.GetString(2);
            has_tunnels = reader.GetBoolean(3);
            has_highways = reader.GetBoolean(4);
            has_bridge = reader.GetBoolean(5);
            has_acces_restriction = reader.GetBoolean(6);                          
            conn.Close();




           
            return new TourInfo()
            {
                From = from,
                To = to,
                Distance = distance,
                Time = time,
                HasTunnels = has_tunnels,
                HasHighways = has_highways,
                HasAccesRestriction = has_acces_restriction,
                HasBridge = has_bridge
            };
           
        }
        private bool AddTourInfoToDB(string tour_id, string distance, string time, bool has_tunnels, bool has_highways, bool has_bridge, bool has_acces_restriction)
        {
            try
            {
                var con = new NpgsqlConnection(connection_string);
                con.Open();
                var sql_insert = "Insert into tourinfos (tour_id, distance, time, has_tunnels, has_highways, has_bridge, has_acces_restriction) values (@tour_id, @distance, @time, @has_tunnels, @has_highways, @has_bridge, @has_acces_restriction)";
                var cmd = new NpgsqlCommand(sql_insert, con);
                //prepared statment
                cmd.Parameters.AddWithValue("tour_id", tour_id);
                cmd.Parameters.AddWithValue("distance", distance);
                cmd.Parameters.AddWithValue("time", time);
                cmd.Parameters.AddWithValue("has_tunnels", has_tunnels);
                cmd.Parameters.AddWithValue("has_highways", has_highways);
                cmd.Parameters.AddWithValue("has_bridge", has_bridge);
                cmd.Parameters.AddWithValue("has_acces_restriction", has_acces_restriction);

                cmd.Prepare();
                //
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                return false;
            }


        }

        private string AddTourInfoToDbPrep(string tourid, string description,string from, string to, RouteInformation route_info)
        {
           
            
            TimeSpan time = TimeSpan.FromSeconds(route_info.time);
            //here backslash is must to tell that colon is
            //not the part of format, it just a character that we want in output
            string str = time.ToString(@"dd\:hh\:mm\:ss");
            

            //infos in die db
            if(!AddTourInfoToDB(tourid, Convert.ToString(route_info.distance)+"km", str, route_info.hasTunnel, route_info.hasHighway, route_info.hasBridge, route_info.hasAccessRestriction))
            {
                throw new Exception("Error: something wnt wrong adding the tour infos!");
            }
            return description;
        }

        public void AddTourAsync(String UUID, string name,string from, string to, string pic_path, string description, string route_type)
        {
            //Maquest Get req
            string url = @"http://www.mapquestapi.com/directions/v2/route?key=" + key + "&from=" + from + "&to=" + to + "&routeType=" + route_type;


            try
            {
                using (var webClient = new System.Net.WebClient())
                {
                    //Response des GEt Request deserializen
                    var json = webClient.DownloadString(url);
                    ReqObj req_obj = JsonConvert.DeserializeObject<ReqObj>(json);

                    //Distanz in km
                    req_obj.route.distance = Math.Round(req_obj.route.distance * 1.60934, 2);


                   


                    /*tourItems.Add(new Tour()
                    {
                        UUID = UUID,
                        Name = name,
                        From = from,
                        To = to,
                        PicPath = pic_path,
                        Description = description

                    }
                    );*/
                    //Insert in die Datenbank
                    var con = new NpgsqlConnection(connection_string);
                    con.Open();

                    //schauen ob die ID schon existiert
                    int count_username = 0;
                    var sql_count = "SELECT count (*) from tours where tour_id = @UUID";
                    var cmd = new NpgsqlCommand(sql_count, con);
                    //prepared statments
                    cmd.Parameters.AddWithValue("UUID", UUID);
                    cmd.Prepare();
                   
                    NpgsqlDataReader GetCount = cmd.ExecuteReader(); //curser

                    GetCount.Read();
                    count_username = GetCount.GetInt32(0);

                    GetCount.Close();


                    if (count_username == 0) //check if name is unique(no one else with the same name)
                    {
                        var sql_insert = "Insert into tours (tour_id, name, description, start, destination, route_type, path) values (@UUID,@name,@description,@start, @destination, @route_type, @pic_path)";
                        var cmd2 = new NpgsqlCommand(sql_insert, con);
                        //prepared statment
                        cmd2.Parameters.AddWithValue("UUID", UUID);
                        cmd2.Parameters.AddWithValue("name", name);
                        cmd2.Parameters.AddWithValue("description", description);
                        cmd2.Parameters.AddWithValue("start", from);
                        cmd2.Parameters.AddWithValue("destination", to);
                        cmd2.Parameters.AddWithValue("route_type", route_type);
                        cmd2.Parameters.AddWithValue("pic_path", pic_path);

                        cmd2.Prepare();
                        //
                        cmd2.ExecuteNonQuery();
                        
                       
                    }
                    else
                    {
                        throw new Exception("Error: ID already in use!");
                    }

                    description = AddTourInfoToDbPrep(UUID, description, from, to, req_obj.route);
                    //damit am anfang steht user description
                    string tmp = "User Description: \n";
                    tmp += description + "\n\n\n\n";


                    //für das anzeigen
                    description = tmp;
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

        public bool DeleteTour(string UUID)
        {
            tourItems.RemoveAt(tourItems.FindIndex(a => a.UUID == UUID));

            NpgsqlConnection conn = new NpgsqlConnection(connection_string);
            conn.Open();

            string strdelete = "Delete from tours where tour_id = @tour_id";
            NpgsqlCommand sqldelete = new NpgsqlCommand(strdelete, conn);

            sqldelete.Parameters.AddWithValue("tour_id", UUID);
            sqldelete.Prepare();
            sqldelete.ExecuteNonQuery();

            conn.Close();
            return true;
        }

        public List<Tour> GetTours()
        {
            tourItems.Clear();
            int i = 0;
            NpgsqlConnection conn = new NpgsqlConnection(connection_string);
            conn.Open();
            string strtours = "Select * from tours";
            NpgsqlCommand sqlcomm = new NpgsqlCommand(strtours, conn);
            sqlcomm.Prepare();

            NpgsqlDataReader reader = sqlcomm.ExecuteReader();
            while (reader.Read())
            {
                tourItems.Add(new Tour()
                {
                    UUID = reader.GetString(0),
                    Name = reader.GetString(1),
                    Description = reader.GetString(2),
                    From = reader.GetString(3),
                    To = reader.GetString(4),
                    Route_Type = reader.GetString(5),
                    PicPath = reader.GetString(6)
                }) ;
                //Die beschreibung des unsers mit der tour info aus dem request zusammentun für die anzeige
                tourItems[0].TourInfo = PutInfosInDescription(tourItems[0].UUID, tourItems[0].From, tourItems[0].To, tourItems[0]);

                //making the touinfo string für das anzeigen in der view
                tourItems[0].TourInfoString = CreateTourInfoString(tourItems[0]);
                i++;
            }
            conn.Close();

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
