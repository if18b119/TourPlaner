using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TourPlaner.ViewModels;

namespace TourPlaner
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        
        void App_Startup(object sender, StartupEventArgs e)
        {
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel();
            MainWindow mainWindow = new MainWindow();
            mainWindow.DataContext = mainWindowViewModel;

            mainWindow.Show();

     
             
        }
    }
}
