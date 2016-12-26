using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DeskApp.Weather
{
    class ParseXML
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
    
        public Weather parseXMLDoc()
        {
            Weather tempWeather = new Weather();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xml_string);
            XmlNodeList itemRefList = xml.GetElementsByTagName("temperature");
            Console.WriteLine("Temp value: " + itemRefList[0].Attributes["value"].Value);
            tempWeather.currDegrees = Math.Round(Double.Parse(itemRefList[0].Attributes["value"].Value)).ToString();
            XmlNodeList itemDescList = xml.GetElementsByTagName("weather");         
            Console.WriteLine("Description: " + itemDescList[0].Attributes["value"].Value);
            tempWeather.currDesc = itemDescList[0].Attributes["value"].Value;
            XmlNodeList itemPlaceList = xml.GetElementsByTagName("city");
            Console.WriteLine("City value: " + itemPlaceList[0].Attributes["name"].Value);
            tempWeather.currLocation = itemPlaceList[0].Attributes["name"].Value;

            return tempWeather;
        }
    }
}
