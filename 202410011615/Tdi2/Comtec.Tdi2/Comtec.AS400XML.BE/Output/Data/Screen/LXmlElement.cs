using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Data.Screen {
    public class LXmlElement {
        [XmlAttribute("lk")]
        public string? LkXmlAttribute {
            get; set;
        }

        [XmlAttribute("ltr")]
        public string? LtrXmlAttribute {
            get; set;
        }

        [XmlAttribute("lon")]
        public string? LonXmlAttribute {
            get; set;
        }

        [XmlAttribute("ln")]
        public string? LnXmlAttribute {
            get; set;
        }

        [XmlAttribute("icn")]
        public string? IcnXmlAttribute {
            get; set;
        }

        [XmlAttribute("fcl")]
        public string? FclXmlAttribute {
            get; set;
        }
    }
}