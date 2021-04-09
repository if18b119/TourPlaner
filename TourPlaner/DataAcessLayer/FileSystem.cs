using System;
using System.Collections.Generic;
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
        private readonly string filePath;

        public FileSystem()
        {
            this.filePath = "C:\\Users\\titto\\Desktop\\Studium\\4.Semester\\Swe2\\TourPlaner\\TourPic\\";
        }
        public bool AddTour(string name,string from, string to, string pic_path)
        {
            throw new NotImplementedException();
        }
        
        public string SaveImage(string from, string to)
        {
            string key = "zWULCwXtukAXX8gQEsyQVMrXaHNJJMPU";
            string picName;
            var json = string.Empty;
            string url = @"https://www.mapquestapi.com/staticmap/v5/map?start=" + from + "&end=" + to + "&size=600,400@2x&key=" + key;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse lxResponse = (HttpWebResponse)request.GetResponse())
            {
                using (BinaryReader reader = new BinaryReader(lxResponse.GetResponseStream()))
                {
                    Byte[] lnByte = reader.ReadBytes(1 * 1024 * 1024 * 10);
                    Random rand2 = new Random();
                    //Thread.Sleep(200);
                    picName = Convert.ToString(rand2.Next(999999));
                    picName += ".jpg";
                    using (FileStream lxFS = new FileStream(filePath + picName, FileMode.Create))
                    {
                        lxFS.Write(lnByte, 0, lnByte.Length);
                    }
                }

            }

            return filePath+picName;
        }

        public bool DeleteTour(string name)
        {
            throw new NotImplementedException();
        }

        public List<Tour> GetTours()
        {
            throw new NotImplementedException();
        }


    }
}
