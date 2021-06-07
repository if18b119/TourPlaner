using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TourPlaner.BusinessLayer;
using TourPlaner.Models;
using TourPlaner.Views;

namespace TourPlaner.ViewModels
{
    public class EditLogsViewModel: ViewModelBase
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

        public UpdateLogViewModel updateLogViewModel;

        private Tour current_tour;
        public ITourItemFactory itemFactory;
        private Log current_log;

        private RelayCommand update_field;
        public ICommand UpdateFieldCommand => update_field ??= new RelayCommand(UpdateField);

        private RelayCommand cancel;
        public ICommand CancelCommand => cancel ??= new RelayCommand(Cancel);

        private RelayCommand done;
        public ICommand DoneCommand => done ??= new RelayCommand(Done);

        
        private void Cancel(object obj)
        {
            Window window = (Window)obj;
            window.Close();
            
        }

        private void Done(object obj)
        {
            Window window = (Window)obj;
            window.Close();
            
        }

        private void UpdateField(object obj)
        {
            Button clicked_button = (Button)obj;
            string b_name = (string)clicked_button.Name;

            UpdateLogVIew updateLogView = new UpdateLogVIew();
            updateLogView.DataContext = updateLogViewModel;
            //Die daten die man für das update braucht der neuen view geben
            updateLogViewModel.tour_id = current_tour.UUID;
            updateLogViewModel.log_id = current_log.UUID;
            updateLogViewModel.to_update_name = b_name;

            //Immer wenn das updateWindow geschlossen wird, wir hier in diesem Objekt aus der Datenbank alle daten des logs wieder reingeladen
            Log updatedLog = new Log();

            bool? dialogResult = updateLogView.ShowDialog();
            if (!(bool)dialogResult)
            {
                //Um die Werte im mainwindowFenster zu aktualisieren
                itemFactory.GetTourLogs(current_tour.UUID);
                updatedLog = itemFactory.GetNewLog(current_tour.UUID, current_log.UUID);
                //Um die werte im editLog window zu aktualisieren
                switch(b_name)
                {
                    case "b0":
                        Date_Time = updatedLog.Date_Time;
                        break;
                    case "b1":
                        Distance = updatedLog.Distance;
                        break;
                    case "b2":
                        TotalTime = updatedLog.TotalTime;
                        break;
                    case "b3":
                        Report = updatedLog.Report;
                        break;
                    case "b4":
                        Rating = updatedLog.Rating;
                        break;
                    case "b5":
                        AvarageSpeed = updatedLog.AvarageSpeed;
                        break;
                    case "b6":
                        Comment = updatedLog.Comment;
                        break;
                    case "b7":
                        Problems = updatedLog.Problems;
                        break;
                    case "b8":
                        TransportModus = updatedLog.TransportModus;
                        break;
                    case "b9":
                        Recomended = updatedLog.Recomended;
                        break;
                    default:
                        break;
                }
            
            }

        }

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
       
        public Log CurrentLog
        {
            get
            {
                return current_log;
            }
            set
            {
                if (current_log != value)
                {
                    current_log = value;
                    RaisePropertyChangedEvent(nameof(CurrentLog));
                }
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

        public EditLogsViewModel()
        {
            this.itemFactory = TourItemFactory.GetInstance();
            this.updateLogViewModel = new UpdateLogViewModel();
           
            

        }


    }
}

