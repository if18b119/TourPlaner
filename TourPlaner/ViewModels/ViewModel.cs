using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlaner.ViewModels
{
    //T - generisches Typ argument - Der datenTyp T ist fllexibel - Basisklasse, typen kann man selber bestimmen
    public abstract class ViewModel <T>:ViewModelBase
    {
        public T Model { get; set; }
    }
}
