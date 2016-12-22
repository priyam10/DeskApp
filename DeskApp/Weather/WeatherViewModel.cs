using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DeskApp.Weather
{
    class WeatherViewModel : INotifyPropertyChanged
    {
        private IList<Weather> _WeatherForecastList;
        private Weather NowWeather;
        private const string API_KEY = "2db5a5da5c3a27d247f2423a226fd5bf";
        // Query URLs. Replace @LOC@ with the location.
        private const string CurrentUrl = "http://api.openweathermap.org/data/2.5/weather?" + "q=@LOC@&mode=xml&units=imperial&APPID=" + API_KEY;
        private const string ForecastUrl = "http://api.openweathermap.org/data/2.5/forecast?" + "q=@LOC@&mode=xml&units=imperial&APPID=" + API_KEY;

        public WeatherViewModel()
        {
            _WeatherForecastList = new List<Weather> { };
            NowWeather = new Weather();
            if(!String.IsNullOrEmpty(Properties.Settings.Default.FavLocation))
            {
                this.currLocation = Properties.Settings.Default.FavLocation;
                fetchCurrentWeather();
            }
        }

        //public IList<Weather> WeatherForecast
        //{
        //    get { return _WeatherForecastList; }
        //    set { _WeatherForecastList = value; }
        //}

        // Get current conditions.
        private void fetchCurrentWeather()
        {
            NowWeather.currLocation = this.currLocation;
            // Compose the query URL.
            string url = CurrentUrl.Replace("@LOC@", this.currLocation);
            //txtXml.Text = GetFormattedXml(url);

            ParseXML parser = new ParseXML();
            parser.GetFormattedXml(url);
            Weather temp_weather = parser.parseXMLDoc();
            this.currDesc = temp_weather.currDesc;
            this.currDegrees = temp_weather.currDegrees + " °F";
            Console.WriteLine("nowweather curr_desc : " + NowWeather.currDesc);
            //this.currDesc = NowWeather.currDesc;
          
        }

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
                this.CurrentLocation = value;
                OnPropertyChanged("currLocation");
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

        private ICommand _getCurrWeatherClick;

        public ICommand GetCurrWeatherClick
        {
            get
            {
                if (_getCurrWeatherClick == null)
                {
                    _getCurrWeatherClick = new RelayCommand(
                        param => this.SaveObject(),
                        param => this.CanSave()
                    );
                }
                return _getCurrWeatherClick;
            }
        }

        private bool CanSave()
        {
            // Verify command can be executed here
            return true;
        }

        private void SaveObject()
        {
            // Save command execution logic
            Properties.Settings.Default.FavLocation = this.currLocation;
            Properties.Settings.Default.Save();
            fetchCurrentWeather();
        }

        private void populateCurrWeather()
        {
            
        }

    }
}
