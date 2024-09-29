using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Data.Screen {
    public class FieldsXmlElement {
        [XmlElement("f")]
        public List<FXmlElement> FXmlElementList {
            get; set;
        }
    }
}