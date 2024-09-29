using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Data.Screen {
    public class RctBkgXmlElement {
        [XmlElement("r")]
        public List<RXmlElement> RXmlElementList {
            get; set;
        }
    }
}