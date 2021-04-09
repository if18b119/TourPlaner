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
    public class MainWindowViewModel : ViewModelBase
    {

        public ITourItemFactory itemFactory;

        public AddTourViewModel addTourViewModel;

        //die ObservableCollection nimmt dann die liste die von dem DataAccessLayer über den businessLayer geht
        private ObservableCollection<Tour> tours;

        private Tour currentTour;

        private RelayCommand addTourCommand;
        public ICommand AddTourCommand => addTourCommand ??= new RelayCommand(AddTour);

        private void AddTour(object obj)
        {
            //view Object erstellen und DataContext auf dessen Viewmodel setzen.
           
            AddTourView addTourView = new AddTourView();
            addTourView.DataContext = addTourViewModel;
            bool? dialogResult = addTourView.ShowDialog();
            if(!(bool)dialogResult)
            {
                tours.Clear();
                RefreshingListItems();
            }
        }

        private ICommand deleteTourCommand;
        public ICommand DeleteTourCommand => deleteTourCommand ??= new RelayCommand(Delete);

        private void Delete(object obj)
        {

            if (currentTour == null)
                return;
            itemFactory.DeleteTour(currentTour);
            tours.Clear();
            RefreshingListItems();
        }

        public ObservableCollection<Tour> Tours
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

        public Tour CurrentTour
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
             addTourViewModel = new AddTourViewModel();
            itemFactory = TourItemFactory.GetInstance();
            ReadListBox();

        }

        private void ReadListBox()
        {
            Tours = new ObservableCollection<Tour>();
            RefreshingListItems();
        }

        private void RefreshingListItems()
        {
            foreach (Tour item in this.itemFactory.GetItems())
            {
                Tours.Add(item);
            }
        }

        private void Search(object commandParameter)
        {

        }


        private void Clear(object commandParameter)
        {

        }

       
    }
}
