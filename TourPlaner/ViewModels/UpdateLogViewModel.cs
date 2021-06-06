using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TourPlaner.BusinessLayer;

namespace TourPlaner.ViewModels
{
    public class UpdateLogViewModel:ViewModelBase, INotifyDataErrorInfo
    {
        public string to_update_name;
        public string tour_id;
        public string log_id;



        private RelayCommand update_field;
        public ICommand UpdateFieldCommand => update_field ??= new RelayCommand(UpdateField);
        private RelayCommand cancel;
        public ICommand CancelCommand => cancel ??= new RelayCommand(Cancel);

        ITourItemFactory itemFactory;

        private string new_value;

        //Pop Up Edited Successfully
        // //////////////////////////////
        private ICommand openEditedCommand;
        private ICommand closeEditedCommand;
        private bool isEditedVisible;

        

        public ICommand OpenEditedCommand
        {
            get
            {
                if (openEditedCommand == null)
                    openEditedCommand = new RelayCommand(OpenEdited);
                return openEditedCommand;
            }
        }

        private void OpenEdited(object param)
        {
            IsEditedVisible = true;
        }

        public ICommand CloseEditedCommand
        {
            get
            {
                if (closeEditedCommand == null)
                    closeEditedCommand = new RelayCommand(CloseEdited);
                return closeEditedCommand;
            }
        }

        private void CloseEdited(object param)
        {
            IsEditedVisible = false;
        }

        public bool IsEditedVisible
        {
            get { return isEditedVisible; }
            set
            {
                if (isEditedVisible == value)
                    return;
                isEditedVisible = value;
                RaisePropertyChangedEvent("IsEditedVisible");
            }
        }

        // ////////////

        public string NewValue
        {
            get
            {
                return new_value;
            }
            set
            {
                if (new_value != value)
                {
                    new_value = value;
                    RaisePropertyChangedEvent(nameof(NewValue));
                    
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

        private bool Validate()
        {

            ClearErrors(nameof(NewValue));
            if (string.IsNullOrWhiteSpace(NewValue))
            {
                AddError(nameof(NewValue), "Field cannot be empty.");
                return false;
            }               
            if (NewValue == null || NewValue?.Length <= 2)
            {
                AddError(nameof(NewValue), "'From' must contain at least 3 characters.");
                return false;
            }                
            if (NewValue?.Length >= 21)
            {
                AddError(nameof(NewValue), "'From' can contain a maximum of 20 characters.");
                return false;
            }
            else
            {
                return true;
            }
                
        }

        //------------------End of Data validierung ------------//

        private void UpdateField(object obj)
        {
            Window window = (Window)obj;
            if(Validate())
            {
                
                itemFactory.UpdateLogValue(tour_id,log_id, to_update_name, NewValue);
                OpenEdited(obj);
                new_value = string.Empty;
                window.Close();
            }
            else
            {
                return;
            }
            
        }

        private void Cancel(object obj)
        {
            
            Window window = (Window)obj;
            new_value = string.Empty;
            window.Close();
        }

       

        public UpdateLogViewModel()
        {
            this.itemFactory = TourItemFactory.GetInstance();

        }
    }
}
