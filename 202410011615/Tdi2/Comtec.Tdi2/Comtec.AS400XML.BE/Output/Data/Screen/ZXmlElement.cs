using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Data.Screen {
    public class ZXmlElement {
        [XmlAttribute("zln")]
        public string? ZlnXmlAttribute {
            get; set;
        }

        [XmlAttribute("zcl")]
        public string? ZclXmlAttribute {
            get; set;
        }
    }
}