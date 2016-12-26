using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace DeskApp.Reddit
{
    class RedditParseXML
    {
        string xml_string;


        public void GetFormattedXml(string url)
        {
            // Create a web client.
            using (WebClient client = new WebClient())
            {
                // Get the response string from the URL.
                string xml = client.DownloadString(url);

                // Load the response into an XML document.
                XmlDocument xml_document = new XmlDocument();
                xml_document.LoadXml(xml);

                // Format the XML.
                using (StringWriter string_writer = new StringWriter())
                {
                    XmlTextWriter xml_text_writer = new XmlTextWriter(string_writer);
                    xml_text_writer.Formatting = Formatting.Indented;
                    xml_document.WriteTo(xml_text_writer);

                    // Return the result.
                    this.xml_string = string_writer.ToString();
                }
            }

        }

        public ObservableCollection<RedditPost> parseXMLDoc()
        {
            ObservableCollection<RedditPost> tempPost = new ObservableCollection<RedditPost>();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xml_string);

            XmlNodeList nodelist = xml.GetElementsByTagName("entry");

            foreach (XmlNode node in nodelist) // for each <testcase> node
            {
                try
                {                   
                    string author = node.ChildNodes.Item(0).FirstChild.InnerText;
                    string title = node.LastChild.InnerText;
                    RedditPost reddit_entry = new RedditPost(title, author, "Null");
                    if (node.InnerText.Contains("thumbs.redditmedia"))
                    {
                        int start_thumb = node.InnerText.IndexOf("https://b.thumbs.redditmedia");
                        int end_thumb = node.InnerText.IndexOf(".jpg");
                        if (start_thumb != -1 && end_thumb != -1) {
                            string thumb_url = node.InnerText.Substring(start_thumb, end_thumb - start_thumb + 4);
                            reddit_entry.thumbnail = thumb_url;
                            Console.WriteLine("title: " + title + "thumb: " + thumb_url);
                        }
                    }
                    //reddit_entry.author = author;
                    //reddit_entry.title = title;
                    tempPost.Add(reddit_entry);
                    //Console.WriteLine("Title: "+ reddit_entry.title + "\nAuthor: "+ reddit_entry.author);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }         

            return tempPost;
        }
    }
}
