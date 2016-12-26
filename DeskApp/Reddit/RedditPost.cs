using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DeskApp.Reddit
{
    public class RedditPost : INotifyPropertyChanged
    {
        private string Title { get; set; }
        private string Author { get; set; }
        private string Thumbnail { get; set; }


        public RedditPost(string title, string author, string thumbnail)
        {
            this.Title = title;
            this.Author = author;
            this.Thumbnail = thumbnail;
        }

        public string title
        {
            get { return Title; }
            set { Title = value;  OnPropertyChanged("title"); }
        }

        public string author
        {
            get { return Author; }
            set { Author = value;  OnPropertyChanged("author"); }
        } 

        public string thumbnail
        {
            get { return Thumbnail; }
            set { Thumbnail = value;  OnPropertyChanged("thumbnail");  }
        }

        public static ObservableCollection<RedditPost> fetchRedditPosts(string subreddit)
        {
            // Compose the query URL.
            string url = "https://www.reddit.com/r/" + subreddit + "/.rss";
            Console.WriteLine("url is :" + url);

            RedditParseXML parser = new RedditParseXML();
            try
            {
                parser.GetFormattedXml(url);
            }
            catch (Exception e)
            {
                return null;
            }
            ObservableCollection<RedditPost> test = new ObservableCollection<RedditPost>();
            test = parser.parseXMLDoc();
            return test;
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

    }
}
