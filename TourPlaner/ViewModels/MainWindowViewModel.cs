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
using TourPlaner.Views;

namespace TourPlaner.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        public ITourItemFactory itemFactory;

        public AddTourViewModel addTourViewModel;
        public AddLogViewModel addLogViewModel;
        public EditLogsViewModel editLogViewModel;

        //die ObservableCollection nimmt dann die liste die von dem DataAccessLayer über den businessLayer geht
        private ObservableCollection<Tour> tours;


        private ObservableCollection<Log> logs;

        private Tour currentTour;

        private Log currentLog;

        private RelayCommand editlogs;
        public ICommand EditLogsCommand => editlogs ??= new RelayCommand(EditLogs);

        private RelayCommand delete_log;
        public ICommand DeleteLogCommand => delete_log ??= new RelayCommand(DeleteLog);

        private RelayCommand addTourCommand;
        public ICommand AddTourCommand => addTourCommand ??= new RelayCommand(AddTour);

        private RelayCommand addLog;
        public ICommand AddLogCommand => addLog ??= new RelayCommand(AddLog);

        private ICommand deleteTourCommand;
        public ICommand DeleteTourCommand => deleteTourCommand ??= new RelayCommand(Delete);




        //Pop Up Delted Successfully 
        private ICommand openHelpCommand;
        private ICommand closeHelpCommand;
        private bool isHelpVisible;

        public ICommand OpenHelpCommand
        {
            get
            {
                if (openHelpCommand == null)
                    openHelpCommand = new RelayCommand(OpenHelp);
                return openHelpCommand;
            }
        }

        private void OpenHelp(object param)
        {
            IsHelpVisible = true;
        }

        public ICommand CloseHelpCommand
        {
            get
            {
                if (closeHelpCommand == null)
                    closeHelpCommand = new RelayCommand(CloseHelp);
                return closeHelpCommand;
            }
        }

        private void CloseHelp(object param)
        {
            IsHelpVisible = false;
        }

        public bool IsHelpVisible
        {
            get { return isHelpVisible; }
            set
            {
                if (isHelpVisible == value)
                    return;
                isHelpVisible = value;
                RaisePropertyChangedEvent("IsHelpVisible");
            }
        }

       // ////////////

        private void DeleteLog(object obj)
        {
            if(currentLog != null && currentTour != null)
            {
                itemFactory.DeleteLog(currentTour.UUID, currentLog.UUID);
                if (currentTour != null)
                {
                    currentTour.LogItems = itemFactory.GetTourLogs(currentTour.UUID);
                    Logs = currentTour.LogItems;
                }
                OpenHelp( obj);
            }
        }

        public void RefreshLogsForEditWindow()
        {
            if (currentLog != null)
            {
                editLogViewModel.Date_Time = CurrentLog.Date_Time;
                editLogViewModel.Distance = CurrentLog.Distance;
                editLogViewModel.TotalTime = CurrentLog.TotalTime;
                editLogViewModel.Report = CurrentLog.Report;
                editLogViewModel.Rating = CurrentLog.Rating;
                editLogViewModel.AvarageSpeed = CurrentLog.AvarageSpeed;
                editLogViewModel.Comment = CurrentLog.Comment;
                editLogViewModel.Problems = CurrentLog.Problems;
                editLogViewModel.TransportModus = CurrentLog.TransportModus;
                editLogViewModel.Recomended = CurrentLog.Recomended;
            }
        }
        private void EditLogs(object obj)
        {
            if (currentTour != null && currentLog != null)
            {
                EditLogsView editLogView = new EditLogsView();
                editLogView.DataContext = editLogViewModel;

                //um den angeklickten Log und die Tour ID im anderen View zu bekommen
                editLogViewModel.CurrentTour = CurrentTour;
                //Um die daten im neuen fenster einzeigen zu können
                editLogViewModel.CurrentLog = CurrentLog;
                RefreshLogsForEditWindow();

                bool? dialogResult = editLogView.ShowDialog();
                if (!(bool)dialogResult)
                {
                    tours.Clear();
                    RefreshingListItems();

                }
            }
            else
            {
                return;
            }

        }
        private void AddLog(object obj)
        {
            if(currentTour!=null)
            {
                AddLogView addLogView = new AddLogView();
                addLogView.DataContext = addLogViewModel;
                addLogViewModel.CurrentTour = CurrentTour;
               
                bool? dialogResult = addLogView.ShowDialog();
                if (!(bool)dialogResult)
                {
                    tours.Clear();
                    RefreshingListItems();
                    
                }
            }
            else
            {
                return;
            }
            
        }


        private void AddTour(object obj)
        {
            //view Object erstellen und DataContext auf dessen Viewmodel setzen.

            AddTourView addTourView = new AddTourView();
            addTourView.DataContext = addTourViewModel;
            bool? dialogResult = addTourView.ShowDialog();
            if (!(bool)dialogResult)
            {
                tours.Clear();
                RefreshingListItems();
            }
        }

        

        private void Delete(object obj)
        {

            if (currentTour == null)
                return;

            itemFactory.SavePathAndDeleteTour(currentTour);
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

        public ObservableCollection<Log> Logs
        {
            get
            {

                return logs;
                
            }
            set
            {
                if (logs != value)
                {
                    logs = value;
                    RaisePropertyChangedEvent(nameof(Logs));
                }
            }
        }


        public Log CurrentLog
        {
            get
            {

                return currentLog;
            }
            set
            {
                if (currentLog != value)
                {

                    currentLog = value;
                    RaisePropertyChangedEvent(nameof(CurrentLog));                 
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

                    //Jedes mal wenn sich der currenttour ändert, wird die liste der logs vom item entleert und wieder aufgefüllt.
                    if(currentTour != null)
                    {
                        currentTour.LogItems = itemFactory.GetTourLogs(currentTour.UUID);
                        Logs = currentTour.LogItems;
                    }
                    
                }
            }
        }

        /*public string CurrentTourDateLog
        {
            get
            {
                CurrentTour.
            }
        }*/
      

        public MainWindowViewModel()
        {
            addTourViewModel = new AddTourViewModel();
            addLogViewModel = new AddLogViewModel();
            editLogViewModel = new EditLogsViewModel();
            itemFactory = TourItemFactory.GetInstance();
            ReadListBox();

        }

        ~MainWindowViewModel()
        {
            itemFactory.DeleteImages();
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
