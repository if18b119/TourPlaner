using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlaner.Models;

namespace TourPlaner.ViewModels
{
    public class TourViewModel:ViewModel<Tour>
    {
        public string Name
        {
            get
            {
                return Model.Name;
            }
            set
            {
                if (Model.Name != value)
                {
                    Model.Name = value;
                    RaisePropertyChangedEvent(nameof(Name));
                }
            }
        }

        public string Description
        {
            get
            {
                return Model.Description;
            }
            set
            {
                if (Model.Description != value)
                {
                    Model.Description = value;
                    RaisePropertyChangedEvent(nameof(Description));
                }
            }
        }

        public string From
        {
            get
            {
                return Model.From;
            }
            set
            {
                if (Model.From != value)
                {
                    Model.From = value;
                    RaisePropertyChangedEvent(nameof(From));
                }
            }
        }

        public string To
        {
            get
            {
                return Model.To;
            }
            set
            {
                if (Model.To != value)
                {
                    Model.To = value;
                    RaisePropertyChangedEvent(nameof(To));
                }
            }
        }

        public RouteType Route_Type
        {
            get
            {
                return Model.Route_Type;
            }
            set
            {
                if (Model.Route_Type != value)
                {
                    Model.Route_Type = value;
                    RaisePropertyChangedEvent(nameof(Route_Type));
                }
            }
        }

        public TourViewModel()
        {
            Model = new Tour();
        }
    }
}
