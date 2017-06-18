using GoogleMapsDemo.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GoogleMapsDemo.ViewModel
{
    class MainPageViewModel : INotifyPropertyChanged
    {

        public MainPageViewModel()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<LocationEventModel> locationEvents;

        public ObservableCollection<LocationEventModel> LocationEvents
        {
            get
            {
                return locationEvents;
            }
            set
            {
                locationEvents = value;
                RaisePropertyChanged("LocationEvents");
            }
        }
    }
}
