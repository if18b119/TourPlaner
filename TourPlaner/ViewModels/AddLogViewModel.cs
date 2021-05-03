using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TourPlaner.BusinessLayer;
using TourPlaner.DataAcessLayer;
using TourPlaner.Models;

namespace TourPlaner.ViewModels
{
    public class AddLogViewModel : ViewModelBase
    {
        private string date_Time;
        private double distance;
        private double totalTime;
        private string report;
        public Tour current_tour;
        public ITourItemFactory itemFactory;
        private RelayCommand addLog;
        public ICommand AddLogCommand => addLog ??= new RelayCommand(AddLog);

        public void AddLog(object obj)
        {

            itemFactory.AddLog(current_tour, Date_Time, Distance, TotalTime, Report);
            Window tmp = (Window)obj;
            tmp.Close();
        }

        public string NameOfTour
        {
            get
            {
                if (current_tour != null)
                    return current_tour.Name;
                else
                    return null;
            }
            set
            {
              
            }
        }
        public string Date_Time
        {
            get
            {

                return date_Time;
            }
            set
            {
                if (date_Time != value)
                {
                    date_Time = value;
                    RaisePropertyChangedEvent(nameof(Date_Time));
                }
            }
        }

        public double Distance
        {
            get
            {

                return distance;
            }
            set
            {
                if (distance != value)
                {
                    distance = value;
                    RaisePropertyChangedEvent(nameof(Distance));
                }
            }
        }


        public double TotalTime
        {
            get
            {

                return totalTime;
            }
            set
            {
                if (totalTime != value)
                {
                    totalTime = value;
                    RaisePropertyChangedEvent(nameof(TotalTime));
                }
            }
        }

        public AddLogViewModel()
        {
            this.itemFactory = TourItemFactory.GetInstance();
        }

        public string Report
        {
            get
            {

                return report;
            }
            set
            {
                if (report != value)
                {
                    report = value;
                    RaisePropertyChangedEvent(nameof(Report));
                }
            }
        }
    }
}
