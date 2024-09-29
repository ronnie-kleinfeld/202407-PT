using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Data.Screen {
    public class CmdXmlElement {
        [XmlElement("c")]
        public List<CXmlElement> CXmlElementList {
            get; set;
        }
    }
}