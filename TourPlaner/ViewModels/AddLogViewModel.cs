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


        //Pop Up Added Successfully 
        // //////////////////////////////
        private ICommand openAddedCommand;
        private ICommand closeAddedCommand;
        private bool isAddedVisible;

        public ICommand OpenAddedCommand
        {
            get
            {
                if (openAddedCommand == null)
                    openAddedCommand = new RelayCommand(OpenAdded);
                return openAddedCommand;
            }
        }

        private void OpenAdded(object param)
        {
            IsAddedVisible = true;
        }

        public ICommand CloseAddedCommand
        {
            get
            {
                if (closeAddedCommand == null)
                    closeAddedCommand = new RelayCommand(CloseAdded);
                return closeAddedCommand;
            }
        }

        private void CloseAdded(object param)
        {
            IsAddedVisible = false;
            Window tmp = (Window)param;
            tmp.Close();
        }

        public bool IsAddedVisible
        {
            get { return isAddedVisible; }
            set
            {
                if (isAddedVisible == value)
                    return;
                isAddedVisible = value;
                RaisePropertyChangedEvent("IsAddedVisible");
            }
        }

        // ////////////

        //Pop Up Null  
        // //////////////////////////////
        private ICommand openNullCommand;
        private ICommand closeNullCommand;
        private bool isNullVisible;

        public ICommand OpenNullCommand
        {
            get
            {
                if (openNullCommand == null)
                    openNullCommand = new RelayCommand(OpenNull);
                return openAddedCommand;
            }
        }

        private void OpenNull(object param)
        {
            IsNullVisible = true;
        }

        public ICommand CloseNullCommand
        {
            get
            {
                if (closeNullCommand == null)
                    closeNullCommand = new RelayCommand(CloseNull);
                return closeNullCommand;
            }
        }

        private void CloseNull(object param)
        {
            IsNullVisible = false;
           
        }

        public bool IsNullVisible
        {
            get { return isNullVisible; }
            set
            {
                if (isNullVisible == value)
                    return;
                isNullVisible = value;
                RaisePropertyChangedEvent("IsNullVisible");
            }
        }

        // ////////////


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
            if (Date_Time == null || Distance == null || TotalTime == null || Report == null || Rating == null || AvarageSpeed == null
                || Comment == null || Problems == null || TransportModus == null || Recomended == null )
            {
                OpenNull(obj);
            }
            else
            {
                itemFactory.AddLog(current_tour, date_Time, distance, totalTime, report, rating, avarage_speed, comment, problems, transport_modus, recomended);
                CurrentTour = null;
                Date_Time = null;
                Distance = null;
                TotalTime = null;
                Report = null;
                Rating = null;
                AvarageSpeed = null;
                Comment = null;
                Problems = null;
                TransportModus = null;
                Recomended = null;
                OpenAdded(obj);
            }

            
            
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
