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
    public class AddLogViewModel : ViewModelBase, INotifyDataErrorInfo
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
        //Data valdierung
        //https://kmatyaszek.github.io/wpf%20validation/2019/03/13/wpf-validation-using-inotifydataerrorinfo.html
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        private readonly Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();
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
        private bool ValidateDateOrTime()
        {
            //reggex
            Regex regex = new Regex(@"^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[13-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$", RegexOptions.IgnorePatternWhitespace);
            Regex regex2 = new Regex(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", RegexOptions.IgnorePatternWhitespace);
            Match x = regex.Match(Date_Time);
            Match x2 = regex2.Match(Date_Time);

            ClearErrors(nameof(Date_Time));
            if (string.IsNullOrWhiteSpace(Date_Time))
            {
                AddError(nameof(Date_Time), "Field cannot be empty.");
                return false;
            }
            if (x.Success == false && x2.Success==false)
            {
                AddError(nameof(Date_Time), "Wrong Format of Date or Time");
                return false;
            }
             else
            {
                return true;
            }
            
        }
        private bool ValidateDistance()
        {
            //reggex
            Regex regex = new Regex(@"[+]?([0-9]*[.])?[0-9]+", RegexOptions.IgnorePatternWhitespace);
            Match x = regex.Match(Distance);

            ClearErrors(nameof(Distance));
            if (string.IsNullOrWhiteSpace(Distance))
            {
                AddError(nameof(Distance), "Field cannot be empty.");
                return false;
            }
            if (x.Success == false)
            {
                AddError(nameof(Distance), "Wrong Format of Distance");
                return false;
            }
            else
            {
                return true;
            }

        }
        private bool ValidateTotalTime()
        {
            //reggex
            Regex regex = new Regex(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$", RegexOptions.IgnorePatternWhitespace);
            Match x = regex.Match(TotalTime);

            ClearErrors(nameof(TotalTime));
            if (string.IsNullOrWhiteSpace(TotalTime))
            {
                AddError(nameof(TotalTime), "Field cannot be empty.");
                return false;
            }
            if (x.Success == false)
            {
                AddError(nameof(TotalTime), "Wrong Format of Distance");
                return false;
            }
            else
            {
                return true;
            }

        }

        private bool ValidateReport()
        {

            ClearErrors(nameof(Report));
            if (string.IsNullOrWhiteSpace(Report))
            {
                AddError(nameof(Report), "Field cannot be empty.");
                return false;
            }
            if (Report == null || Report?.Length <= 3)
            {
                AddError(nameof(Report), "Report must contain at least 3 characters.");
                return false;
            }
            if (Report?.Length >= 100)
            {
                AddError(nameof(Report), "Report can contain a maximum of 20 characters.");
                return false;
            }
            else
            {
                return true;
            }
        }
        private bool ValidateRating()
        {
            //reggex
            Regex regex = new Regex(@"^([0-5]){1}$", RegexOptions.IgnorePatternWhitespace);
            Match x = regex.Match(Rating);

            ClearErrors(nameof(Rating));
            if (string.IsNullOrWhiteSpace(Rating))
            {
                AddError(nameof(Rating), "Field cannot be empty.");
                return false;
            }
            if (x.Success == false)
            {
                AddError(nameof(Rating), "Wrong Format of Distance");
                return false;
            }
            else
            {
                return true;
            }

        }
        private bool ValidateAvgSpeed()
        {
            //reggex
            Regex regex = new Regex(@"[+]?([0-9]*[.])?[0-9]+", RegexOptions.IgnorePatternWhitespace);
            Match x = regex.Match(AvarageSpeed);

            ClearErrors(nameof(AvarageSpeed));
            if (string.IsNullOrWhiteSpace(AvarageSpeed))
            {
                AddError(nameof(AvarageSpeed), "Field cannot be empty.");
                return false;
            }
            if (x.Success == false)
            {
                AddError(nameof(AvarageSpeed), "Wrong Format of Distance");
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool ValidateComment()
        {

            ClearErrors(nameof(Comment));
            if (string.IsNullOrWhiteSpace(Comment))
            {
                AddError(nameof(Comment), "Field cannot be empty.");
                return false;
            }
            if (Comment == null || Comment?.Length <= 3)
            {
                AddError(nameof(Comment), "Comment must contain at least 3 characters.");
                return false;
            }
            if (Comment?.Length >= 100)
            {
                AddError(nameof(Comment), "Comment can contain a maximum of 20 characters.");
                return false;
            }
            else
            {
                return true;
            }

        }

        private bool ValidateProblems()
        {

            ClearErrors(nameof(Problems));
            if (string.IsNullOrWhiteSpace(Problems))
            {
                AddError(nameof(Problems), "Field cannot be empty.");
                return false;
            }
            if (Problems == null || Problems?.Length <= 3)
            {
                AddError(nameof(Problems), "Problems must contain at least 3 characters.");
                return false;
            }
            if (Problems?.Length >= 100)
            {
                AddError(nameof(Problems), "Problems can contain a maximum of 20 characters.");
                return false;
            }
            else
            {
                return true;
            }

        }

        private bool ValidateTransport()
        {

            ClearErrors(nameof(TransportModus));
            if (string.IsNullOrWhiteSpace(TransportModus))
            {
                AddError(nameof(TransportModus), "Field cannot be empty.");
                return false;
            }
            if (TransportModus == null || TransportModus?.Length <= 3)
            {
                AddError(nameof(TransportModus), "TransportModus must contain at least 3 characters.");
                return false;
            }
            if (TransportModus?.Length >= 100)
            {
                AddError(nameof(TransportModus), "TransportModus can contain a maximum of 20 characters.");
                return false;
            }
            else
            {
                return true;
            }


        }

        private bool ValidateRecomended()
        {

            ClearErrors(nameof(Recomended));
            if (string.IsNullOrWhiteSpace(Recomended))
            {
                AddError(nameof(Recomended), "Field cannot be empty.");
                return false;
            }
                
            if (Recomended == null || Recomended?.Length <= 3)
            {
                AddError(nameof(Recomended), "Recomended must contain at least 3 characters.");
                return false;
            }
                
            if (Recomended?.Length >= 100)
            {
                AddError(nameof(Recomended), "Recomended can contain a maximum of 20 characters.");
                return false;
            }
            else
            {
                return true;
            }
                

        }
        //--------------------Data validierung ende ----------------//
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
                if(ValidateDateOrTime() && ValidateDistance() && ValidateTotalTime() && ValidateReport() && ValidateRating() &&
                    ValidateAvgSpeed() && ValidateComment() && ValidateProblems() && ValidateTransport() && ValidateRecomended())
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
                    ValidateDateOrTime();
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
                    ValidateDistance();
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
                    ValidateTotalTime();
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
                    ValidateReport();
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
                    ValidateRating();
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
                    ValidateAvgSpeed();
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
                    ValidateComment();
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
                    ValidateProblems();
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
                    ValidateTransport();
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
                    ValidateRecomended();
                }
            }
        }

        

        public AddLogViewModel()
        {
            this.itemFactory = TourItemFactory.GetInstance();
        }

       
    }
}
