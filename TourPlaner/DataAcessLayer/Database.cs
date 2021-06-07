using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        //Logging -Instanz
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
            string json_path = "config_file.json";
            string json = File.ReadAllText(json_path); //den text in der json datei auslesen
            this.config_file = JsonConvert.DeserializeObject<ConfigFile>(json);  //aus dem text ein Object erstellén
            this.key = config_file.RequestKey.Key; //MapQuest Key
            this.connection_string = "Host=" + this.config_file.DbSettings.Host + ";Username=" + this.config_file.DbSettings.Username +"; Password="+
                         this.config_file.DbSettings.Password + ";Database=" + this.config_file.DbSettings.Database + ";Port=" + this.config_file.DbSettings.Port;
        }

        public bool AddLog(Tour current_tour, string date_Time, string distance, string totalTime, string report, string rating, string avarage_speed, string comment, string problems, string transport_modus, string recomended)
        {
            /*current_tour.LogItems.Add(

                new Log()
                 {
                     Date_Time = date_Time,
                     Distance = distance,
                     TotalTime = totalTime,
                     Report = report,
                     Rating = rating,
                     AvarageSpeed=avarage_speed,
                     Comment = comment,
                     Problems=problems,
                     TransportModus=transport_modus,
                     Recomended=recomended
                 }
                 );*/
            String UUID = Guid.NewGuid().ToString();
            try
            {
                if(current_tour != null)
                {
                    var con = new NpgsqlConnection(connection_string);
                    con.Open();
                    var sql_insert = "Insert into logs (log_id, fk_tour_id, date_time, distance, total_time, report, rating, avarage_speed, comment, problems, transport_modus, recomended) values (@UUID, @fk_tour_id, @date_time, @distance, @total_time, @report, @rating, @avarage_speed, @comment, @problems, @transport_modus, @recomended)";
                    var cmd = new NpgsqlCommand(sql_insert, con);
                    //prepared statment
                    cmd.Parameters.AddWithValue("UUID", UUID);
                    cmd.Parameters.AddWithValue("fk_tour_id", current_tour.UUID);
                    cmd.Parameters.AddWithValue("date_time", date_Time);
                    cmd.Parameters.AddWithValue("distance", distance);
                    cmd.Parameters.AddWithValue("total_time", totalTime);
                    cmd.Parameters.AddWithValue("report", report);
                    cmd.Parameters.AddWithValue("rating", rating);
                    cmd.Parameters.AddWithValue("avarage_speed", avarage_speed);
                    cmd.Parameters.AddWithValue("comment", comment);
                    cmd.Parameters.AddWithValue("problems", problems);
                    cmd.Parameters.AddWithValue("transport_modus", transport_modus);
                    cmd.Parameters.AddWithValue("recomended", recomended);
                    cmd.Prepare();
                    //
                    cmd.ExecuteNonQuery();
                    return true;
                }
                else
                {
                    return false;
                }
               
            }
            catch (Exception e)
            {
                string exception = "{\"errorMessages\":[\"" + e.Message.ToString() + "\"],\"errors\":{}}";
                log.Error(exception, e);
                return false;
            }                  
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
                string exception = "{\"errorMessages\":[\"" + e.Message.ToString() + "\"],\"errors\":{}}";
                log.Error(exception, e);
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
                        return;
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
                string exception = "{\"errorMessages\":[\"" + e.Message.ToString() + "\"],\"errors\":{}}";
                log.Error(exception, e);
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
            NpgsqlConnection conn = new NpgsqlConnection(connection_string);
            conn.Open();
            try
            {
                tourItems.RemoveAt(tourItems.FindIndex(a => a.UUID == UUID));

                

                string strdelete = "Delete from tours where tour_id = @tour_id";
                NpgsqlCommand sqldelete = new NpgsqlCommand(strdelete, conn);

                sqldelete.Parameters.AddWithValue("tour_id", UUID);
                sqldelete.Prepare();
                sqldelete.ExecuteNonQuery();

                
                return true;
            }
            catch(Exception e)
            {
                string exception = "{\"errorMessages\":[\"" + e.Message.ToString() + "\"],\"errors\":{}}";
                log.Error(exception, e);
                
                return false;
            }
            finally
            {
                conn.Close();
            }
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
                tourItems[i].TourInfo = PutInfosInDescription(tourItems[i].UUID, tourItems[i].From, tourItems[i].To, tourItems[i]);

                //making the touinfo string für das anzeigen in der view
                tourItems[i].TourInfoString = CreateTourInfoString(tourItems[i]);
                i++;
            }
            conn.Close();

            return tourItems;
        }

        public ObservableCollection<Log> GetTourLogs(String UUID)
        {
            //Die tour in der localen tourliste mit dem index finden
            int index = tourItems.FindIndex(a => a.UUID == UUID);
            //ausleeren
            if (tourItems[index].LogItems != null)
                tourItems[index].LogItems.Clear();
            else
                tourItems[index].LogItems = new ObservableCollection<Log>();

            //neu befüllen von der DB
            NpgsqlConnection conn = new NpgsqlConnection(connection_string);
            conn.Open();
            string cmdtourlogs = "Select * from logs where fk_tour_id = @UUID";

            
            NpgsqlCommand cmd = new NpgsqlCommand(cmdtourlogs, conn);
            cmd.Parameters.AddWithValue("UUID", UUID);
            cmd.Prepare();
            cmd.Prepare();

            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                tourItems[index].LogItems.Add(new Log()
                {
                    UUID = reader.GetString(0),
                    Date_Time = reader.GetString(2),
                    Distance = reader.GetString(3),
                    TotalTime = reader.GetString(4),
                    Report = reader.GetString(5),
                    Rating = reader.GetString(6),
                    AvarageSpeed = reader.GetString(7),
                    Comment = reader.GetString(8),
                    Problems = reader.GetString(9),
                    TransportModus = reader.GetString(10),
                    Recomended = reader.GetString(11)
                });
                
                
            }
            conn.Close();

            return tourItems[index].LogItems;
        }

        public string SaveImage(string from, string to)
        {
            throw new NotImplementedException();
        }

        public void SaveImagePath(string path)
        {
            throw new NotImplementedException();
        }

        public bool UpdateLogValue(string tour_id, string log_id, string to_update_column,string new_value)
        {
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection(connection_string);
                conn.Open();
                string updatelogs;
                switch (to_update_column)
                {
                    case "b0":
                        updatelogs = "Update logs set date_time = @new_value where (log_id = @logid and fk_tour_id = @tour_id);";
                        NpgsqlCommand sqlupdate = new NpgsqlCommand(updatelogs, conn);
                        sqlupdate.Parameters.AddWithValue("new_value", new_value);
                        sqlupdate.Parameters.AddWithValue("logid", log_id);
                        sqlupdate.Parameters.AddWithValue("tour_id", tour_id);
                        sqlupdate.Prepare();
                        sqlupdate.ExecuteNonQuery();
                        conn.Close();
                        return true;
                    case "b1":
                        updatelogs = "Update logs set distance = @new_value where (log_id = @logid and fk_tour_id = @tour_id);";
                        NpgsqlCommand sqlupdate2 = new NpgsqlCommand(updatelogs, conn);
                        sqlupdate2.Parameters.AddWithValue("new_value", new_value);
                        sqlupdate2.Parameters.AddWithValue("logid", log_id);
                        sqlupdate2.Parameters.AddWithValue("tour_id", tour_id);
                        sqlupdate2.Prepare();
                        sqlupdate2.ExecuteNonQuery();
                        conn.Close();
                        return true;
                    case "b2":
                        updatelogs = "Update logs set total_time = @new_value where (log_id = @logid and fk_tour_id = @tour_id);";
                        NpgsqlCommand sqlupdate3 = new NpgsqlCommand(updatelogs, conn);
                        sqlupdate3.Parameters.AddWithValue("new_value", new_value);
                        sqlupdate3.Parameters.AddWithValue("logid", log_id);
                        sqlupdate3.Parameters.AddWithValue("tour_id", tour_id);
                        sqlupdate3.Prepare();
                        sqlupdate3.ExecuteNonQuery();
                        conn.Close();
                        return true;
                    case "b3":
                        updatelogs = "Update logs set report = @new_value where (log_id = @logid and fk_tour_id = @tour_id);";
                        NpgsqlCommand sqlupdate4 = new NpgsqlCommand(updatelogs, conn);
                        sqlupdate4.Parameters.AddWithValue("new_value", new_value);
                        sqlupdate4.Parameters.AddWithValue("logid", log_id);
                        sqlupdate4.Parameters.AddWithValue("tour_id", tour_id);
                        sqlupdate4.Prepare();
                        sqlupdate4.ExecuteNonQuery();
                        conn.Close();
                        return true;
                    case "b4":
                        updatelogs = "Update logs set rating = @new_value where (log_id = @logid and fk_tour_id = @tour_id);";
                        NpgsqlCommand sqlupdate5 = new NpgsqlCommand(updatelogs, conn);
                        sqlupdate5.Parameters.AddWithValue("new_value", new_value);
                        sqlupdate5.Parameters.AddWithValue("logid", log_id);
                        sqlupdate5.Parameters.AddWithValue("tour_id", tour_id);
                        sqlupdate5.Prepare();
                        sqlupdate5.ExecuteNonQuery();
                        conn.Close();
                        return true;
                    case "b5":
                        updatelogs = "Update logs set avarage_speed = @new_value where (log_id = @logid and fk_tour_id = @tour_id);";
                        NpgsqlCommand sqlupdate6 = new NpgsqlCommand(updatelogs, conn);
                        sqlupdate6.Parameters.AddWithValue("new_value", new_value);
                        sqlupdate6.Parameters.AddWithValue("logid", log_id);
                        sqlupdate6.Parameters.AddWithValue("tour_id", tour_id);
                        sqlupdate6.Prepare();
                        sqlupdate6.ExecuteNonQuery();
                        conn.Close();
                        return true;
                    case "b6":
                        updatelogs = "Update logs set comment = @new_value where (log_id = @logid and fk_tour_id = @tour_id);";
                        NpgsqlCommand sqlupdate7 = new NpgsqlCommand(updatelogs, conn);
                        sqlupdate7.Parameters.AddWithValue("new_value", new_value);
                        sqlupdate7.Parameters.AddWithValue("logid", log_id);
                        sqlupdate7.Parameters.AddWithValue("tour_id", tour_id);
                        sqlupdate7.Prepare();
                        sqlupdate7.ExecuteNonQuery();
                        conn.Close();
                        return true;
                    case "b7":
                        updatelogs = "Update logs set problems = @new_value where (log_id = @logid and fk_tour_id = @tour_id);";
                        NpgsqlCommand sqlupdate8 = new NpgsqlCommand(updatelogs, conn);
                        sqlupdate8.Parameters.AddWithValue("new_value", new_value);
                        sqlupdate8.Parameters.AddWithValue("logid", log_id);
                        sqlupdate8.Parameters.AddWithValue("tour_id", tour_id);
                        sqlupdate8.Prepare();
                        sqlupdate8.ExecuteNonQuery();
                        conn.Close();
                        return true;
                    case "b8":
                        updatelogs = "Update logs set transport_modus = @new_value where (log_id = @logid and fk_tour_id = @tour_id);";
                        NpgsqlCommand sqlupdate9 = new NpgsqlCommand(updatelogs, conn);
                        sqlupdate9.Parameters.AddWithValue("new_value", new_value);
                        sqlupdate9.Parameters.AddWithValue("logid", log_id);
                        sqlupdate9.Parameters.AddWithValue("tour_id", tour_id);
                        sqlupdate9.Prepare();
                        sqlupdate9.ExecuteNonQuery();
                        conn.Close();
                        return true;
                    case "b9":
                        updatelogs = "Update logs set recomended = @new_value where (log_id = @logid and fk_tour_id = @tour_id);";
                        NpgsqlCommand sqlupdate10 = new NpgsqlCommand(updatelogs, conn);
                        sqlupdate10.Parameters.AddWithValue("new_value", new_value);
                        sqlupdate10.Parameters.AddWithValue("logid", log_id);
                        sqlupdate10.Parameters.AddWithValue("tour_id", tour_id);
                        sqlupdate10.Prepare();
                        sqlupdate10.ExecuteNonQuery();
                        conn.Close();
                        return true;

                    default:
                        conn.Close();
                        return false;
                }
           

            }
            catch(Exception e)
            {

                return false;
            }
        }

        public Log GetNewLog(string tour_id, string log_id)
        {

            Log new_log = new Log();

            NpgsqlConnection conn = new NpgsqlConnection(connection_string);
            conn.Open();
            string strtours = "Select * from logs where (log_id = @log_id and fk_tour_id = @tour_id);";
            NpgsqlCommand sqlcomm = new NpgsqlCommand(strtours, conn);
            sqlcomm.Parameters.AddWithValue("log_id", log_id);
            sqlcomm.Parameters.AddWithValue("tour_id", tour_id);
            sqlcomm.Prepare();


            NpgsqlDataReader reader = sqlcomm.ExecuteReader();
            while (reader.Read())
            {

                new_log.UUID = reader.GetString(0);
                new_log.Date_Time = reader.GetString(2);
                new_log.Distance = reader.GetString(3);
                new_log.TotalTime = reader.GetString(4);
                new_log.Report = reader.GetString(5);
                new_log.Rating = reader.GetString(6);
                new_log.AvarageSpeed = reader.GetString(7);
                new_log.Comment = reader.GetString(8);
                new_log.Problems = reader.GetString(9);
                new_log.TransportModus = reader.GetString(10);
                new_log.Recomended = reader.GetString(11);
                ;
            }
            conn.Close();
            return new_log;
        }

        public Tour GetNewTour(string tour_id)
        {
            Tour new_tour = new Tour();

            NpgsqlConnection conn = new NpgsqlConnection(connection_string);
            conn.Open();
            string strtours = "Select * from tours where tour_id=@tour_id;";
            NpgsqlCommand sqlcomm = new NpgsqlCommand(strtours, conn);
            sqlcomm.Parameters.AddWithValue("tour_id", tour_id);
            sqlcomm.Prepare();


            NpgsqlDataReader reader = sqlcomm.ExecuteReader();
            while (reader.Read())
            {

                new_tour.UUID = reader.GetString(0);
                new_tour.Name = reader.GetString(1);
                new_tour.Description = reader.GetString(2);
                new_tour.From = reader.GetString(3);
                new_tour.To = reader.GetString(4);
                new_tour.Route_Type = reader.GetString(5);
                new_tour.PicPath = reader.GetString(6);

            }
            conn.Close();
            return new_tour;
        }



        public bool DeleteLog(string tour_id, string log_id)
        {
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection(connection_string);
                conn.Open();

                string strdelete = "Delete from logs where (log_id = @log_id and fk_tour_id = @tour_id)";
                NpgsqlCommand sqldelete = new NpgsqlCommand(strdelete, conn);

                sqldelete.Parameters.AddWithValue("log_id", log_id);
                sqldelete.Parameters.AddWithValue("tour_id", tour_id);
                sqldelete.Prepare();
                sqldelete.ExecuteNonQuery();

                conn.Close();
                return true;
            }
            catch(Exception e)
            {
                string exception = "{\"errorMessages\":[\"" + e.Message.ToString() + "\"],\"errors\":{}}";
                log.Error(exception, e);
                return false;
            }
           
        }

        public bool MakePdf(Tour current_tour)
        {
            throw new NotImplementedException();
        }
        public bool MakeReport(List <Tour> tours)
        {
            throw new NotImplementedException();
        }

        public bool Export(List<Tour> current_tours_in_DB)
        {
            throw new NotImplementedException();
        }

        public bool DoesTourExistInDb(string tour_id)
        {
            try
            {
                var con = new NpgsqlConnection(connection_string);
                con.Open();

                int count_username = 0;
                var sql_count = "SELECT count (*) from tours where tour_id = @UUID";
                var cmd = new NpgsqlCommand(sql_count, con);
                //prepared statments
                cmd.Parameters.AddWithValue("UUID", tour_id);
                cmd.Prepare();

                NpgsqlDataReader GetCount = cmd.ExecuteReader(); //curser

                GetCount.Read();
                count_username = GetCount.GetInt32(0);

                GetCount.Close();

                if (count_username > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                string exception = "{\"errorMessages\":[\"" + e.Message.ToString() + "\"],\"errors\":{}}";
                log.Error(exception, e);
                return false;
            }
           
        }

        public bool DeleteAllTour()
        {
            try
            {
                tourItems.Clear();

                NpgsqlConnection conn = new NpgsqlConnection(connection_string);
                conn.Open();

                string strdelete = "Delete from tours where tour_id is not null;";
                NpgsqlCommand sqldelete = new NpgsqlCommand(strdelete, conn);

                sqldelete.Prepare();
                sqldelete.ExecuteNonQuery();

                conn.Close();
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
