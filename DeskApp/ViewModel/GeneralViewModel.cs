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
    class GeneralViewModel : INotifyPropertyChanged //,ObservableCollection<RedditPost>
    {
        private Weather NowWeather;
        private ObservableCollection<Weather> WeatherList;
        private string MySubreddit;
        private ObservableCollection<RedditPost> RedditPostList;
        private string PostsView { get; set; }
        private string CommentsView { get; set; }

        public GeneralViewModel()
        {
                        
            NowWeather = new Weather();
            WeatherList = new ObservableCollection<Weather>();
            RedditPostList = new ObservableCollection<RedditPost>();
            if(!String.IsNullOrEmpty(Properties.Settings.Default.FavLocation))
            {
                nowWeather.currLocation = Properties.Settings.Default.FavLocation;
                NowWeather.fetchCurrentWeather();
                ObservableCollection<Weather> temp_list = Weather.fetchWeatherForecast(NowWeather.currLocation);
                //WeatherList.Clear();
                foreach (Weather w in temp_list)
                {
                    Weather weather_item = new Weather();
                    weather_item.currDegrees = w.currDegrees;
                    weather_item.currDesc = w.currDesc;
                    weather_item.forecastTime = w.forecastTime;
                    WeatherList.Add(weather_item);
                }

            }
            if (!String.IsNullOrEmpty(Properties.Settings.Default.LastSubreddit))
            {
                MySubreddit = Properties.Settings.Default.LastSubreddit;
                ObservableCollection<RedditPost> temp_posts = RedditPost.fetchRedditPosts(MySubreddit);
                RedditPostList.Clear();
                foreach (RedditPost rp in temp_posts)
                {
                    RedditPost new_redditpost = new RedditPost(rp.title, rp.author, rp.thumbnail);
                    new_redditpost.showThumbnail = rp.showThumbnail;
                    new_redditpost.commentsUrl = rp.commentsUrl;
                    RedditPostList.Add(new_redditpost);
                }
            }
        }

        public Weather nowWeather
        {
            get { return NowWeather; }
            set { NowWeather = value; OnPropertyChanged("nowWeather"); }
        }

        public ObservableCollection<Weather> weatherList
        {
            get
            {
                if (WeatherList == null)
                {
                    WeatherList = new ObservableCollection<Weather>();
                }
                return WeatherList;
            }
            set
            {
                if (WeatherList == null)
                {
                    WeatherList = new ObservableCollection<Weather>();
                }
                foreach (Weather w in value)
                {
                    WeatherList.Add(w);
                }
                OnPropertyChanged("weatherList");
            }
        }
    

        public string mySubreddit
        {
            get { return MySubreddit;  }
            set { MySubreddit = value; OnPropertyChanged("mySubreddit");  }
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

        public string postsView
        {
            get { return PostsView; }
            set { PostsView = value; OnPropertyChanged("postsView"); }
        }

        public string commentsView
        {
            get { return CommentsView; }
            set { CommentsView = value; OnPropertyChanged("commentsView"); }
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
            Properties.Settings.Default.FavLocation = nowWeather.currLocation;
            Properties.Settings.Default.Save();
            NowWeather.fetchCurrentWeather();
            //Fetch forecast
            ObservableCollection<Weather> temp_list = Weather.fetchWeatherForecast(NowWeather.currLocation);
            //WeatherList.Clear();
            foreach (Weather w in temp_list)
            {
                Weather weather_item = new Weather();
                weather_item.currDegrees = w.currDegrees;
                weather_item.currDesc = w.currDesc;
                weather_item.forecastTime = w.forecastTime;                
                WeatherList.Add(weather_item);
            }
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
                //RedditPostList.Add(new RedditPost(rp.title, rp.author, rp.thumbnail));

                RedditPost new_redditpost = new RedditPost(rp.title, rp.author, rp.thumbnail);
                new_redditpost.showThumbnail = rp.showThumbnail;
                new_redditpost.commentsUrl = rp.commentsUrl;
                RedditPostList.Add(new_redditpost);
            }
            //RedditPostList = temp_list;
        }


        private ICommand openRedditPost;

        public ICommand OpenRedditPost
        {
            get
            {
                if (openRedditPost == null)
                {
                    openRedditPost = new RelayCommand(
                        param => this.OnRedditPostClick(param)
                    );
                }
                return openRedditPost;
            }
        }

        public void OnRedditPostClick(object e)
        {
            this.postsView = "Collapsed";
            this.CommentsView = "Visible";
            string url = (e as string) + "/.rss";
            //RedditParseXML parser = new RedditParseXML();
            //try
            //{
            //    parser.GetFormattedXml(url);
            //}
            //catch (Exception e)
            //{
            //    return;
            //}
            //ObservableCollection<RedditComment> test = new ObservableCollection<RedditComment>();
            //test = parser.parseCommentsXml();
        }

    }
}
