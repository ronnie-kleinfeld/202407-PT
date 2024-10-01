using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Data.Screen {
    public class DXmlElement {
        [XmlAttribute("qtyp")]
        public string? QtypXmlAttribute {
            get; set;
        }
    }
}