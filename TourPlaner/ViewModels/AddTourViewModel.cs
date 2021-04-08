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

        private void AddTour(object obj)
        {
            itemFactory.AddTour(NewTourName);
        }

        public AddTourViewModel()
        {
            itemFactory = TourItemFactory.GetInstance(DataType.Database);
            
        }

    }
}
