﻿using System;
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
        private string distance;
        private string totalTime;
        private string report;
        private string rating;
        private string avarage_speed;
        private string comment;
        private string problems;
        private string transport_modus;
        private string recomended;

        private Tour current_tour;
        public ITourItemFactory itemFactory;
        private RelayCommand addLog;
        public ICommand AddLogCommand => addLog ??= new RelayCommand(AddLog);


        public Tour CurrentTour
        {
            get
            {
                return current_tour;
            }
            set
            {
                if (current_tour != value)
                {
                    current_tour = value;
                    RaisePropertyChangedEvent(nameof(CurrentTour));
                }
            }
        }
        public void AddLog(object obj)
        {

            itemFactory.AddLog(current_tour, date_Time, distance, totalTime, report, rating, avarage_speed, comment, problems, transport_modus, recomended);

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

        public string Distance
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


        public string TotalTime
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

        public string Rating
        {
            get
            {

                return rating;
            }
            set
            {
                if (rating != value)
                {
                    rating = value;
                    RaisePropertyChangedEvent(nameof(Rating));
                }
            }
        }

        public string AvarageSpeed
        {
            get
            {

                return avarage_speed;
            }
            set
            {
                if (avarage_speed != value)
                {
                    avarage_speed = value;
                    RaisePropertyChangedEvent(nameof(AvarageSpeed));
                }
            }
        }

        public string Comment
        {
            get
            {

                return comment;
            }
            set
            {
                if (comment != value)
                {
                    comment = value;
                    RaisePropertyChangedEvent(nameof(Comment));
                }
            }
        }

        public string Problems
        {
            get
            {

                return problems;
            }
            set
            {
                if (problems != value)
                {
                    problems = value;
                    RaisePropertyChangedEvent(nameof(Problems));
                }
            }
        }

        public string TransportModus
        {
            get
            {

                return transport_modus;
            }
            set
            {
                if (transport_modus != value)
                {
                    transport_modus = value;
                    RaisePropertyChangedEvent(nameof(TransportModus));
                }
            }
        }

        public string Recomended
        {
            get
            {

                return recomended;
            }
            set
            {
                if (recomended != value)
                {
                    recomended = value;
                    RaisePropertyChangedEvent(nameof(Recomended));
                }
            }
        }

        public AddLogViewModel()
        {
            this.itemFactory = TourItemFactory.GetInstance();
        }

       
    }
}
