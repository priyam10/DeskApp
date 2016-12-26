using DeskApp.Reddit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DeskApp.Weather
{
    class WeatherViewModel : INotifyPropertyChanged //,ObservableCollection<RedditPost>
    {
        private IList<Weather> _WeatherForecastList;
        private Weather NowWeather;
        private string MySubreddit;
        private ObservableCollection<RedditPost> RedditPostList;        
       
        public WeatherViewModel()
        {
            _WeatherForecastList = new List<Weather> { };
            NowWeather = new Weather();
            RedditPostList = new ObservableCollection<RedditPost>();
            if(!String.IsNullOrEmpty(Properties.Settings.Default.FavLocation))
            {
                nowWeather.currLocation = Properties.Settings.Default.FavLocation;
                NowWeather.fetchCurrentWeather();
            }
            if (!String.IsNullOrEmpty(Properties.Settings.Default.LastSubreddit))
            {
                MySubreddit = Properties.Settings.Default.LastSubreddit;
                //RedditPostList =  RedditPost.fetchRedditPosts(MySubreddit);
                ObservableCollection<RedditPost> temp_posts = RedditPost.fetchRedditPosts(MySubreddit);
                RedditPostList.Clear();
                foreach (RedditPost rp in temp_posts)
                {                   
                    RedditPostList.Add(new RedditPost(rp.title, rp.author, rp.thumbnail));
                }
            }
        }

        public Weather nowWeather
        {
            get { return NowWeather; }
            set { NowWeather = value; OnPropertyChanged("nowWeather"); }
        }

        public string mySubreddit
        {
            get { return MySubreddit;  }
            set { MySubreddit = value;  OnPropertyChanged("mySubreddit");  }
        }

        public ObservableCollection<RedditPost> redditPostList
        {
            get {
                if (RedditPostList == null)
                {
                    RedditPostList = new ObservableCollection<RedditPost>();
                }
                return RedditPostList;
            }
            set {
                if (RedditPostList == null)
                {
                    RedditPostList = new ObservableCollection<RedditPost>();
                }
                foreach(RedditPost rp in value)
                {
                    RedditPostList.Add(rp);
                }
                OnPropertyChanged("redditPostList");
            }
        }

        //public IList<Weather> WeatherForecast
        //{
        //    get { return _WeatherForecastList; }
        //    set { _WeatherForecastList = value; }
        //}


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
            Properties.Settings.Default.FavLocation = nowWeather.currLocation;
            Properties.Settings.Default.Save();
            NowWeather.fetchCurrentWeather();
        }


        private ICommand _getSubredditClick;

        public ICommand GetSubredditClick
        {
            get
            {
                if (_getSubredditClick == null)
                {
                    _getSubredditClick = new RelayCommand(
                        param => this.SaveRedditObject(),
                        param => this.RedditCanSave()
                    );
                }
                return _getSubredditClick;
            }
        }

        private bool RedditCanSave()
        {
            return true;
        }

        private void SaveRedditObject()
        {
            // Save command execution logic
            Properties.Settings.Default.LastSubreddit = MySubreddit;
            Properties.Settings.Default.Save();
            ObservableCollection<RedditPost> temp_list = RedditPost.fetchRedditPosts(MySubreddit);
            RedditPostList.Clear();
            foreach(RedditPost rp in temp_list)
            {
                RedditPostList.Add(new RedditPost(rp.title, rp.author, rp.thumbnail));
            }
            //RedditPostList = temp_list;
        }

    }
}
