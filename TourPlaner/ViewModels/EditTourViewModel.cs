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
    public class EditTourViewModel : ViewModelBase
    {

        private string tour_name;
        private string from;
        private string to;
        private string route_type;
        private string description;

        UpdateTourViewModel updateTourViewModel;

        private Tour current_tour;
        public ITourItemFactory itemFactory;

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

        public string RouteType
        {
            get
            {
                return route_type;
            }
            set
            {
                if (route_type != value)
                {
                    route_type = value;
                    RaisePropertyChangedEvent(nameof(RouteType));
                }
            }
        }
        public string TourName
        {
            get
            {

                return tour_name;
            }
            set
            {
                if (tour_name != value)
                {
                    tour_name = value;
                    RaisePropertyChangedEvent(nameof(TourName));

                }
            }
        }

        public string From
        {
            get
            {

                return from;
            }
            set
            {
                if (from != value)
                {
                    from = value;
                    RaisePropertyChangedEvent(nameof(From));
                }
            }
        }

        public string To
        {
            get
            {

                return to;
            }
            set
            {
                if (to != value)
                {
                    to = value;
                    RaisePropertyChangedEvent(nameof(To));
                }
            }
        }

        public string Description
        {
            get
            {

                return description;
            }
            set
            {
                if (description != value)
                {
                    description = value;
                    RaisePropertyChangedEvent(nameof(Description));
                }
            }
        }

        private void UpdateField(object obj)
        {
            Button clicked_button = (Button)obj;
            string b_name = (string)clicked_button.Name;

            UpdateTourView updateTourView = new UpdateTourView();
            updateTourView.DataContext = updateTourViewModel;
            //Die daten die man für das update braucht der neuen view geben
            updateTourViewModel.tour_id = current_tour.UUID;
            updateTourViewModel.to_update_name = b_name;

            //Immer wenn das updateWindow geschlossen wird, wir hier in diesem Objekt aus der Datenbank die neue Tour gespeichert
            

            bool? dialogResult = updateTourView.ShowDialog();
            if (!(bool)dialogResult)
            {
                //Um die Werte im mainwindowFenster zu aktualisieren
                Tour updatedTour = itemFactory.GetNewTour(current_tour.UUID);
                switch (b_name)
                {
                    case "b0":
                        TourName = updatedTour.Name;
                        break;
                    case "b1":
                        From = updatedTour.From;
                        break;
                    case "b2":
                        To = updatedTour.To;
                        break;
                    case "b3":
                        RouteType = updatedTour.Route_Type;
                        break;
                    case "b4":
                        Description = updatedTour.Description;
                        break;
                    default:
                        break;
                }
                

            }

        }

        public EditTourViewModel(Tour current_t)
        {
            CurrentTour = current_t;
            TourName = current_t.Name;
            From = current_t.From;
            To = current_t.To;
            RouteType = current_t.Route_Type;
            Description = current_t.Description;
            this.updateTourViewModel = new UpdateTourViewModel();
            this.itemFactory = TourItemFactory.GetInstance();
        }

    }
}
