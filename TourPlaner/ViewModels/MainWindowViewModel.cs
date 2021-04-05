using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace TourPlaner.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ObservableCollection<TourViewModel> tours;
        private TourViewModel currentTour;

        //Das zweite Fenster ViewModel 
        


        private RelayCommand addTourCommand;
        public ICommand AddTourCommand => addTourCommand ??= new RelayCommand(AddTour);

        private void AddTour(object obj)
        {
            //view Object erstellen und DataContext auf dessen Viewmodel setzen.
            AddTourViewModel addTourViewModel = new AddTourViewModel();
            AddTourView addTourView = new AddTourView();
            addTourView.DataContext = addTourViewModel;
            addTourView.ShowDialog();
            
        }

        private ICommand deleteTourCommand;
        public ICommand DeleteTourCommand => deleteTourCommand ??= new RelayCommand(Delete);

        private void Delete(object obj)
        {
            Tours.Remove(currentTour);
        }

        public ObservableCollection<TourViewModel> Tours
        {
            get
            {
                
                return tours;
            }
            set
            {
                if (tours != value)
                {
                    tours = value;
                    RaisePropertyChangedEvent(nameof(Tours));
                }
            }
        }

        public TourViewModel CurrentTour
        {
            get
            {
                
                return currentTour;
            }
            set
            {
                if (currentTour != value)
                {
                    currentTour = value;
                    RaisePropertyChangedEvent(nameof(CurrentTour));
                }
            }
        }

        public MainWindowViewModel()
        {
            Tours = new ObservableCollection<TourViewModel>();
            
        }

        private void Search(object commandParameter)
        {

        }


        private void Clear(object commandParameter)
        {

        }

       
    }
}
