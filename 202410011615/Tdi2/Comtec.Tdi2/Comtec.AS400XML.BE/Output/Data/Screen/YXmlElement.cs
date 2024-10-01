using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Data.Screen {
    public class YXmlElement {
        [XmlAttribute("yt")]
        public string? YtXmlAttribute {
            get; set;
        }

        [XmlAttribute("ys")]
        public string? YsXmlAttribute {
            get; set;
        }

        [XmlAttribute("yn")]
        public string? YnXmlAttribute {
            get; set;
        }

        [XmlAttribute("bkg")]
        public string? BkgXmlAttribute {
            get; set;
        }
    }
}