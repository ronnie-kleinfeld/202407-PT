using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Data.Screen {
    public class BXmlElement {
        [XmlAttribute("op")]
        public string? OpXmlAttribute {
            get; set;
        }

        [XmlAttribute("nm")]
        public string? NmXmlAttribute {
            get; set;
        }

        [XmlAttribute("ro")]
        public string? RoXmlAttribute {
            get; set;
        }
    }
}