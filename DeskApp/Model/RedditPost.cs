﻿using DeskApp.Weather;
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
        private ObservableCollection<RedditComment> CommentsList;

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
            set { Title = value; OnPropertyChanged("title"); }
        }

        public string author
        {
            get { return Author; }
            set { Author = value; OnPropertyChanged("author"); }
        }

        public string thumbnail
        {
            get { return Thumbnail; }
            set { Thumbnail = value; OnPropertyChanged("thumbnail"); }
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

        public ObservableCollection<RedditComment> commentsList
        {
            get
            {
                if (CommentsList == null)
                {
                    CommentsList = new ObservableCollection<RedditComment>();
                }
                return CommentsList;
            }
            set
            {
                if (CommentsList == null)
                {
                    CommentsList = new ObservableCollection<RedditComment>();
                }
                foreach (RedditComment rc in value)
                {
                    CommentsList.Add(rc);
                }
                OnPropertyChanged("commentsList");
            }
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


    public class RedditComment : INotifyPropertyChanged
    {
        private string Title { get; set; }
        private string Author { get; set; }
        private string Avatar { get; set; }
        private string Time { get; set; }

        public string title
        {
            get { return Title; }
            set { Title = value; OnPropertyChanged("title"); }
        }

        public string author
        {
            get { return Author; }
            set { Author = value; OnPropertyChanged("author"); }
        }

        public string avatar
        {
            get { return Avatar; }
            set { Avatar = value; OnPropertyChanged("avatar"); }
        }

        public string time
        {
            get { return Time; }
            set { Time = value; OnPropertyChanged("time"); }
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
