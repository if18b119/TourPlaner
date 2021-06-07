using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TourPlaner.BusinessLayer;
using TourPlaner.DataAcessLayer;
using TourPlaner.Models;

namespace NUnitTourPlaner
{
    [TestFixture, SingleThreaded]
    public class Tests
    {
        
        ITourItemFactory tour_item_fac_impl = TourItemFactory.GetInstance();
        string save_path;
        string delete_path;
        string key;
        string to_pdf;
        string export_path;
        ConfigFile config_file;
        int num_pic;
        int num_pdf;
        int num_export;
        int num_tours=0;

        [OneTimeSetUp]
        public void Setup()
        {
            
            string json_path = "config_file_test.json";
            string json = File.ReadAllText(json_path);
            config_file = JsonConvert.DeserializeObject<ConfigFile>(json);
            this.save_path = config_file.RouteImageSettings.Location;
            this.delete_path = config_file.ToDeletePath.Path;
            this.key = config_file.RequestKey.Key;
            this.to_pdf = config_file.GeneratePdf.Path;
            this.export_path = config_file.ExportTours.Path;



            //die anzahl der files in jedem ordner
            num_pic = Directory.GetFiles(save_path, "*", SearchOption.TopDirectoryOnly).Length;
            num_pdf = Directory.GetFiles(to_pdf, "*", SearchOption.TopDirectoryOnly).Length;
            num_export = Directory.GetFiles(export_path, "*", SearchOption.TopDirectoryOnly).Length;

            IEnumerable<Tour> created_tours = tour_item_fac_impl.GetItems();
           
            foreach (Tour item in created_tours)
            {
                num_tours++;
            }
        }

        [TearDown]
        public void TearDown()
        {

        }

        [Test, Order(1)]
        public void TourCreateDB()
        {
            Assert.AreEqual(true, tour_item_fac_impl.AddTour("UUID","Tour1", "Vienna", "Berlin", "Hey","Fastest"));
        }

        [Test, Order(2)]
        public void SavedPicToFolder()
        {
            
            int fCount = Directory.GetFiles(save_path, "*", SearchOption.TopDirectoryOnly).Length;

            Assert.AreEqual(num_pic +1, fCount);
        }

        [Test, Order(3)]
        public void CountTours()
        {
            IEnumerable<Tour> created_tours = tour_item_fac_impl.GetItems();           
            int count_tours = 0;
            foreach (Tour item in created_tours)
            {
                count_tours++;
            }
            Assert.AreEqual(num_tours +1, count_tours);
        }

        [Test, Order(4)]
        public void ControllParameters()
        {
            IEnumerable<Tour> created_tours = tour_item_fac_impl.GetItems();
            Tour first = created_tours.Where(x =>x.UUID =="UUID").First();
            Assert.AreEqual("Tour1", first.Name);
            Assert.AreEqual("Vienna", first.From);
            Assert.AreEqual("Berlin", first.To);

        }

        [Test, Order(5)]
        public void AddLog()
        {
            IEnumerable<Tour> created_tours = tour_item_fac_impl.GetItems();
            Tour first = created_tours.Where(x => x.UUID == "UUID").First();

            Assert.AreEqual(true,tour_item_fac_impl.AddLog(first, "12.12.2020", "234", "2", "Jo", "5", "23", "gut", "None", "good", "Yes"));
        }

        [Test,Order(6)]
        public void AddLogError()
        {
            Assert.AreEqual(true, tour_item_fac_impl.AddLog(null, "12.12.2020", "234", "2", "Jo", "5", "23", "gut", "None", "good", "Yes"));
        }

        [Test, Order(7)]
        public void EditLog()
        {
            IEnumerable<Tour> created_tours = tour_item_fac_impl.GetItems();
            Tour first = created_tours.Where(x => x.UUID == "UUID").First();
            Log first_log = tour_item_fac_impl.GetTourLogs(first.UUID)[0];
            //b0 = date-time
            Assert.AreEqual(true,tour_item_fac_impl.UpdateLogValue(first.UUID, first_log.UUID, "b0", "11.11.2011"));
        }

