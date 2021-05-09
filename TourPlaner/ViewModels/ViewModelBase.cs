using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace TourPlaner.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged 
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //Wenn sich ein property mit einem gewisse namen ändert wird ein event geschossen um die ui zu notifizieren
        protected void RaisePropertyChangedEvent([CallerMemberName] string propertyName = "")
        {
            ValidatePropertyName(propertyName);
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //ist ein validator um zu überprüfen ob es den namen überhaupt gibt
        protected void ValidatePropertyName(string propertyName)
        {
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                throw new ArgumentException("Invalid property name: " + propertyName);
            }
        }
    }
}
