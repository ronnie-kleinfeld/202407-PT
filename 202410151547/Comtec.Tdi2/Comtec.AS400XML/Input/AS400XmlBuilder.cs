using Comtec.AS400XML.Input.Model.Screen;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Comtec.AS400XML.Input {
    public class AS400XmlBuilder {
        public string ConvertToString(ScreenInputXmlRoot screenInputXmlRoot, bool indentXml) {
            XmlSerializer serializer = new XmlSerializer(typeof(ScreenInputXmlRoot));

            using (MemoryStream memoryStream = new MemoryStream()) {
                XmlWriterSettings settings = new XmlWriterSettings {
                    Encoding = Encoding.GetEncoding("Windows-1255"),
                    Indent = indentXml
                };

                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add("", ""); // remove the default namespaces that go into the 

                using (XmlWriter xmlWriter = XmlWriter.Create(memoryStream, settings)) {
                    serializer.Serialize(xmlWriter, screenInputXmlRoot, namespaces);
                }

                return Encoding.GetEncoding("Windows-1255").GetString(memoryStream.ToArray());
            }
        }

        public void WriteToFile(ScreenInputXmlRoot screenInputXmlRoot, string xmlFilePath) {
            XmlSerializer serializer = new XmlSerializer(typeof(ScreenInputXmlRoot));

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", ""); // remove the default namespaces that go into the 

            using (StreamWriter xmlWriter = new StreamWriter(xmlFilePath, false, Encoding.GetEncoding("Windows-1255"))) {
                serializer.Serialize(xmlWriter, screenInputXmlRoot, namespaces);
            }
        }
    }
}