        [Test, Order(8)]
        public void EditLogError()
        {
            IEnumerable<Tour> created_tours = tour_item_fac_impl.GetItems();
            Tour first = created_tours.Where(x => x.UUID == "UUID").First();
            Log first_log = tour_item_fac_impl.GetTourLogs(first.UUID)[0];

            Assert.AreEqual(false, tour_item_fac_impl.UpdateLogValue(first.UUID, first_log.UUID, "b14", "11.11.2011"));
        }

        [Test, Order(9)]
        public void EditTour()
        {
            
            Assert.AreEqual(true,tour_item_fac_impl.UpdateTourValue("UUID", "b0", "Tour1Updated"));
      
        }

        [Test, Order(10)]
        public void CheckTourEditedSucc()
        {

            IEnumerable<Tour> created_tours = tour_item_fac_impl.GetItems();
            Tour first = created_tours.Where(x => x.UUID == "UUID").First();

            Assert.AreEqual(first.Name, "Tour1Updated");

        }

        [Test, Order(11)]
        public void TourEditError()
        {
            Assert.AreEqual(false, tour_item_fac_impl.UpdateTourValue("UUID", "b123", "Tour1UpdatedNew"));
        }

        
        [Test, Order(12)]
        public void CheckCopyPaste()
        {
            IEnumerable<Tour> created_tours = tour_item_fac_impl.GetItems();
            Tour first = created_tours.Where(x => x.UUID == "UUID").First();

            if(first !=null)
            {
                Assert.AreEqual(true, tour_item_fac_impl.Paste(first));
            }
            
            
        }

        [Test, Order(13)]
        public void CheckCopyPasteSucc()
        {

            IEnumerable<Tour> created_tours = tour_item_fac_impl.GetItems();
            int count_tours = 0;
            foreach (Tour item in created_tours)
            {
                count_tours++;
            }
            Assert.AreEqual(num_tours + 2, count_tours);
        }

        [Test,Order(14)]
        public void CheckIdAfterCopy()
        {
            IEnumerable<Tour> created_tours = tour_item_fac_impl.GetItems();
            Tour first = created_tours.Where(x => x.UUID == "UUID-Copy").First();
            Assert.IsNotNull(first);
        }

        [Test, Order (15)]
        public void Export()
        {
            
            Assert.AreEqual(true,tour_item_fac_impl.Export());
            
        }

        [Test, Order(16)]
        public void CheckExport()
        {
            int fCount = Directory.GetFiles(export_path, "*", SearchOption.TopDirectoryOnly).Length;
            Assert.AreEqual(num_export + 1, fCount);
        }



        [Test, Order(17)]
        public void DeleteFromDB()
        {
            IEnumerable<Tour> created_tours = tour_item_fac_impl.GetItems();
            Tour first = created_tours.Where(x => x.UUID == "UUID").First();
            string pfad = first.PicPath;
            Tour copy = created_tours.Where(x => x.UUID == "UUID-Copy").First();
            string pfad_copy = copy.PicPath;
            //foto löschen
            File.Delete(pfad);
            File.Delete(pfad_copy);

            Assert.AreEqual(true, tour_item_fac_impl.SavePathAndDeleteTour(first));
            Assert.AreEqual(true, tour_item_fac_impl.SavePathAndDeleteTour(copy));
        }

        [Test, Order(18)]
        public void CheckIfDeleted()
        {
            int fCount = Directory.GetFiles(save_path, "*", SearchOption.TopDirectoryOnly).Length;
            Assert.AreEqual(num_pic +1, fCount);
        }

        [Test, Order(19)]
        public void CountListAfterDelete()
        {
            IEnumerable<Tour> created_tours = tour_item_fac_impl.GetItems();
            int count_tours = 0;
            foreach (Tour item in created_tours)
            {
                count_tours++;
            }
            Assert.AreEqual(num_tours, count_tours);
        }

        [Test, Order(20)]
        public void CountPicAfterDelete()
        {
            
            int fCount = Directory.GetFiles(save_path, "*", SearchOption.TopDirectoryOnly).Length;

            Assert.AreEqual(num_pic + 1, fCount);
        }
       
    }
}