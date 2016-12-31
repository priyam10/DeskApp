using DeskApp.Weather;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DeskApp.Reddit
{
    public class RedditPost : INotifyPropertyChanged
    {
        private string Title { get; set; }
        private string Author { get; set; }
        private string Thumbnail { get; set; }
        private string ShowThumbnail { get; set; }
        private string CommentsUrl { get; set; }
        private string PostsView { get; set; }

        public RedditPost(string title, string author, string thumbnail)
        {
            this.Title = title;
            this.Author = author;
            this.Thumbnail = thumbnail;
        }

        public RedditPost()
        {
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

        public string showThumbnail
        {
            get { return ShowThumbnail; }
            set { ShowThumbnail = value; OnPropertyChanged("showThumbnail"); }
        }

        public string commentsUrl
        {
            get { return CommentsUrl; }
            set { CommentsUrl = value; OnPropertyChanged("commentsUrl"); }
        }

        public string postsView
        {
            get { return PostsView; }
            set { PostsView = value; OnPropertyChanged("postsView"); }
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

        private ICommand openRedditPost;

        public ICommand OpenRedditPost
        {
            get
            {
                if (openRedditPost == null)
                {
                    openRedditPost = new RelayCommand(
                        param => this.OnRedditPostClick()
                    );
                }
                return openRedditPost;
            }
        }

        public void OnRedditPostClick()
        {         
            postsView = "Hidden";

            string url = commentsUrl + "/.rss";
            RedditParseXML parser = new RedditParseXML();
            try
            {
                parser.GetFormattedXml(url);
            }
            catch (Exception e)
            {
                return;
            }
            ObservableCollection<RedditPost> test = new ObservableCollection<RedditPost>();
            parser.parseCommentsXml();
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
