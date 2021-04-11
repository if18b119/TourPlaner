using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlaner.BusinessLayer;
using TourPlaner.DataAcessLayer;

namespace TourPlaner.ViewModels
{
    public class AddTourViewModel : ViewModelBase
    {
        private string from;
        private string to;
        private string description;
        public ITourItemFactory itemFactory;
        private RelayCommand addTour;
        public ICommand AddTourCommand => addTour ??= new RelayCommand(AddTour);

        private string newTourName;

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
            itemFactory.AddTour(NewTourName, From, To, Description);
        }

        public AddTourViewModel()
        {
            itemFactory = TourItemFactory.GetInstance();
            
        }

    }
}
