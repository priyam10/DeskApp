using DeskApp.Model;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Xml;

namespace DeskApp.Reddit
{
    class RedditParseXML
    {
        string html_string;


        public void GetFormattedXml(string url)
        {
            try {
                // Create a web client.
                using (WebClient client = new WebClient())
                {
                    // Get the response string from the URL.
                    string html = client.DownloadString(url);

                    // Load the response into an XML document.
                    HtmlDocument html_document = new HtmlDocument();
                    html_document.LoadHtml(html);
                    this.html_string = html_document.DocumentNode.OuterHtml;
                    // Format the XML.
                    //using (StringWriter string_writer = new StringWriter())
                    //{
                    //    XmlTextWriter xml_text_writer = new XmlTextWriter(string_writer);
                    //    xml_text_writer.Formatting = Formatting.Indented;
                    //    xml_document.WriteTo(xml_text_writer);

                    //    // Return the result.
                    //    this.xml_string = string_writer.ToString();
                }
            }catch(Exception e)
            {
                Console.WriteLine("u wut m8");
            }

        }



        public ObservableCollection<RedditPost> parseXMLDoc()
        {
            //HtmlNode.ElementsFlags.Remove("form");
            ObservableCollection<RedditPost> tempPost = new ObservableCollection<RedditPost>();
            HtmlDocument doc = new HtmlDocument();
            //doc.OptionAutoCloseOnEnd = true;
            //doc.OptionDefaultStreamEncoding = Encoding.UTF8;
            doc.LoadHtml(html_string);

            HtmlNodeCollection nodelist = doc.DocumentNode.SelectNodes("//entry");

            foreach (HtmlNode node in nodelist)
            {
                try
                {
                    string author = node.SelectSingleNode("author//name").InnerText;
                    string title = node.SelectSingleNode("title").InnerText;
                    string time = node.SelectSingleNode("updated").InnerText;
                    string comments = node.SelectSingleNode("link").OuterHtml;
                    RedditPost reddit_entry = new RedditPost();
                    reddit_entry.title = title;
                    if (author.Contains("/u/"))
                    {
                        author = author.Substring(3);
                    }

                    reddit_entry.author = "Submitted " + parseTime(time) + " by " + author;
                    string[] url_parse = comments.Split('"');
                    comments = url_parse[1];
                    reddit_entry.commentsUrl = comments + ".rss";

                    reddit_entry.showThumbnail = "Collapsed";
                    string img_node = node.SelectSingleNode("content").InnerHtml;
                    if (!String.IsNullOrEmpty(img_node) && img_node.Contains("img src"))
                    {
                        int start_thumb = img_node.IndexOf("img src=&quot;") + 14;
                        int end_thumb = img_node.IndexOf(".jpg", start_thumb);
                        if (start_thumb != -1 && end_thumb != -1)
                        {
                            string thumb_url = img_node.Substring(start_thumb, end_thumb - start_thumb + 4);
                            reddit_entry.thumbnail = thumb_url;
                            //Console.WriteLine("title: " + title + "thumb: " + thumb_url);
                            reddit_entry.showThumbnail = "Visible";
                        }

                    }
                   
                    tempPost.Add(reddit_entry);
                }
                catch (Exception e)
                {
                    //Console.WriteLine(e.Message);
                }

            }

            return tempPost;
        }


        public ObservableCollection<RedditComment> parseCommentsXml()
        {
            ObservableCollection<RedditComment> tempComment = new ObservableCollection<RedditComment>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html_string);
            DateTime currTime = DateTime.Now;
            HtmlNodeCollection nodelist = doc.DocumentNode.SelectNodes("//entry");

            foreach (HtmlNode node in nodelist)
            {
                //Regex for extracting all urls from content tag
                //var linkParser = new Regex(@"\b(?:https?://|www\.)\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                //var rawString = node.ChildNodes.Item(2).InnerText;
                //foreach (Match m in linkParser.Matches(rawString))
                //{
                //    MessageBox.Show(m.Value);
                //}


                try
                {
                    string writer = node.SelectSingleNode("author//name") == null? null : node.SelectSingleNode("author//name").InnerText;
                    string content = node.SelectSingleNode("content").InnerHtml;
                    string time = node.SelectSingleNode("updated").InnerText;
                    RedditComment reddit_comment = new RedditComment();
                    if (writer != null && writer.Contains("/u/"))
                    {
                        writer = writer.Substring(3);
                    }
                    reddit_comment.writer = writer;
                    reddit_comment.content = content;
                    //string date_time = time.Split(T);
                    var regex = new Regex(Regex.Escape("T"));
                    string parsedTime = regex.Replace(time, " ", 1);
                    DateTime writeTime = DateTime.ParseExact(parsedTime.Substring(0, parsedTime.Length - 6), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    int hours = (currTime - writeTime).Hours;
                    if (hours >= 24)
                    {
                        time = (currTime - writeTime).Days.ToString() + " days ago";
                    }
                    else
                    {
                        time = (currTime - writeTime).Hours.ToString() + " hours ago";
                    }
                    reddit_comment.time = time;
                    tempComment.Add(reddit_comment);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }

            return tempComment;
        }

        private string parseTime(string time)
        {
            DateTime currTime = DateTime.Now;
            string output;
            var regex = new Regex(Regex.Escape("T"));
            string parsedTime = regex.Replace(time, " ", 1);
            DateTime writeTime = DateTime.ParseExact(parsedTime.Substring(0, parsedTime.Length - 6), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            int hours = (currTime - writeTime).Hours;
            if (hours >= 24)
            {
                output = (currTime - writeTime).Days.ToString() + " days ago";
            }
            else
            {
                output = (currTime - writeTime).Hours.ToString() + " hours ago";
            }
            return output;
        }
    }
}
