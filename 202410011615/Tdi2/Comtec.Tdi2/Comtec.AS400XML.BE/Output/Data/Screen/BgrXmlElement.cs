using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Data.Screen {
    public class BgrXmlElement {
        [XmlElement("z")]
        public List<ZXmlElement> ZXmlElementList {
            get; set;
        }
    }
}