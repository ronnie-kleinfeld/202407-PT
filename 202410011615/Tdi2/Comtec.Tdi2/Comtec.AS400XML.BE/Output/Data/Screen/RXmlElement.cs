using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Data.Screen {
    public class RXmlElement {
        [XmlAttribute("rx1")]
        public string? Rx1XmlAttribute {
            get; set;
        }

        [XmlAttribute("ry1")]
        public string? Ry1XmlAttribute {
            get; set;
        }

        [XmlAttribute("rx2")]
        public string? Rx2XmlAttribute {
            get; set;
        }

        [XmlAttribute("ry2")]
        public string? Ry2XmlAttribute {
            get; set;
        }

        [XmlAttribute("rcl")]
        public string? RclXmlAttribute {
            get; set;
        }

        [XmlAttribute("ficn")]
        public string? FicnXmlAttribute {
            get; set;
        }

        [XmlAttribute("ftit")]
        public string? FtitXmlAttribute {
            get; set;
        }

        [XmlAttribute("bkg")]
        public string? BkgXmlAttribute {
            get; set;
        }
    }
}