
using System.Xml.Serialization;
using System.Xml.Linq;
using System.IO;

namespace Model.ToolsModels
{
    public class XmlHandler
    {
        object _ClassObject { get; set; }
        public XmlHandler(object classObject)
        {
            _ClassObject = classObject;
        }
        public void SaveXML(string file)
        {

            string xmlString = ConvertObjectToXMLString();
            // Save C# class object into Xml file
            XElement xElement = XElement.Parse(xmlString);
            xElement.Save(file);
        }

        public string ConvertObjectToXMLString()
        {
            string xmlString = null;
            XmlSerializer xmlSerializer = new XmlSerializer(_ClassObject.GetType());
            using (MemoryStream memoryStream = new MemoryStream())
            {
                xmlSerializer.Serialize(memoryStream, _ClassObject);
                memoryStream.Position = 0;
                xmlString = new StreamReader(memoryStream).ReadToEnd();
            }
            return xmlString;
        }

        public T ConvertXmlStringtoObject<T>(string xmlString)
        {
            T classObject;

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (StringReader stringReader = new StringReader(xmlString))
            {
                classObject = (T)xmlSerializer.Deserialize(stringReader);
            }
            return classObject;
        }
    }

}
