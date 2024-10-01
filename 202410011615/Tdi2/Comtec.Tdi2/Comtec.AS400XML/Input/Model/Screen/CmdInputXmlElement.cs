using System.Xml.Serialization;

namespace Comtec.AS400XML.Input.Model.Screen {
    public class CmdInputXmlElement {
        [XmlElement("c")]
        public List<CInputXmlElement>? CInputXmlElementList {
            get; set;
        }

        public CmdInputXmlElement() {
            CInputXmlElementList = new List<CInputXmlElement>();
        }
    }
}