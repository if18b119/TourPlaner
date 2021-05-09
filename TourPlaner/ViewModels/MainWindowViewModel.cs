﻿using System;
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
        private string search_name;

        public ITourItemFactory itemFactory;

        public AddTourViewModel addTourViewModel;
        public AddLogViewModel addLogViewModel;
        public EditLogsViewModel editLogViewModel;

        //die ObservableCollection nimmt dann die liste die von dem DataAccessLayer über den businessLayer geht
        private ObservableCollection<Tour> tours;


        private ObservableCollection<Log> logs;

        private Tour currentTour;

        private Log currentLog;

        private RelayCommand search;
        public ICommand SearchCommand => search ??= new RelayCommand(Search);

        private RelayCommand clear;
        public ICommand ClearCommand => clear ??= new RelayCommand(Clear);

        private RelayCommand editlogs;
        public ICommand EditLogsCommand => editlogs ??= new RelayCommand(EditLogs);

        private RelayCommand delete_log;
        public ICommand DeleteLogCommand => delete_log ??= new RelayCommand(DeleteLog);

        private RelayCommand addTourCommand;
        public ICommand AddTourCommand => addTourCommand ??= new RelayCommand(AddTour);

        private RelayCommand addLog;
        public ICommand AddLogCommand => addLog ??= new RelayCommand(AddLog);

        private RelayCommand deleteTourCommand;
        public ICommand DeleteTourCommand => deleteTourCommand ??= new RelayCommand(Delete);


        private RelayCommand makepdflog;
        public ICommand MakePdfCommand => makepdflog ??= new RelayCommand(MakePdf);


        //Pop Up Deleted Successfully 
        // //////////////////////////////
        private ICommand openDeleteCommand;
        private ICommand closeDeleteCommand;
        private bool isDeleteVisible;

        public ICommand OpenDeleteCommand
        {
            get
            {
                if (openDeleteCommand == null)
                    openDeleteCommand = new RelayCommand(OpenDelete);
                return openDeleteCommand;
            }
        }

        private void OpenDelete(object param)
        {
            IsDeleteVisible = true;
        }

        public ICommand CloseDeleteCommand
        {
            get
            {
                if (closeDeleteCommand == null)
                    closeDeleteCommand = new RelayCommand(CloseDelete);
                return closeDeleteCommand;
            }
        }

        private void CloseDelete(object param)
        {
            IsDeleteVisible = false;
        }

        public bool IsDeleteVisible
        {
            get { return isDeleteVisible; }
            set
            {
                if (isDeleteVisible == value)
                    return;
                isDeleteVisible = value;
                RaisePropertyChangedEvent("IsDeleteVisible");
            }
        }

        // ////////////
        //Pop Up Added Successfully 
        // //////////////////////////////
        private ICommand openAddedPdfCommand;
        private ICommand closeAddedPdfCommand;
        private bool isAddedPdfVisible;

        public ICommand OpenAddedPdfCommand
        {
            get
            {
                if (openAddedPdfCommand == null)
                    openAddedPdfCommand = new RelayCommand(OpenAddedPdf);
                return openAddedPdfCommand;
            }
        }

        private void OpenAddedPdf(object param)
        {
            IsAddedPdfVisible = true;
        }

        public ICommand CloseAddedPdfCommand
        {
            get
            {
                if (closeAddedPdfCommand == null)
                    closeAddedPdfCommand = new RelayCommand(CloseAddedPdf);
                return closeAddedPdfCommand;
            }
        }

        private void CloseAddedPdf(object param)
        {
            IsAddedPdfVisible = false;           
        }

        public bool IsAddedPdfVisible
        {
            get { return isAddedPdfVisible; }
            set
            {
                if (isAddedPdfVisible == value)
                    return;
                isAddedPdfVisible = value;
                RaisePropertyChangedEvent("IsAddedPdfVisible");
            }
        }
        // ///////////////

        private void MakePdf(object obj)
        {
            if(currentTour == null || currentTour.LogItems.Count()==0)
            {
                return;
            }
            else
            {
                itemFactory.MakePdf(currentTour);
                OpenAddedPdf(obj);
            }
        }
       

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
                OpenDelete(obj);
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
            OpenDelete(obj);
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

        public string SearchName
        {
            get 
            {
                return search_name;
            }
            set
            {
                if (search_name == value)
                    return;
                search_name = value;
                RaisePropertyChangedEvent("SearchName");
            }
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

            if(tours.Count==0)
            {
                MessageBox.Show("No Data Available to search for!");
            }
            else
            {
                IEnumerable<Tour> search_outcome = itemFactory.Search(SearchName);
                Tours.Clear();
                foreach(Tour t in search_outcome)
                {
                    Tours.Add(t);
                }
            }

        }


        private void Clear(object commandParameter)
        {
            Tours.Clear();
            SearchName = string.Empty;
            RefreshingListItems();
        }

       
    }
}
