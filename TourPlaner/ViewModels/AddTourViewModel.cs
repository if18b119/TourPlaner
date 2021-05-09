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
    public class AddTourViewModel : ViewModelBase
    {
        private string from;
        private string to;
        private string description;
        private string route_type;

        public ObservableCollection<TourType> list_of_tourtypes { get; private set; }

        public ITourItemFactory itemFactory;
        private RelayCommand addTour;
        public ICommand AddTourCommand => addTour ??= new RelayCommand(AddTour);

        private string newTourName;


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

        public ObservableCollection<TourType> ListOfTourTypes
        {
            get
            {
                return list_of_tourtypes;
            }
            set
            {
                if (list_of_tourtypes != value)
                {
                    list_of_tourtypes = value;
                    RaisePropertyChangedEvent(nameof(ListOfTourTypes));
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
        public string NewTourName
        {
            get
            {

                return newTourName;
            }
            set
            {
                if (newTourName != value)
                {
                    newTourName = value;
                    RaisePropertyChangedEvent(nameof(NewTourName));
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

        private void AddTour(object obj)
        {

            if (From == null || To == null || Description == null || RouteType == null || RouteType == null)
            {
                OpenNull(obj);
            }
            else
            {
                //UUID erstellen
                String UUID = Guid.NewGuid().ToString();

                itemFactory.AddTour(UUID, NewTourName, From, To, Description, RouteType);


                //die felder leeren
                NewTourName = string.Empty;
                From = string.Empty;
                To = string.Empty;
                Description = string.Empty;

                OpenAdded(obj);
                //schließen des Fensters
            }


        }

        public AddTourViewModel()
        {
            itemFactory = TourItemFactory.GetInstance();
            list_of_tourtypes = new ObservableCollection<TourType>();
            list_of_tourtypes.Add(new TourType(){ Name="Fastest"});
            list_of_tourtypes.Add(new TourType() { Name = "Shortest" });
            list_of_tourtypes.Add(new TourType() { Name = "Pedestrian" });
            list_of_tourtypes.Add(new TourType() { Name = "Bicycle" });
            list_of_tourtypes.Add(new TourType() { Name = "Multimodal" });
        }

    }
}
