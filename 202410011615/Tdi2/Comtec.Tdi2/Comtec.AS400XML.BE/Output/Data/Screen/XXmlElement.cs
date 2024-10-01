using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Data.Screen {
    public class XXmlElement {
        [XmlAttribute("xs")]
        public string? XsXmlAttribute {
            get; set;
        }

        [XmlAttribute("xn")]
        public string? XnXmlAttribute {
            get; set;
        }

        [XmlAttribute("bkg")]
        public string? BkgXmlAttribute {
            get; set;
        }
    }
}