using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DeskApp.Weather
{
    class Weather : INotifyPropertyChanged
    {
        private const string API_KEY = "2db5a5da5c3a27d247f2423a226fd5bf";
        private string CurrentUrl = "http://api.openweathermap.org/data/2.5/weather?" + "q=@LOC@&mode=xml&units=imperial&APPID=" + API_KEY;
        private string ForecastUrl = "http://api.openweathermap.org/data/2.5/forecast?" + "q=@LOC@&mode=xml&units=imperial&APPID=" + API_KEY;
        private string CurrentDegrees;
        private string CurrentDesc { get; set; }
        private string CurrentLocation { get; set; }

        // publicly currDegrees, privately CurrentDegrees
        public string currDegrees
        {
            get
            {
                return CurrentDegrees;
            }
            set
            {
                CurrentDegrees = value;
                OnPropertyChanged("currDegrees");
            }
        }

        // publicly currDesc, privately CurrentDesc
        public string currDesc
        {
            get
            {
                return CurrentDesc;
            }
            set
            {
                CurrentDesc = value;
                OnPropertyChanged("currDesc");
            }
        }

        // publicly currLocation, privately CurrentLocation
        public string currLocation
        {
            get
            {
                return CurrentLocation;
            }
            set
            {
                CurrentLocation = value;
                OnPropertyChanged("currLocation");
            }
        }


        // publicly currUrl, privately CurrentUrl
        public string currUrl
        {
            get
            {
                return CurrentUrl;
            }
            set
            {
                CurrentUrl = value;
                OnPropertyChanged("currUrl");
            }
        }

        // publicly forecastUrl, privately ForecastUrl
        public string forecastUrl
        {
            get
            {
                return ForecastUrl;
            }
            set
            {
                ForecastUrl = value;
                OnPropertyChanged("forecastUrl");
            }
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion



        //Update stuff
        private ICommand mUpdater;
        public ICommand UpdateCommand
        {
            get
            {
                if (mUpdater == null)
                    mUpdater = new Updater();
                return mUpdater;
            }
            set
            {
                mUpdater = value;
            }
        }
        private class Updater : ICommand
        {
            #region ICommand Members
            public bool CanExecute(object parameter)
            {
                return true;
            }
            public event EventHandler CanExecuteChanged;
            public void Execute(object parameter)
            {
            }
            #endregion
        }



    }
}
