using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TourPlaner.BusinessLayer;
using TourPlaner.DataAcessLayer;
using TourPlaner.Models;

namespace TourPlaner.ViewModels
{
    public class AddTourViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private string from;
        private string to;
        private string description;
        private string route_type;

        //Logging -Instanz
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
                    ValidateTourName();
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
                    ValidateFrom();
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
                    ValidateTo();
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
                    ValidateDesc();
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
                if(ValidateTourName() && ValidateFrom() && ValidateTo() && ValidateDesc())
                { 
                    //UUID erstellen
                    String UUID = Guid.NewGuid().ToString();

                    itemFactory.AddTour(UUID, NewTourName, From, To, Description, RouteType);

                    string _log = "{\"tourAdded\":[Name: \"" + NewTourName + "\"],\"successfully\":{1}}";
                    log.Info(_log);

                    //die felder leeren
                    NewTourName = string.Empty;
                    From = string.Empty;
                    To = string.Empty;
                    Description = string.Empty;
                    OpenAdded(obj);

                }
                
            }

        }


        //Datenvalidierung
        //https://kmatyaszek.github.io/wpf%20validation/2019/03/13/wpf-validation-using-inotifydataerrorinfo.html

        private readonly Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public bool HasErrors => _errorsByPropertyName.Any();
        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsByPropertyName.ContainsKey(propertyName) ?
            _errorsByPropertyName[propertyName] : null;
        }
        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
        private void AddError(string propertyName, string error)
        {
            if (!_errorsByPropertyName.ContainsKey(propertyName))
                _errorsByPropertyName[propertyName] = new List<string>();

            if (!_errorsByPropertyName[propertyName].Contains(error))
            {
                _errorsByPropertyName[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        private void ClearErrors(string propertyName)
        {
            if (_errorsByPropertyName.ContainsKey(propertyName))
            {
                _errorsByPropertyName.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }

        private bool ValidateFrom()
        {
            Regex regex = new Regex(@"[a-zA-Z]{3,}", RegexOptions.IgnorePatternWhitespace);
            Match x = regex.Match(From);

            ClearErrors(nameof(From));
            if (string.IsNullOrWhiteSpace(From))
            {
                AddError(nameof(From), "Field cannot be empty.");
                return false;
            }
            if (From == null || From?.Length <= 2)
            {
                AddError(nameof(From), "'From' must contain at least 3 characters.");
                return false;
            }
                
            if (From?.Length >= 21)
            {
                AddError(nameof(From), "'From' can contain a maximum of 20 characters.");
                return false;
            }
            if (x.Success == false)
            {
                AddError(nameof(From), "Don't use numbers");
                return false;
            }
            else
            {
                return true;
            }

        }
        private bool ValidateTourName()
        {
            

            ClearErrors(nameof(NewTourName));
            if (string.IsNullOrWhiteSpace(NewTourName))
            {
                AddError(nameof(NewTourName), "Field cannot be empty.");
                return false;
            }

            if (NewTourName == null || NewTourName?.Length <= 2)
            {
                AddError(nameof(NewTourName), "Tour name must contain at least 3 characters.");
                return false;
            }
            if (NewTourName?.Length >= 21)
            {
                AddError(nameof(NewTourName), "Tour name can contain a maximum of 20 characters.");
                return false;
            }
           
            else
            {
                return true;
            }
                
        }
        private bool ValidateTo()
        {
            Regex regex = new Regex(@"[a-zA-Z]{3,}", RegexOptions.IgnorePatternWhitespace);
            Match x = regex.Match(To);

            ClearErrors(nameof(To));
            if (string.IsNullOrWhiteSpace(To))
            {
                AddError(nameof(To), "'To' cannot be empty.");
                return false;
            }
                
            if (To == null || To?.Length <= 2)
            {
                AddError(nameof(To), "'To' must contain at least 3 characters.");
                return false;
            }
               
            if (To?.Length >= 21)
            {
                AddError(nameof(To), "'To' can contain a maximum of 20 characters.");
                return false;
            }
                
            if (x.Success == false)
            {
                AddError(nameof(To), "Don't use numbers");
                return false;
            }
            else
            {
                return true;
            }
                
        }
        private bool ValidateDesc()
        {
            ClearErrors(nameof(Description));
            if (string.IsNullOrWhiteSpace(Description))
            {
                AddError(nameof(Description), "Field cannot be empty.");
                return false;
            }
                
            if (Description == null || Description?.Length <= 2)
            {
                AddError(nameof(Description), "Description must contain at least 3 characters.");
                return false;
            }
                
            if (Description?.Length >= 151)
            {
                AddError(nameof(Description), "Description can contain a maximum of 150 characters.");
                return false;
            }
            else
            {
                return true;
            }
        }


        //----------------------Ende Valdierung------------//
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
