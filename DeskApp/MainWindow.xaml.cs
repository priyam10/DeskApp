using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using DeskApp.Weather;
using DeskApp.Reddit;

namespace DeskApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string API_KEY = "2db5a5da5c3a27d247f2423a226fd5bf";
        // Query URLs. Replace @LOC@ with the location.
        private const string CurrentUrl = "http://api.openweathermap.org/data/2.5/weather?" + "q=@LOC@&mode=xml&units=imperial&APPID=" + API_KEY;
        private const string ForecastUrl = "http://api.openweathermap.org/data/2.5/forecast?" + "q=@LOC@&mode=xml&units=imperial&APPID=" + API_KEY; 
        private ListView lvwTemps;


        public MainWindow()
        {
            InitializeComponent();
            startRSSFeed();
            DataContext = new WeatherViewModel();
        }

        public void startRSSFeed()
        {
            
        }

        //private void btnForecast_Click2(object sender, RoutedEventArgs e)
        //{
        //    // Compose the query URL.
        //    string url = ForecastUrl.Replace("@LOC@", txtLocation.Text);
        //    txtXml.Text = GetFormattedXml(url);
        //}

        //// Get current conditions.
        //private void btnForecast_Click(object sender, RoutedEventArgs e)
        //{
        //    // Compose the query URL.
        //    string url = CurrentUrl.Replace("@LOC@", txtLocation.Text);
        //    //txtXml.Text = GetFormattedXml(url);

        //    ParseXML parser = new ParseXML();
        //    parser.GetFormattedXml(url);
        //    parser.parseXMLDoc(NowWeather);
        //}

        // Return the XML result of the URL.
        private string GetFormattedXml(string url)
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
                    XmlTextWriter xml_text_writer =
                        new XmlTextWriter(string_writer);
                    xml_text_writer.Formatting = Formatting.Indented;
                    xml_document.WriteTo(xml_text_writer);

                    // Return the result.
                    return string_writer.ToString();
                }
            }
        }
        // List the temperatures.
        //private void ListTemperatures(XmlDocument xml_doc)
        //{
        //    lvwTemps.Items.Clear();

        //    // Loop throuh the time entries.
        //    string last_day = "";
        //    foreach (XmlNode time_node in xml_doc.SelectNodes("//time"))
        //    {
        //        // Get the start date and time.
        //        XmlAttribute time_attr = time_node.Attributes["from"];
        //        DateTime start_time = DateTime.Parse(time_attr.Value);

        //        // Convert from UTC to local time.
        //        start_time = start_time.ToLocalTime();

        //        // Add 90 minutes to get to the middle of the interval.
        //        start_time += new TimeSpan(1, 30, 0);

        //        // Get the temperature node.
        //        XmlNode temp_node =
        //            time_node.SelectSingleNode("temperature");
        //        XmlAttribute temp_attr = temp_node.Attributes["value"];
        //        float temp = 0;
        //        if (temp_attr != null)
        //            temp = float.Parse(temp_attr.Value.ToString());

        //        ListViewItem item;
        //        if (start_time.DayOfWeek.ToString() == last_day)
        //            item = lvwTemps.Items.Add("");
        //        else
        //        {
        //            last_day = start_time.DayOfWeek.ToString();
        //            item = lvwTemps.Items.Add(last_day);
        //        }
        //        item.SubItems.Add(start_time.ToShortTimeString());
        //        item.SubItems.Add(temp.ToString("0.00"));
        //    }
        //}


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (rect.Visibility == System.Windows.Visibility.Collapsed)
            {
                rect.Visibility = System.Windows.Visibility.Visible;
                (sender as Button).Content = "<";
                tabView.Visibility = System.Windows.Visibility.Visible;
                gridCol1.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                rect.Visibility = System.Windows.Visibility.Collapsed;
                (sender as Button).Content = ">";
                tabView.Visibility = System.Windows.Visibility.Collapsed;
                gridCol1.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void newTab_Clicked(object sender, SelectionChangedEventArgs e)
        {/*
            var item = sender as TabControl;
            if (item != null)
            {
                var selected = item.SelectedItem as TabItem;
                if (selected.Header != null)
                {
                    switch (selected.Header.ToString())
                    {
                        case "Weather":
                            MessageBox.Show("Waether");
                                                        
                            break;
                        case "Reddit":
                            MessageBox.Show("Reddit");
                            break;
                        case "Imgur":
                            MessageBox.Show("IMmgur");
                            break;
                    }
                }

            }*/
        }
    }
}
