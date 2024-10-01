using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Data.Screen {
    public class FolderXmlElement {
        [XmlElement("l")]
        public List<LXmlElement> LXmlElementList {
            get; set;
        }
    }
}