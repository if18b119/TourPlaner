using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TourPlaner.BusinessLayer;

namespace TourPlaner.ViewModels
{
    public class UpdateLogViewModel:ViewModelBase
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

        private void UpdateField(object obj)
        {
            Window window = (Window)obj;

            itemFactory.UpdateLogValue(tour_id, log_id, to_update_name, NewValue);
            new_value = string.Empty;

            window.Close();
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
