using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TourPlaner.BusinessLayer;
using TourPlaner.DataAcessLayer;
using TourPlaner.Models;

namespace NUnitTourPlaner
{
    [TestFixture]
    public class Tests
    {
        string pfad = "C:\\Users\\titto\\Desktop\\Studium\\4.Semester\\Swe2\\TourPlaner\\TourPic";
        ITourItemFactory tour_item_fac_impl = TourItemFactory.GetInstance();
        [OneTimeSetUp]
        public void Setup()
        {
            //ITourItemFactory tour_item_fac_impl = TourItemFactory.GetInstance();
            string[] files = Directory.GetFiles(pfad);
            foreach (string file in files)
            {
                File.Delete(file);
            }
        }

        [TearDown]
        public void TearDown()
        {

        }

        [Test, Order(1)]
        public void TourCreateDB()
        {
            Assert.AreEqual(true, tour_item_fac_impl.AddTour("Tour1", "Vienna", "Berlin", "Hey"));
        }

        [Test, Order(2)]
        public void SavedPicToFolder()
        {
            
            int fCount = Directory.GetFiles(pfad, "*", SearchOption.TopDirectoryOnly).Length;

            Assert.AreEqual(1, fCount);
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
            Assert.AreEqual(1, count_tours);
        }

        [Test, Order(4)]
        public void ControllParameters()
        {
            IEnumerable<Tour> created_tours = tour_item_fac_impl.GetItems();
            Tour first = created_tours.First();
            Assert.AreEqual("Tour1", first.Name);
            Assert.AreEqual("Vienna", first.From);
            Assert.AreEqual("Berlin", first.To);

        }

        [Test, Order(5)]
        public void DeleteFromDB()
        {
            IEnumerable<Tour> created_tours = tour_item_fac_impl.GetItems();
            Tour first = created_tours.First();

            Assert.AreEqual(true, tour_item_fac_impl.DeleteTour(first));

        }

        [Test, Order(6)]
        public void CountListAfterDelete()
        {
            IEnumerable<Tour> created_tours = tour_item_fac_impl.GetItems();
            int count_tours = 0;
            foreach (Tour item in created_tours)
            {
                count_tours++;
            }
            Assert.AreEqual(0, count_tours);
        }

        [Test, Order(7)]
        public void CountPicAfterDelete()
        {
            string pfad = "C:\\Users\\titto\\Desktop\\Studium\\4.Semester\\Swe2\\TourPlaner\\TourPic";
            int fCount = Directory.GetFiles(pfad, "*", SearchOption.TopDirectoryOnly).Length;

            Assert.AreEqual(0, fCount);
        }
    }
}