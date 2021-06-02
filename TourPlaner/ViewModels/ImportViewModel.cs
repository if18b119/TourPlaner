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
    public class ImportViewModel : ViewModelBase
    {
        //Logging -Instanz
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private RelayCommand keep_Data;
        public ICommand KeepOldDataCommand => keep_Data ??= new RelayCommand(KeepOldData);
        public ITourItemFactory itemFactory;
        private void KeepOldData(object obj)
        {
            Window tmp = (Window)obj;
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();

            // Launch OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = openFileDlg.ShowDialog();
            // Get the selected file name and display in a TextBox.
            // Load content of file in a TextBlock
            if (result == true)
            {
                if (itemFactory.Import(openFileDlg.FileName))
                {
                    string _log = "{\"File Import \",\"successfully\":{1}}";
                    log.Info(_log);
                    MessageBox.Show("File imported successfully!");
                }
                else
                {
                    MessageBox.Show("Error while importing file!");
                }

            }
            tmp.Close();
        }

        private RelayCommand delete_old_Data;
        public ICommand DeleteOldDataCommand => delete_old_Data??= new RelayCommand(DeleteOldData);

        private void DeleteOldData(object obj)
        {
            Window tmp = (Window)obj;
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();

            // Launch OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = openFileDlg.ShowDialog();
            // Get the selected file name and display in a TextBox.
            // Load content of file in a TextBlock
            if (result == true)
            {
                if (itemFactory.ImportAndDelete(openFileDlg.FileName))
                {
                    string _log = "{\"File Import \",\"successfully\":{1}}";
                    log.Info(_log);
                    MessageBox.Show("File imported successfully!");
                }
                else
                {
                    MessageBox.Show("Error while importing file!");
                }
            }
            tmp.Close();
        }

        public ImportViewModel()
        {
            itemFactory = TourItemFactory.GetInstance();
        }
    }
}
